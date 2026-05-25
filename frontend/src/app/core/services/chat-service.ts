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
export class ChatService {
  private readonly hubs = environment.hubsUrl;
  private readonly api = environment.apiUrl;
  private readonly http = inject(HttpClient);

  private authService = inject(AuthService);

  messages = signal<ChatMessage[]>([]);

  addToList(message: ChatMessage) {
    this.messages.update((messages) => [...messages, message]);
  }

  // rest api methods
  getList() {
    return this.http.get<ChatMessage[]>(`${this.api}/chatmessage/list`).pipe(
      tap((list) => {
        this.messages.set(list);
        console.log(this.messages());
      }),
    );
  }

  getCurrentUserId() {
    return this.authService.currentUser()?.id;
  }

  // signal R methods
  private hub = new SignalR.HubConnectionBuilder()
    .withUrl(`${this.hubs}/chat`, {
      accessTokenFactory: () => this.authService.getAccessToken() ?? '',
    })
    .withAutomaticReconnect()
    .build();

  constructor() {
    this.onMessage((message: ChatMessage) => {
      console.log(message);
      this.addToList(message);
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

  onMessage(cb: (message: ChatMessage) => void) {
    this.hub.on('ReceiveMessage', cb);
  }

  sendMessage(content: string) {
    this.hub.invoke('SendMessage', content);
  }
}
