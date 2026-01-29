import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationService } from '../services/notification.service';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-notifications',
  imports: [CommonModule],
  templateUrl: './notifications.html',
  styleUrls: ['./notifications.scss']
})
export class NotificationsComponent implements OnInit {

  unread: any[] = [];
  all: any[] = [];

  userId!: number;

  constructor(
    private notifications: NotificationService,
    private auth: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const user = this.auth.getUser();
    this.userId = user.id;

    this.loadNotifications();
  }

  loadNotifications() {
    this.notifications.getUnread(this.userId)
      .subscribe(data => this.unread = data);

    this.notifications.getAll(this.userId)
      .subscribe(data => this.all = data);
  }

  markRead(notificationId: number) {
    this.notifications.markAsRead(notificationId)
      .subscribe(() => {
        // 🔄 refresca listas
        this.loadNotifications();
      });
  }

  goBack(): void {
    this.router.navigateByUrl('/menu');
  }
}
