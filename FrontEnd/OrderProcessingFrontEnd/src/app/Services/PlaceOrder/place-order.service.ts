import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GetOrderDetail, PlaceOrder } from '../../../Models/PlaceOrder';
import { environment } from '../../../environments/environment';
import { TokenService } from '../TokenService/token-service.service';
import { tap,Observable, catchError ,throwError, finalize} from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class PlaceOrderService {

  private placeOrderURL=environment.placeOrderURL;
  private orderDetailsURL=environment.orderDetailsURL
  constructor(private http:HttpClient, 
    private tokenService:TokenService ,
  ) { }


  //backedn returns message as  object like this 
  /*
  {
  "message": "Order placed successfully!"
}
  due to ok({message=result}) so I will pass {message:string} in observable as a return 'object'
  */

 placeOrder(placeOrder:PlaceOrder):Observable<{message:string}>{
 
  const token=this.tokenService.getDecodedToken();
  if(!token){
    return throwError(()=>new Error('Token is missng '));
  }

  const headers=new HttpHeaders().set('Authorization', `Bearer ${this.tokenService.getToken()}`);
  console.log('Header', headers);
  //order date as curent date using spread opertor(...) changes in new object oiginal object or feld remain same 
  const orderWithCurrentDate = { ...placeOrder, orderDate: new Date() };

  return this.http.post<{message:string}>(this.placeOrderURL, orderWithCurrentDate, { headers })  // Use orderWithCurrentDate here
  .pipe(
    tap(response => console.log('Order Placed Successfully', response)),
    catchError(error => {
      console.log('Error placing order', error);
      return throwError(() => error);
    })
  );
    
  }

  getOrderDetails(orderId:number):Observable<GetOrderDetail>{
 const token=this.tokenService.getDecodedToken();
  if(!token){
    return throwError(()=>new Error('Token is missng '));
  }

  const headers=new HttpHeaders().set('Authorization', `Bearer ${this.tokenService.getToken()}`);
  console.log('Header', headers);
  return this.http.get<GetOrderDetail>(`${this.orderDetailsURL}/${orderId}`, { headers }).pipe(
    tap(orderDetail=>console.log('Order details feteched', orderDetail)),
    catchError(error=>{console.log('Error fecthing Order detail',error);
            return throwError(() => new Error('Failed to load order details'));

    }),
    finalize(()=>{
          console.log('Completed fetching order details'); 

    }
)
  );
  }
 
}


