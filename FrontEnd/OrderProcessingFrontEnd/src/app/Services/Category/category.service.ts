import { HttpClient , HttpHeaders} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Category } from '../../../Models/Category';
import { TokenService } from '../TokenService/token-service.service';
import { environment } from '../../../environments/environment';
import { Observable, throwError } from 'rxjs';
import { tap, catchError, of } from 'rxjs';
import { AddCategoryComponent } from '../../Component/Category/add-category/add-category.component';
@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  private addCategoryURL=environment.addCategoryURL;
  private  getAllCategroiesURL=environment.getAllCategroiesURL
  constructor(private http:HttpClient,
    private tokenService:TokenService
  ) { }

  fetchAllCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(this.getAllCategroiesURL).pipe(
      tap((response: Category[]) => {
        console.log("Categories fetched successfully");
      }),
      catchError(error => {
        console.error("Error fetching categories:", error);
        return of([] as Category[]);  // returns empty array on error to match type
      })
    );
  }

  addCategory(category: Category): Observable<Category> {
    const token = this.tokenService.getDecodedToken();
    if (!token) {
      return throwError(() => new Error('Token is missing. Please log in first.'));
    }
  
    const headers = new HttpHeaders().set('Authorization', `Bearer ${this.tokenService.getToken()}`);
    console.log('Authorization Header:', headers);
  
    return this.http.post<Category>(this.addCategoryURL, category, { headers }).pipe(
      tap((response:Category) => 
        console.log(`Category with ID ${response.categoryID} has been successfully added at time ${response.createdAt}.`)
      ),
      catchError((error:Category) => {
        console.error('Error in addCategory service:', error);
        return throwError(() => error);
      })
    );
  }

}

