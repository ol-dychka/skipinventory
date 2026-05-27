import { inject, Injectable, signal } from '@angular/core';
import * as SignalR from '@microsoft/signalr';
import { environment } from '../../../environments/environment';
import { ChatMessage } from '../models/chat-message';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { AuthService } from './auth-service';

@Injectable({
  providedIn: 'root',
})
export class NotificationsService {
  private readonly hubs = environment.hubsUrl;

  private authService = inject(AuthService);

  notifications = signal<string[]>([]);

  addToList(notification: string) {
    this.notifications.update((notifications) => [...notifications, notification]);
  }

  // signal R methods
  private hub = new SignalR.HubConnectionBuilder()
    .withUrl(`${this.hubs}/notifications`, {
      accessTokenFactory: () => this.authService.getAccessToken() ?? '',
    })
    .withAutomaticReconnect()
    .build();

  constructor() {
    this.onNotification((notification: string) => {
      console.log(notification);
      this.addToList(notification);
    });
  }

  private connectionPromise: Promise<void> | null = null;
  connect(): Promise<void> {
    if (!this.connectionPromise) {
      this.connectionPromise = this.hub.start();
    }
    return this.connectionPromise;
  }

  async joinRoom(): Promise<void> {
    await this.connect();
    return this.hub.invoke('JoinRoom');
  }

  onNotification(cb: (notification: string) => void) {
    this.hub.on('ReceiveNotification', cb);
  }
}
