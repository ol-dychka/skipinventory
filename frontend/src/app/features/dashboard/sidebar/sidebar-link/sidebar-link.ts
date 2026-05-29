import { NgClass } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-sidebar-link',
  imports: [NgClass],
  templateUrl: './sidebar-link.html',
})
export class SidebarLink {
  @Input() isActive: boolean = false;
  @Input() name: string = '';
}
