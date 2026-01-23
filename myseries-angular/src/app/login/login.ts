import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  standalone: true,
  selector: 'app-login',
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.scss']
})
export class LoginComponent {
  userName = '';
  password = '';
  error?: string;
  creatingUser = false;

  constructor(
    private auth: AuthService,
    private router: Router
  ) {}

  login() {
  this.error = undefined;

  this.auth.login(this.userName, this.password).subscribe({
    next: () => this.router.navigateByUrl('/menu'),
    error: () => {
      this.error = 'Usuario o contraseña incorrectos';
      this.creatingUser = true;
    }
  });
  }

  createUser() {
    this.auth.createUser({
      userName: this.userName,
      password: this.password,
      email: undefined,
      notificationsByEmail: false,
      notificationsByApp: true
    }).subscribe({
      next: () => this.login(),
      error: err => this.error = err.error?.error?.message
    });
  }

}
