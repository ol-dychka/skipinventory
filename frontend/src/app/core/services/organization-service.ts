import { computed, inject, Injectable, signal } from '@angular/core';
import {
  CreateOrganizationRequest,
  OrganizationModel,
  CreateOrganizationResponse,
  OrganizationPreviewModel,
} from '../models/organization';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable, switchMap, tap, throwError } from 'rxjs';
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

  readonly role = computed(() => {
    const memberships = this.authService.currentUser()?.memberships;
    const organizationId = this.currentOrganization()?.id;

    if (memberships === undefined || organizationId === undefined) return '';

    return memberships.find((m) => m.organizationId === organizationId)?.role;
  });

  isOwner() {
    return this.role() === 'Owner';
  }

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

  exit(): Observable<void> {
    return this.authService.refresh().pipe(
      tap(() => this.currentOrganization.set(null)),
      map(() => void 0),
    );
  }

  get(organizationId: string) {
    return this.http.get<OrganizationModel>(`${this.api}/organization/${organizationId}`).pipe(
      tap((organization) => {
        this.currentOrganization.set(organization);
        console.log(this.currentOrganization());
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
    console.log(this.authService.currentUser()!.id);
    return this.http.post<void>(`${this.api}/organization/${organizationId}/request`, {});
  }

  resolveRequest(requestId: string, decision: boolean) {
    return this.http.post(`${this.api}/organization/${requestId}/${decision}`, {}).pipe(
      tap(() => this.updateJoinRequests(requestId)),
      catchError((err) => {
        console.log(err);
        return throwError(() => err);
      }),
    );
  }

  updateJoinRequests(requestId: string) {
    this.currentOrganization.update((org) => {
      if (!org) return org;

      const updated = org.joinRequests.filter((r) => r.id !== requestId);
      return {
        ...org,
        joinRequests: updated,
      };
    });

    console.log(this.currentOrganization());
  }
}
