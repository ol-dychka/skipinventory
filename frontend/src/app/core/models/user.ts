interface MembershipModel {
  role: string;
  organizationId: string;
  organizationName: string;
}

export interface UserModel {
  id: string;
  name: string;
  email: string;
  memberships: MembershipModel[];
}
