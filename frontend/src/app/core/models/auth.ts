export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  name: string;
  isOwner: boolean;
}

export interface AuthResponse {
  accessToken: string;
}
