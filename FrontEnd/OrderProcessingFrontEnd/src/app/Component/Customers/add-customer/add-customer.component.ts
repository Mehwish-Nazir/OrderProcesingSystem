import { Component } from '@angular/core';
import { CustomersService } from '../../../Services/Customer/customers.service';
import { Customers } from '../../../../Models/Customers';
import { routes } from '../../../app.routes';
import { CommonModule, NgFor, NgIf } from '@angular/common';
import { Route,Router, RouterModule,RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Observable } from 'rxjs';
import { TokenService } from '../../../Services/TokenService/token-service.service';
import { HeaderService } from '../../../Services/Header/header.service';
import { LogoutComponent } from '../../logout/logout.component';
@Component({
  selector: 'app-add-customer',
  standalone:true,
  imports: [FormsModule,RouterModule,LogoutComponent, CommonModule],
  templateUrl: './add-customer.component.html',
  styleUrl: './add-customer.component.css'
})
export class AddCustomerComponent {

  constructor(private customerService:CustomersService,
    private getRolefromToken:TokenService,
    private router:Router,
    private loggingAndErrorService:HeaderService
  ){}

ngOnInit() {
  console.log('AddCustomerComponent initialized');

  const decodedToken = this.getRolefromToken.getDecodedToken();

  if (decodedToken) {
    const userIdFromToken =
      decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];

    if (userIdFromToken) {
      this.customer.userID = +userIdFromToken; // Cast to number
      console.log('UserID auto-set from token:', this.customer.userID);
    } else {
      alert('User ID not found in token.');
      this.router.navigate(['/login']);
    }
  } else {
    alert('Invalid or expired token.');
    this.router.navigate(['/login']);
  }
}
  customer: Customers = {
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    userID:0
  };


  onLogoutSuccess(name:string) {  //based on emit parametr in child compnnet , here paramters are provided 
    this.loggingAndErrorService.log(`${name} you have logout successfully `); //instead of using cosnole.log 
    // Perform additional actions after logout, like updating UI or navigating
  }
  //const phone = this.customer.phoneNumber;
  existingEmails: string[] = ['existing@example.com', 'taken@example.com'];

  addCustomer() {
    if (this.customer.phoneNumber.length > 11) {
      alert('Phone number must not exceed 11 digits.');
      return;
    }

    return this.customerService.addCustomer(this.customer).subscribe(
      (response: any) => {
        console.log('Customer added:', response);

        const customerId = response?.data?.customerID;
        const createdDate = response?.data?.createdAt;
        const message = response?.message;

        if (customerId) {
          alert(`${message} at time ${createdDate}. Your ID is ${customerId}.`);
          this.router.navigate(['/customer/customer-dashboard']);
        } else {
          alert("Customer added, but ID not found in response.");
        }
      },
      (error) => {
        console.error('Full error object:', error);
        const backendMessage = error?.error?.message || error?.message || "An unexpected error occurred.";
        alert(backendMessage);
      }
    );
  }
  }

