import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { PaymentMethod,Transactions } from '../../../Models/Transactions';
import { Observable,tap,catchError,of } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  private paymentMethodURL=environment.paymentMethodURL;
  constructor(private http :HttpClient) { }

  //inject this service into order place component
  getPaymentMethod():Observable<PaymentMethod[]>{
    return this.http.get<PaymentMethod[]>(`${this.paymentMethodURL}`).pipe(
      tap((response:PaymentMethod[])=>{
        console.log("Payment method have fetched",response);
      }),
      catchError((error)=>{
       console.log("Error fetching payment method");
       return of([])
      })
    );
  }
}
