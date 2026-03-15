import { Component } from '@angular/core';
import { Auth } from '../../../core/services/auth';
import { RouterLink } from '@angular/router';
import { LogoutButton } from '../logout-button/logout-button';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink, LogoutButton],
  templateUrl: './navbar.html',
})
export class Navbar {}
