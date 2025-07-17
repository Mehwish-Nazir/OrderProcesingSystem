import { Component , Input } from '@angular/core';
import { InputDecorator } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
@Component({
  selector: 'app-sidebar',
  imports: [CommonModule, RouterOutlet],
  standalone:true,
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
 @Input() role: string = '';  // Pass 'Admin' or 'Customer'

  get isAdmin() {
    return this.role === 'Admin';
  }

  get isCustomer() {
    return this.role === 'Customer';
  }
}
