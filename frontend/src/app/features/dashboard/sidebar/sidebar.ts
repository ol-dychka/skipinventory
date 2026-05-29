import { NgClass } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { SidebarLink } from './sidebar-link/sidebar-link';

@Component({
  selector: 'app-sidebar',
  imports: [RouterLink, SidebarLink],
  templateUrl: './sidebar.html',
})
export class Sidebar implements OnInit {
  private router = inject(Router);

  ngOnInit(): void {
    const url = this.router.url;
    const page = url.split('/').filter(Boolean).pop() ?? '';
    this.page.set(page);
  }
  page = signal<string>('dashboard');

  isCurrent(page: string) {
    return this.page() === page;
  }

  switchPage(page: string) {
    this.page.set(page);
  }
}
