import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap, catchError,throwError } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Customers } from '../../../Models/Customers';
import { CustomerWithUser } from '../../../Models/Customers';
import { routes } from '../../app.routes';
import { map } from 'rxjs';
import { TokenService } from '../TokenService/token-service.service';
@Injectable({
  providedIn: 'root'
})
export class CustomersService {

  private getCustomerURL=environment.getCustomerURL;
  private getCustomerByIdURL=environment.getCustomerByIdURL;
  private getCustomerWithUserURL=environment.getCustomerWithUserURL;
  private addCustomerURL=environment.addCustomerURL;
  constructor (private http:HttpClient,
    private tokenService: TokenService  // Inject the TokenDecoderService

  ){};
  
  getCustomer():Observable<Customers[]>{
      return this.http.get<Customers[]>(`${this.getCustomerURL}`);
  }

  getCustomerById(id:number):Observable<Customers>{
    if (!id || id <= 0) {
      throw new Error('Invalid ID'); // this stops the request
    }
    return this.http.get<Customers>(`${this.getCustomerByIdURL}/${id}`);
  }


  getCustomerWithUser():Observable<CustomerWithUser[]>{
    return this.http.get<CustomerWithUser[]>(this.getCustomerWithUserURL).pipe(
      map(customers =>
        customers.map(customer => {
          if (customer.user) {
            customer.userStatus = 'Customer with user';
            customer.usernameDisplay = customer.user.username;
          } else {
            customer.userStatus = 'Customer without user';
            customer.usernameDisplay = 'N/A';
          }
          return customer;
        })
      )
    );
  }

  addCustomer(customer: Customers): Observable<Customers> {
    const token = this.tokenService.getDecodedToken();  // Replace this with your token fetching logic if needed

    if (!token) {
      return throwError(() => new Error('Token is missing. Please log in first.'));
    }

const headers = new HttpHeaders().set('Authorization', `Bearer ${this.tokenService.getToken()}`);
//add token in request header
console.log('Authorization Header:', headers);  // Log this to verify the header

    return this.http.post<Customers>(this.addCustomerURL, customer, { headers }).pipe(
      //customer is input object from model so we don't return our repsone from imut object 
      //while response comes from Serve so we use (response.customerID ) to diplay message from server not {customer.CustomerID}
      //tap((response) => console.log(`Customer with ID ${customer.customerID} has been successfully added at ${customer.createdAt}`)),
      tap((response) => console.log(`Customer with ID ${response.customerID} has been successfully added at time ${response.createdAt}`)),
      catchError((error) => {
        console.error('Add customer failed:', error.message,error);
        return throwError(() => new Error('Add customer failed. Please try again later.'));
      })
    );
  }
}
