import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})


///use logging for infor or debuging not direct cosole.log 
//logging is suitable for both production adn development 
export class HeaderService {

  constructor() { }

  log(message:string):void{
console.log(message);
  }

  error(message:string):void{
    console.log(message);
  }
}
