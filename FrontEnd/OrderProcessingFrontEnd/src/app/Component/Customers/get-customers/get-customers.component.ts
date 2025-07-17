import { Component } from '@angular/core';
import { CustomersService } from '../../../Services/Customer/customers.service';
import { Customers } from '../../../../Models/Customers';
import { Routes , RouterModule,RouterOutlet,Router, RouterLink} from '@angular/router';
import { routes } from '../../../app.routes';
import { CommonModule, NgFor } from '@angular/common';

@Component({
  selector: 'app-get-customers',
  imports: [CommonModule,NgFor, RouterOutlet],
  templateUrl: './get-customers.component.html',
  styleUrl: './get-customers.component.css'
})
export class GetCustomersComponent {

  customers:Customers[]=[]; // to get the array of data in interface from backeend
  //in service file provide customers array ahead of  oberavale<Customers[]>
  constructor(private customerService:CustomersService,    
              private router: Router
            ){};
  getCustomers():void{
    this.customerService.getCustomer().subscribe(
      data => this.customers = data,
        error => console.error('Error fetching customers:', error)
    );
  }
}
