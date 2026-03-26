import { inject, Injectable, signal } from '@angular/core';
import {
  CreateOrganizationRequest,
  JoinOrganizationRequest,
  OrganizationModel,
  OrganizationResponse,
} from '../models/organization';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth-service';

@Injectable({
  providedIn: 'root',
})
export class OrganizationService {
  private readonly api = environment.apiUrl;
  private readonly http = inject(HttpClient);

  readonly currentOrganization = signal<OrganizationModel | null>(null);

  create(payload: CreateOrganizationRequest) {
    return this.http.post<OrganizationResponse>(`${this.api}/auth/login`, payload);
  }

  join(payload: JoinOrganizationRequest) {
    return;
  }
}
