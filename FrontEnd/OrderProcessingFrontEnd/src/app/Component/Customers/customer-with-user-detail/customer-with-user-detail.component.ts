import { Component } from '@angular/core';
import { Customers } from '../../../../Models/Customers';
import { CustomerWithUser } from '../../../../Models/Customers';
import { CustomersService } from '../../../Services/Customer/customers.service';
import { routes } from '../../../app.routes';
import { CommonModule, NgFor, NgIf } from '@angular/common';
import { Route,Router, RouterModule,RouterOutlet } from '@angular/router';
@Component({
  selector: 'app-customer-with-user-detail',
  standalone:true,
  imports: [CommonModule,NgIf, NgFor],
  templateUrl: './customer-with-user-detail.component.html',
  styleUrl: './customer-with-user-detail.component.css'
})
export class CustomerWithUserDetailComponent {

 customers:CustomerWithUser[]=[];
 error: string | null = null;  // Define the error property

  constructor(private customerService:CustomersService){};
  
  getCustomerWithUser(){
    return this.customerService.getCustomerWithUser().subscribe(
        (data)=>{
          console.log('Full response:', data);
            this.customers=data
        },(error)=>{
          this.error=error;
         console.log("The error is :", error)
        }
      
    );
  }
}
