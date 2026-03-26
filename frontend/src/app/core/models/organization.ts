export interface OrganizationModel {
  id: string;
  name: string;
}

export interface OrganizationResponse {
  organizationId: string;
}

export interface CreateOrganizationRequest {
  name: string;
}

export interface JoinOrganizationRequest {
  id: string;
}
