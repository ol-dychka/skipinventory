import { Component, inject } from '@angular/core';
import { Navbar } from '../../navbar/navbar';
import { OrganizationService } from '../../../core/services/organization-service';
import { AuthService } from '../../../core/services/auth-service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.html',
})
export class Dashboard {}
