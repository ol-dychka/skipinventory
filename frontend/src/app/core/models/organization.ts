export interface OrganizationModel {
  id: string;
  name: string;
  members: MemberModel[];
  joinRequests: JoinRequestModel[];
}

export interface OrganizationPreviewModel {
  id: string;
  name: string;
}

export interface CreateOrganizationResponse {
  organizationId: string;
}

export interface CreateOrganizationRequest {
  name: string;
}

interface MemberModel {
  role: string;
  userId: string;
  userName: string;
  userEmail: string;
}

interface JoinRequestModel {
  userId: string;
  userName: string;
  userEmail: string;
}
