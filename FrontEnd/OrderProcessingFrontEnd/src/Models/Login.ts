/*export interface Login{
    username:string;
    password:string;
}*/

//🔸 Request model (what you send to backend)
export interface LoginRequest {
    username: string;
    password: string;
  }

  //Response , what you get from backedn 
  export interface LoginResponse {
  token: string;
  hasCustomerProfile: boolean;
}
  
