import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { CommonModule } from '@angular/common';
import { NotificationService } from '../services/notification.service';

@Component({
  standalone: true,
  selector: 'app-menu',
  templateUrl: './menu.html',
  styleUrls: ['./menu.scss'],
  imports:[CommonModule]
})
export class MenuComponent {
  constructor(
    private router: Router,
    private auth: AuthService,
    private notifications: NotificationService,
  ) {}

  // Ir a la Búsqueda 
  goToSearch() {
    this.router.navigateByUrl('/series');
  }

  // Ir a la Lista de Seguimiento
  goToWatchlist() {
  this.router.navigateByUrl('/watchlist');
  }

  // Comprobación de Administrador
  isAdmin(): boolean {
    return this.auth.isAdmin();
  }

  // Ir a las estadísticas del Monitoreo
  goToMonitoring() {
    this.router.navigateByUrl('/monitoring');
  }

  // Mostrar cantidad de Notificaciones sin leer desde el menú
  unreadCount = 0;
  ngOnInit() {
    const user = this.auth.getUser();
    this.notifications.getUnreadCount(user.id)
      .subscribe(c => this.unreadCount = c);
  }

  // Ir a las notificaciones
  goToNotifications() {
    this.router.navigateByUrl('/notifications');
  }

  // Cerrar sesión
  logout() {
    this.auth.logout();
    this.router.navigateByUrl('/login');
  }
}
