import { Component, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Route, RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { LoginResponse,LoginRequest } from '../../../Models/Login';
import { AuthService } from '../../Services/auth/auth.service';
import { Router } from '@angular/router';
import { OutputDecorator, EventEmitter, InputDecorator, Input } from '@angular/core';
import { HeaderService } from '../../Services/Header/header.service';
import { AddCustomerComponent } from '../Customers/add-customer/add-customer.component';
import { Customers } from '../../../Models/Customers';
@Component({
  selector: 'app-logout',
  imports: [],
  templateUrl: './logout.component.html',
  styleUrl: './logout.component.css'
})
export class LogoutComponent {

  @Input() customerName!: string;  //rto accept dynamic name 

@Output() logOut=new EventEmitter<string>();

  constructor(private logoutService:AuthService , 
    private router:Router ,
    private loggingAndErorService: HeaderService){}

  onLogout(){
    this.logoutService.logout().subscribe(
      () => {
        alert( `${this.customerName} ,have been logged out successfully.`);
        this.router.navigate(['/login']); // Redirect to login page after logout
        //emit logout to reuse in other componets 
        this.logOut.emit();
      },
      (error) => {
        alert('Error logging out. Please try again.');
         this.loggingAndErorService.error(error)  ;
}
    );
  }
}
