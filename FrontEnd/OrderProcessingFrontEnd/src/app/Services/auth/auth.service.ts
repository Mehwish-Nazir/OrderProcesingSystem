import { Injectable } from '@angular/core';
import { LoginRequest,LoginResponse } from '../../../Models/Login';
import { Register } from '../../../Models/Register';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable,tap } from 'rxjs';
import { Router,RouterOutlet,RouterModule } from '@angular/router';
import { TokenService } from '../TokenService/token-service.service';
import { UserProfileDTO } from '../../../Models/Users';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private loginURL=environment.loginURL;  //import url 
  private registerURL=environment.registerURL;
  private getRoleURL=environment.getRoleURL;
  private logoutURL=environment.logoutURL;
  private getProfileNameURL=environment.getPrfileNameURL;
  constructor(private http:HttpClient,
    private router: Router,
    private tokenService:TokenService,
  ) { } // inject HttpClient
  
  /**
   * Login method to authenticate the user and get JWT token
   * @param loginData The login credentials (username, password)
   * @returns Observable with the server response (JWT token)
   */
  /*login(login:Login):Observable<any>{
    return this.http.post(`${this.loginURL}`,login);
  }*/
     // Login method to authenticate the user and store JWT token
login(loginData: LoginRequest): Observable<LoginResponse> {
  return this.http.post<LoginResponse>(this.loginURL, loginData).pipe(
    tap((response) => {
      this.tokenService.storeToken(response.token);
    })
  );
}


    
  register(register:Register):Observable<any>{
    return this.http.post(`${this.registerURL}`, register);
  }

  getRole():Observable<any>{
    return this.http.get(`${this.getRoleURL}`)
  }


  getUserProfile(): Observable<UserProfileDTO> {
    const token = localStorage.getItem('jwtToken');

  if (!token) {
    throw new Error('JWT token not found in localStorage.');
  }

  const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<UserProfileDTO>(this.getProfileNameURL, {headers});
  }
   
  
  logout():Observable<any>{

    this.tokenService.removeToken();
   return this.http.post(`${this.logoutURL}`,{}).pipe(
    tap(() => {
      // Any additional logic after logout, like redirecting the user
      console.log("Logged out successfully");
    })
   );

  }
  
  }
  

