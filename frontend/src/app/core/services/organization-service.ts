import { inject, Injectable, signal } from '@angular/core';
import {
  CreateOrganizationRequest,
  JoinOrganizationRequest,
  OrganizationModel,
  CreateOrganizationResponse,
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

  create(payload: CreateOrganizationRequest): Observable<OrganizationModel> {
    return this.http.post<CreateOrganizationResponse>(`${this.api}/organization`, payload).pipe(
      switchMap(({ organizationId }) =>
        this.authService.refresh({ organizationId }).pipe(map(() => organizationId)),
      ),
      switchMap((organizationId) => this.get(organizationId)),
    );
  }

  get(organizationId: string) {
    return this.http.get<OrganizationModel>(`${this.api}/organization/${organizationId}`).pipe(
      tap((organization) => {
        this.currentOrganization.set(organization);
        console.log(organization);
      }),
    );
  }

  request(payload: JoinOrganizationRequest) {
    //request to join
    return;
  }
}
