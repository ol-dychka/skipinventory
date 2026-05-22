import { Injectable } from '@angular/core';
import * as SignalR from '@microsoft/signalr';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  private readonly hubs = environment.hubsUrl;

  private hub = new SignalR.HubConnectionBuilder()
    .withUrl(`${this.hubs}/chat`)
    .withAutomaticReconnect()
    .build();

  constructor() {
    this.onMessage((m) => console.log(m));
  }

  private connectionPromise: Promise<void> | null = null;
  connect(): Promise<void> {
    if (!this.connectionPromise) {
      this.connectionPromise = this.hub.start();
    }
    return this.connectionPromise;
  }

  async joinRoom(roomId: string): Promise<void> {
    await this.connect();
    return this.hub.invoke('JoinRoom', roomId);
  }

  onMessage(cb: (msg: string) => void) {
    this.hub.on('ReceiveMessage', cb);
  }

  sendMessage(roomId: string, message: string) {
    this.hub.invoke('SendMessage', roomId, message);
  }
}
