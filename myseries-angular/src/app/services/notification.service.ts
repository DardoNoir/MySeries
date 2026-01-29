import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";


@Injectable({ providedIn: 'root' })
export class NotificationService {
  private baseUrl = '/api/app/notifications';

  constructor(private http: HttpClient) {}

  getUnread(userId: number) {
    return this.http.get<any[]>(`${this.baseUrl}/unread`, {
      params: { userId }
    });
  }

  getAll(userId: number) {
    return this.http.get<any[]>(`${this.baseUrl}/all`, {
      params: { userId }
    });
  }

  getUnreadCount(userId: number) {
    return this.http.get<number>(`${this.baseUrl}/unread-count`, {
      params: { userId }
    });
  }

  markAsRead(notificationId: number) {
    return this.http.get(`${this.baseUrl}/mark-readen`, {
      params: { notificationId }
    });
  }
}

