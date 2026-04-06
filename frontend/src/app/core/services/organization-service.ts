import { inject, Injectable, signal } from '@angular/core';
import {
  CreateOrganizationRequest,
  OrganizationModel,
  CreateOrganizationResponse,
  OrganizationPreviewModel,
} from '../models/organization';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map, Observable, switchMap, tap } from 'rxjs';
import { AuthService } from './auth-service';

@Injectable({
  providedIn: 'root',
})
export class OrganizationService {
  private readonly api = environment.apiUrl;
  private readonly http = inject(HttpClient);
  private authService = inject(AuthService);

  readonly currentOrganization = signal<OrganizationModel | null>(null);
  readonly availableOrganizations = signal<OrganizationPreviewModel[]>([]);

  create(payload: CreateOrganizationRequest): Observable<string> {
    return this.http.post<CreateOrganizationResponse>(`${this.api}/organization`, payload).pipe(
      switchMap(({ organizationId }) =>
        this.authService.refresh(organizationId).pipe(map(() => organizationId)),
      ),
      switchMap((organizationId) => this.get(organizationId).pipe(map(() => organizationId))),
    );
  }

  access(organizationId: string): Observable<void> {
    return this.authService
      .refresh(organizationId)
      .pipe(switchMap(() => this.get(organizationId).pipe(map(() => void 0))));
  }

  get(organizationId: string) {
    return this.http.get<OrganizationModel>(`${this.api}/organization/${organizationId}`).pipe(
      tap((organization) => {
        this.currentOrganization.set(organization);
        console.log(organization);
      }),
    );
  }

  getPreviews() {
    return this.http.get<OrganizationPreviewModel[]>(`${this.api}/organization/list`).pipe(
      tap((list) => {
        this.availableOrganizations.set(list);
        console.log(this.availableOrganizations);
      }),
    );
  }

  request(organizationId: string) {
    return this.http.post<void>(`${this.api}/organization/${organizationId}/request`, {});
  }
}
