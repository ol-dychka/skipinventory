import { Component } from '@angular/core';
import { AuthInput } from '../../../shared/components/auth-input/auth-input';

@Component({
  selector: 'app-join-organization',
  imports: [AuthInput],
  templateUrl: './join-organization.html',
})
export class JoinOrganization {}
