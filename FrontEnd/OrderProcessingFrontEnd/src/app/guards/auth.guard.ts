import { CanActivateFn } from '@angular/router';
import { TokenService } from '../Services/TokenService/token-service.service';
import { inject } from '@angular/core';
import { Router } from '@angular/router';

//Guard detects user is not logged in	Save current route (state.url) into localStorage.setItem('redirectUrl', state.url);	Trying to access /add-order
//in auth.component.ts file 
//After login success	Check if there is a redirectUrl saved. Navigate there.	Read redirectUrl and navigate(['/add-order'])
//Cleanup	Remove the redirectUrl from localStorage after navigation	localStorage.removeItem('redirectUrl')

//place this [authGuard] anotation in routes file , where the authenticaton is required
export const authGuard: CanActivateFn = (route, state) => {
  const tokenService = inject(TokenService);
  const router = inject(Router);
  const allowedRoles = route.data?.['allowedRoles'] as string[];

  const isExpired = tokenService.isTokenExpired();
  const role = tokenService.getUserRole();

   //console.log(role);
  if (!role || isExpired) {
    alert('Session expired or invalid access. Please log in again.');
      // Save the URL that the user was trying to access
    // This allows us to redirect them back after successful login
    localStorage.setItem('redirectUrl', state.url); //state.url = current route path like /add-order, /customer-list, etc.

    //'redirect url is used to store current url'
// You can store any small text data in localStorage.
//User Preferences	Theme, Language, Settings	"theme": "dark"
//"language": "en"
    router.navigate(['/login']);
    return false;
  }
      
 if (allowedRoles?.includes(role)) {
    return true;
  }

  /*if (role === 'Admin') {
    return true;
  }*/
 // If user is logged in but not an Admin
 //alert('Access denied! Please login as Admin.');
  
 // Save the attempted URL for redirecting after login (optional here, depending on needs)
 localStorage.setItem('redirectUrl', state.url);

  return false;
};
