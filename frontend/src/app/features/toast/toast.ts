import { Component, inject, OnInit } from '@angular/core';
import { NotificationsService } from '../../core/services/notifications-service';

@Component({
  selector: 'app-toast',
  imports: [],
  templateUrl: './toast.html',
})
export class Toast implements OnInit {
  notificationsService = inject(NotificationsService);

  ngOnInit(): void {
    this.notificationsService.joinRoom();
  }
}
