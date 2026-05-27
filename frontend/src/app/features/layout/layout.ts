import { Component } from '@angular/core';
import { Navbar } from '../navbar/navbar';
import { RouterOutlet } from '@angular/router';
import { Toast } from '../toast/toast';

@Component({
  selector: 'app-layout',
  imports: [Navbar, RouterOutlet, Toast],
  templateUrl: './layout.html',
})
export class Layout {}
