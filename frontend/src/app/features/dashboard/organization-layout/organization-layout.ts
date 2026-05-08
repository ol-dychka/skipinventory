import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Sidebar } from '../sidebar/sidebar';

@Component({
  selector: 'app-organization-layout',
  imports: [RouterOutlet, Sidebar],
  templateUrl: './organization-layout.html',
})
export class OrganizationLayout {}
