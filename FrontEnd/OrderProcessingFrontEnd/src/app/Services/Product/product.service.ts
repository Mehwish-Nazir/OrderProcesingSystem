import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { CreateNewProduct, ProductsWithCategory } from '../../../Models/Products';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { TokenService } from '../TokenService/token-service.service';
import { catchError, tap, of, throwError} from 'rxjs';
import { Observable } from 'rxjs';
import { Category } from '../../../Models/Category';
@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private getAllProductsWithCategoriesURL=environment.getAllProductsWithCategoriesURL;
  private addProductURL=environment.addProductURL;
  private getProductbyCategoryIdURL=environment.getProductbyCategoryIdURL;
  constructor(private http:HttpClient, 
    private tokenService:TokenService
  ) {}



  //ProductWithCategory Service
  getProductWithCategory():Observable<ProductsWithCategory[]>{
      return this.http.get<ProductsWithCategory []>(`${this.getAllProductsWithCategoriesURL}`).pipe(
        tap((response: ProductsWithCategory[]) => {
          console.log('Products with category fetched:', response);
        }),
        catchError(error => {
          console.error('Error fetching products with category:', error);
          return of([]); // return fallback value
        })
      );
  }


//AddProduct Service 
  addProduct(product: CreateNewProduct):Observable<CreateNewProduct>{
    const token= this.tokenService.getDecodedToken();
    if(!token){
      return throwError(() => new Error('Token is missing. Please log in first.'));
    }

    var headers=new HttpHeaders().set('Authorization', `Bearer ${this.tokenService.getToken()}`);
    console.log("Authorization Header", headers);
    return this.http.post<CreateNewProduct>(`${this.addProductURL}`, product , {headers}).pipe(
    tap((response: CreateNewProduct) => {
      console.log(`New Product Added Successfully on id`, response);
    }),
    catchError((error: HttpErrorResponse) => {
        const errorMsg = error.error?.message || 'Unexpected error while adding product.';

      console.error("Adding Product failed", error);
      return throwError(() => new Error('Failed to add product.', errorMsg));
    })
  );
  }

  //ProductByCategoryID Service

  getProductByCategoryID(categoryId:number):Observable<ProductsWithCategory[]>{
   
    return this.http.get<ProductsWithCategory[]>(`${environment.getProductbyCategoryIdURL}/${categoryId}`).pipe(
      tap((response:ProductsWithCategory[])=>{
        console.log(`${categoryId} has the followingProducts `, response);
      }),
      catchError((error:HttpErrorResponse)=>{

      const errorMsg = error?.error?.message || "Unexpected error occurred.";
        return throwError(() => new Error('Failed to add product.', errorMsg));

      })
    );
  
}
}
