// token.service.ts
import { Injectable } from '@angular/core';
import { DecodedToken } from '../../../Models/DecodedToken';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  private tokenKey = 'jwtToken'; // Key used for storing the token in localStorage

  // Store the JWT token in localStorage
  storeToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  // Retrieve the JWT token from localStorage
  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  // Remove the JWT token from localStorage
  removeToken(): void {
    localStorage.removeItem(this.tokenKey);
  }

  // Decode the JWT token
  private decodeToken(): DecodedToken | null {
    const token = this.getToken();
    if (!token) return null;
    try {
      const payload = token.split('.')[1]; //
      const decoded = atob(payload); // Decode Base64
      console.log('Decoded Token:', decoded); // Log the decoded token for debugging
      return JSON.parse(decoded);
    } catch (error) {
      console.error('Token decoding failed:', error);
      return null;
    }
  }

  // Check if the token is expired
  isTokenExpired(): boolean {
    const decoded = this.decodeToken();
    if (!decoded?.exp) return true;
    const currentTime = Math.floor(Date.now() / 1000); // Current time in seconds
    return decoded.exp < currentTime;
  }

  // Get the decoded token (for other checks like role or user data)
  getDecodedToken(): DecodedToken | null {
     const token = this.getToken();
  if (!token) return null;
  return JSON.parse(atob(token.split('.')[1]));
  }
  getUserRole(): string | null {
    const decodedToken = this.decodeToken();
    if (decodedToken) {
      // Check for the custom role claim in the decoded token
      const role = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
      console.log('User Role:', role);  // Log the role to check if it's extracted correctly
      return role || null;  // Return the role if found, otherwise return null
    }
    return null;  // Return null if token is invalid or decoding fails
  }

getCustomerName(): string | null {
  const decodedToken = this.decodeToken();
  
  if (decodedToken) {
    return decodedToken["customerName"] || null;
  }
  return null;
}
}
