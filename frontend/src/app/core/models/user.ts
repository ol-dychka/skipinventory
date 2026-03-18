import { OrganizationModel } from './organization';

export interface UserModel {
  id: string;
  name: string;
  email: string;
  role: string;
  organization?: OrganizationModel;
}

export interface UserResponse {
  user: UserModel;
}
