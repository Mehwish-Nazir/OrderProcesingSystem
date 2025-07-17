import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { HttpClientModule, HttpHeaders } from '@angular/common/http';
import { LoginRequest } from '../../../Models/Login';
import { AuthService } from '../../Services/auth/auth.service';
import { TokenService } from '../../Services/TokenService/token-service.service';
import { Customers } from '../../../Models/Customers';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    HttpClientModule
  ],
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent {
  loginData: LoginRequest = {
    username: '',
    password: ''
  };

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

onLogin() {
  this.authService.login(this.loginData).subscribe(
    (response: any) => {
      const jwtToken = response.token.token; // ✅ extract actual token
      const hasProfile = response.token.hasCustomerProfile; // ✅ extract flag

      localStorage.setItem('jwtToken', jwtToken);
      alert(`${response.message}`); // Optional

      // decode token
      const payload = JSON.parse(atob(jwtToken.split('.')[1]));
      const role = payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

      const redirectUrl = localStorage.getItem('redirectUrl');

      if (redirectUrl) {
        this.router.navigate([redirectUrl]);
        localStorage.removeItem('redirectUrl');
      } else {
        if (role === 'Admin') {
          this.router.navigate(['/admin/admin-dashboard']);
        } else if (role === 'Customer') {
          // ✅ route based on profile presence
          if (hasProfile) {
            this.router.navigate(['/customer/customer-dashboard']);
          } else {
            this.router.navigate(['/add-customer']); // ✅ Top-level now
          }
        } else {
          this.router.navigate(['/login']);
        }
      }
    },
    error => {
      alert('Invalid login.');
      console.error(error);
    }
  );
}
  goToRegister() {
    this.router.navigate(['/register']);
  }
}
