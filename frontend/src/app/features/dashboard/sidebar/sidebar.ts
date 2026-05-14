import { NgClass } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  imports: [RouterLink, NgClass],
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
