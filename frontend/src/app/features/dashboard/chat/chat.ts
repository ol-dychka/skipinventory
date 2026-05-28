import { Component, inject, OnInit, signal } from '@angular/core';
import { ChatService } from '../../../core/services/chat-service';
import { FormsModule } from '@angular/forms';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-chat',
  imports: [FormsModule, NgClass],
  templateUrl: './chat.html',
})
export class Chat implements OnInit {
  chatService = inject(ChatService);

  content = signal<string>('');

  ngOnInit(): void {
    this.chatService.joinRoom();
  }

  handleMessage() {
    if (this.isContent()) this.chatService.sendMessage(this.content());
  }

  updateContent(value: string) {
    this.content.set(value);
  }

  isContent() {
    if (this.content().trim().length > 0) return true;
    return false;
  }
}
