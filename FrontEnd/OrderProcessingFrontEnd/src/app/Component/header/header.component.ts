import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../Services/auth/auth.service';
import { UserProfileDTO } from '../../../Models/Users';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  userProfile!: UserProfileDTO;
  isLoading: boolean = true;
  showMenu: boolean = false; // required for dropdown toggle

  constructor(private userService: AuthService, private router: Router) {}

  ngOnInit() {
    this.userService.getUserProfile().subscribe({
      next: (profile) => {
        this.userProfile = profile;
        this.isLoading = false;
      },
      error: (err) => {
        if (err.status === 401) {
          this.isLoading = false;
          return;
        }
        console.error('Failed to load user profile:', err);
        this.isLoading = false;
      }
    });
  }

  toggleMenu() {
    this.showMenu = !this.showMenu;
  }

  logout() {
    localStorage.clear(); // or this.userService.logout();
    this.router.navigate(['/login']);
  }
}
