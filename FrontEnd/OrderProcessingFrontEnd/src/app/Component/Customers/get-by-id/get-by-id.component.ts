import { Component } from '@angular/core';
import { Customers } from '../../../../Models/Customers';
import { CustomersService } from '../../../Services/Customer/customers.service';
import { Router,RouterModule,RouterOutlet,RouterLink } from '@angular/router';
import { CommonModule,NgIf,NgFor } from '@angular/common';
import { FormsModule,NgModel } from '@angular/forms';
import { routes } from '../../../app.routes';
@Component({
  selector: 'app-get-by-id',
  standalone:true,
  imports: [FormsModule,CommonModule,RouterOutlet],
  templateUrl: './get-by-id.component.html',
  styleUrl: './get-by-id.component.css'
})
export class GetByIdComponent {
  id: number = 0;
  customer: Customers | null = null;
  errorMessage: string = ''; // To hold error messages if needed

  constructor(private getByIdService: CustomersService, private router: Router) {}

  getCustomerById(id: number): void {
    this.customer = null;
    this.errorMessage = '';
    this.getByIdService.getCustomerById(id).subscribe({
      next: data => {
        console.log('Data:', data);
        this.customer = data; // store data if needed
        this.errorMessage = ''; // Clear any previous error messages
      },
      error: err => {
        console.error('Error:', err);
        this.errorMessage = 'Error fetching customer data. Please try again later.'; // User feedback on error
      },
      complete: () => {
        console.log('Request completed');
      }
    });
    this.id=0;
  }
}
