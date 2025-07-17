import { Component } from '@angular/core';
import { SidebarComponent } from '../../Sidebar/sidebar.component';
import { routes } from '../../../app.routes';
import { HeaderComponent } from '../../header/header.component';
import { RouterOutlet } from '@angular/router';
@Component({
  selector: 'app-admin-dashboard',
  imports: [SidebarComponent, HeaderComponent,RouterOutlet],
  standalone:true,
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.css'
})
export class AdminDashboardComponent {

}
