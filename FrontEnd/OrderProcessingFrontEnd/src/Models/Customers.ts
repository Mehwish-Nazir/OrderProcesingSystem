import { Register } from "./Register";
export interface Customers{ 
  customerID?:number,
  firstName:string,
  lastName:string,
  email:string,
  phoneNumber:string,
  userID:number,
  createdAt?:Date;
}

export interface CustomerWithUser {
  customerID: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  createdAt: Date;  // Date object
  userID: number;
  user: Register |null;
  //front -end enitity
 userStatus?:string;  //if customer has user or not 
 usernameDisplay?:string // to display user name if exist just front end enttiyt 
  //Register|null means 
  //The user property must always exist.
 // But its value can either be a valid User object or null.
 //I use this appraoch to fetch customer with valid users and customers with not users
  order: any[];
}