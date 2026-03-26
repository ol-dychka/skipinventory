import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';
import { Dashboard } from './features/dashboard/dashboard/dashboard';
import { authGuard } from './core/guards/auth-guard';
import { noAuthGuard } from './core/guards/no-auth-guard';
import { Layout } from './features/layout/layout';
import { JoinOrganization } from './features/dashboard/join-organization/join-organization';
import { CreateOrganization } from './features/dashboard/create-organization/create-organization';
import { ChooseOrganization } from './features/dashboard/choose-organization/choose-organization';

export const routes: Routes = [
  { path: 'login', component: Login, canActivate: [noAuthGuard] },
  { path: 'register', component: Register, canActivate: [noAuthGuard] },
  {
    path: '',
    component: Layout,
    canActivate: [authGuard],
    children: [
      { path: 'dashboard', component: Dashboard }, // add organization-guard
      { path: 'join-organization', component: JoinOrganization },
      { path: 'create-organization', component: CreateOrganization },
      { path: 'choose-organization', component: ChooseOrganization },

      { path: '', redirectTo: 'choose-organization', pathMatch: 'full' },
    ],
  },
];
