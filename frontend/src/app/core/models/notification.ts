export interface Notification {
  id?: number;
  type: NotificationType;
  message: string;
  isDisappearing: boolean;
}

export type NotificationType = 'info' | 'success' | 'error';
