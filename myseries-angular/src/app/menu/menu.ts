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

  goToSearch() {
    this.router.navigateByUrl('/series');
  }

  goToWatchlist() {
  this.router.navigateByUrl('/watchlist');
  }

  isAdmin(): boolean {
    return this.auth.isAdmin();
  }

  goToMonitoring() {
    this.router.navigateByUrl('/monitoring');
  }

  unreadCount = 0;
  ngOnInit() {
    const user = this.auth.getUser();
    this.notifications.getUnreadCount(user.id)
      .subscribe(c => this.unreadCount = c);
  }

  goToNotifications() {
    this.router.navigateByUrl('/notifications');
  }


  logout() {
    this.auth.logout();
    this.router.navigateByUrl('/login');
  }
}
