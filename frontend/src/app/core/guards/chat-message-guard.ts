import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { ChatService } from '../services/chat-service';
import { catchError, map, of } from 'rxjs';

export const chatMessageGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const chatService = inject(ChatService);

  if (chatService.messages().length > 0) {
    return true;
  }

  return chatService.getList().pipe(
    map(() => true),
    catchError(() => {
      console.log('error getting messages');
      router.navigate(['/dashboard'], { queryParams: { returnUrl: state.url } });
      return of(false);
    }),
  );
};
