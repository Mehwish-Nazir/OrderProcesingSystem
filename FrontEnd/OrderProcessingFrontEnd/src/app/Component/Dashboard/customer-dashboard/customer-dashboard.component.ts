import { Component } from '@angular/core';
import { SidebarComponent } from '../../Sidebar/sidebar.component';
import { Routes } from '@angular/router';
import { HeaderComponent } from '../../header/header.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-customer-dashboard',
  imports: [SidebarComponent, HeaderComponent, RouterOutlet],
  standalone:true,
  templateUrl: './customer-dashboard.component.html',
  styleUrl: './customer-dashboard.component.css'
})
export class CustomerDashboardComponent {

}
