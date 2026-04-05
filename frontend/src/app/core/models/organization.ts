export interface OrganizationModel {
  id: string;
  name: string;
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
