export interface DecodedToken {
  role: string;
  exp: number; // expiration time (in seconds)
  [key: string]: any; // for any other claims
}
