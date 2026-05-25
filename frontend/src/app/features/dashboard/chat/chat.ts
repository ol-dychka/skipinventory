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
    this.chatService.joinRoom();
  }

  handleMessage() {
    this.chatService.sendMessage('new message');
  }
}
