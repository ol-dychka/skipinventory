import { OrganizationModel } from './organization';

interface Membership {
  role: string;
  organizationId: string;
  organizationName: string;
}

export interface UserModel {
  id: string;
  name: string;
  email: string;
  memberships: Membership[];
}

export interface UserResponse {
  user: UserModel;
}
