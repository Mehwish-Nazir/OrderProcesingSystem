import { Component } from '@angular/core';
import { RouterOutlet,RouterModule, Router } from '@angular/router';
import { FormsModule} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { TokenService } from './Services/TokenService/token-service.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { HeaderComponent } from './Component/header/header.component';
@Component({
  selector: 'app-root',
  standalone:true,
  imports: [RouterOutlet,FormsModule,HttpClientModule,RouterModule,  MatSnackBarModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'CommerceCore';

  constructor(private tokenService: TokenService, private router: Router) {}

  ngOnInit() {
    const isExpired = this.tokenService.isTokenExpired();
    if (isExpired) {
      alert('Session expired. Please log in again.');
      this.router.navigate(['/login']);
    }
}
}
