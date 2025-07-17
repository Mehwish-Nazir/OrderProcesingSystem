import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TokenService } from '../TokenService/token-service.service';
import { ProductSearchRequest,PagedProductResponse,ProductResponse } from '../../../Models/SearchProduct';
import { environment } from '../../../environments/environment';
import { HttpHeaders} from '@angular/common/http';
import { throwError, Observable, tap,catchError } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class SearchProductService {

  private searchProductURL=environment.searchProductURL;
  constructor(private http:HttpClient,
    private tokenService:TokenService
  ) { }

  searchProduct(searchProduct:ProductSearchRequest):Observable<PagedProductResponse>{
const token = this.tokenService.getDecodedToken();
    if (!token) {
      return throwError(() => new Error('Token is missing. Please log in first.'));
    }
  
    const headers = new HttpHeaders().set('Authorization', `Bearer ${this.tokenService.getToken()}`);
    console.log('Authorization Header:', headers);
    return this.http.post<PagedProductResponse>(`${this.searchProductURL}`, searchProduct, {headers}).pipe(
       tap(response => {
        console.log(' Product Search Response:', response);
      }),
      catchError(error => {
        console.error(' Product Search Error:', error);

        let errorMessage = 'Something went wrong. Please try again later.';

        if (error.status === 0) {
          errorMessage = 'Unable to connect to the server.';
        } else if (error.status === 400) {
          errorMessage = error.error?.error || 'Bad request. Please check your input.';
        } else if (error.status === 401) {
          errorMessage = 'Unauthorized. Please log in again.';
        } else if (error.status === 500) {
          errorMessage = 'Server error. Please try again later.';
        }

        return throwError(() => new Error(errorMessage));
      })
    );

  }
  

}
