import { Component, inject, OnInit } from '@angular/core';
import { NotificationsService } from '../../core/services/notifications-service';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-toast',
  imports: [NgClass],
  templateUrl: './toast.html',
})
export class Toast implements OnInit {
  notificationsService = inject(NotificationsService);

  ngOnInit(): void {
    this.notificationsService.joinRoom();
  }
}
