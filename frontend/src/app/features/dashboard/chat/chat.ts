import { Component, inject, OnInit } from '@angular/core';
import { ChatService } from '../../../core/services/chat-service';

@Component({
  selector: 'app-chat',
  imports: [],
  templateUrl: './chat.html',
})
export class Chat implements OnInit {
  chatService = inject(ChatService);

  ngOnInit(): void {
    this.chatService.joinRoom('1');
  }

  messages = [
    { id: 1, text: 'Hello!', mine: false },
    { id: 2, text: 'Hey 👋', mine: true },
  ];

  handleMessage() {
    this.chatService.sendMessage('1', 'a');
  }
}
