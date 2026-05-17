import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';
import { Dashboard } from './features/dashboard/dashboard/dashboard';
import { authGuard } from './core/guards/auth-guard';
import { noAuthGuard } from './core/guards/no-auth-guard';
import { orgGuard } from './core/guards/org-guard';
import { Layout } from './features/layout/layout';
import { JoinOrganization } from './features/dashboard/join-organization/join-organization';
import { CreateOrganization } from './features/dashboard/create-organization/create-organization';
import { ChooseOrganization } from './features/dashboard/choose-organization/choose-organization';
import { JoinRequests } from './features/dashboard/join-requests/join-requests';
import { OrganizationLayout } from './features/dashboard/organization-layout/organization-layout';
import { Members } from './features/dashboard/members/members';
import { Products } from './features/dashboard/products/products';
import { productGuard } from './core/guards/product-guard';
import { SaleRecords } from './features/dashboard/sale-records/sale-records';
import { ModelTraining } from './features/dashboard/model-training/model-training';

export const routes: Routes = [
  { path: 'login', component: Login, canActivate: [noAuthGuard] },
  { path: 'register', component: Register, canActivate: [noAuthGuard] },
  {
    path: '',
    component: Layout,
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'choose-organization', pathMatch: 'full' },
      { path: 'join-organization', component: JoinOrganization },
      { path: 'create-organization', component: CreateOrganization },
      { path: 'choose-organization', component: ChooseOrganization },
      {
        path: ':organizationId',
        component: OrganizationLayout,
        canActivate: [orgGuard],
        children: [
          { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
          { path: 'dashboard', component: Dashboard },
          { path: 'members', component: Members },
          { path: 'join-requests', component: JoinRequests },
          { path: 'products', component: Products, canActivate: [productGuard] },
          { path: 'sale-records', component: SaleRecords, canActivate: [productGuard] },
          { path: 'model-training', component: ModelTraining },
        ],
      },
    ],
  },
];
