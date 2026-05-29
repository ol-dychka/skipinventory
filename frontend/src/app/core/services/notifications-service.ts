import { inject, Injectable, signal } from '@angular/core';
import * as SignalR from '@microsoft/signalr';
import { environment } from '../../../environments/environment';
import { ChatMessage } from '../models/chat-message';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { AuthService } from './auth-service';
import { Notification } from '../models/notification';

@Injectable({
  providedIn: 'root',
})
export class NotificationsService {
  private readonly hubs = environment.hubsUrl;

  private authService = inject(AuthService);

  notifications = signal<Notification[]>([]);
  private nextId = 0;

  addToList(notification: Notification) {
    notification.id = this.nextId;
    this.nextId++;

    this.notifications.update((list) => [...list, notification]);
    if (notification.isDisappearing) {
      console.log('mmm');
      setTimeout(() => {
        console.log('lll');
        this.clearFromList(notification.id);
      }, 5000);
    }
    // if isDissapearing is false user has to get rid of the notification himself
  }

  clearFromList(id?: number) {
    this.notifications.update((list) => list.filter((n) => n.id !== id));
  }

  // signal R methods
  private hub = new SignalR.HubConnectionBuilder()
    .withUrl(`${this.hubs}/notifications`, {
      accessTokenFactory: () => this.authService.getAccessToken() ?? '',
    })
    .withAutomaticReconnect()
    .build();

  constructor() {
    this.onNotification((notification: Notification) => {
      console.log(notification);
      this.addToList(notification);
      // this.authService.refresh(); doesn't work for some reason
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

  onNotification(cb: (notification: Notification) => void) {
    this.hub.on('ReceiveNotification', cb);
  }
}
