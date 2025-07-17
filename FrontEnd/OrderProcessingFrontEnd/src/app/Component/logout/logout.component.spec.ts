import { Component, Output, EventEmitter, Input } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../Services/auth/auth.service';
import { HeaderService } from '../../Services/Header/header.service';

@Component({
  selector: 'app-logout',
  standalone: true,  //  VERY IMPORTANT
  imports: [CommonModule], //  If you're using *ngIf or other common directives
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent {

  @Input() customerName!: string;  // Accepts dynamic name from parent
  @Output() logOut = new EventEmitter<string>();  // Emits logout event to parent

  constructor(
    private logoutService: AuthService,
    private router: Router,
    private loggingAndErorService: HeaderService
  ) {}

  onLogout() {
    this.logoutService.logout().subscribe(
      () => {
        alert(`${this.customerName}, you have been logged out successfully.`);
        this.router.navigate(['/login']);
        this.logOut.emit();  // Notify parent component
      },
      (error) => {
        alert('Error logging out. Please try again.');
        this.loggingAndErorService.error(error);
      }
    );
  }
}
