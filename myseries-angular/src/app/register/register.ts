import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,                     
  imports: [CommonModule, FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class RegisterComponent {
userName = '';
  password = '';
  email = '';
  notificationsByEmail = false;
  notificationsByApp = false;
  error?: string;

  constructor(
    private auth: AuthService,
    private router: Router
  ) {
    const data = history.state;

    this.userName = data.userName;
    this.password = data.password;
  }

  crearUsuario() {
    this.auth.createUser({
      userName: this.userName,
      password: this.password,
      email: this.email,
      notificationsByEmail: this.notificationsByEmail,
      notificationsByApp: this.notificationsByApp
    }).subscribe({
      next: user => {
        this.auth.setUser(user);     // 🔑 LOGUEA
        this.router.navigateByUrl('/menu');
      },
      error: err => {
        this.error = err.error?.message || 'Error al crear usuario';
      }
    });
  }

  goBack() {
    this.router.navigateByUrl('/login');
  }
}

