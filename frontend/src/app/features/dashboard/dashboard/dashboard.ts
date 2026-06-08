import { Component, inject } from '@angular/core';
import { Navbar } from '../../navbar/navbar';
import { OrganizationService } from '../../../core/services/organization-service';
import { AuthService } from '../../../core/services/auth-service';
import { RouterLink } from '@angular/router';
import { SaleRecordService } from '../../../core/services/sale-record-service';

@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.html',
})
export class Dashboard {
  authService = inject(AuthService);
  saleRecordService = inject(SaleRecordService);
}
