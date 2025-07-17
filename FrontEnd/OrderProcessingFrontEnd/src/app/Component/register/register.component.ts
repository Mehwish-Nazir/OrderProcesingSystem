import { Component } from '@angular/core';
import { Register } from '../../../Models/Register';
import { AuthService } from '../../Services/auth/auth.service';
import { AuthComponent } from '../Auth/auth.component';
import { CommonModule } from '@angular/common';
import { RouterModule,RouterOutlet,Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-register',
  imports: [FormsModule,RouterModule,CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
//we define varibales to for two way binding in html page
//to to get inut of username wwe wil use registerData.username to get input
  registerData:Register={
     username:'',
     password:'',
     email:'',
     role:''
  };
  roles:string[]=[];
  //showRoles = false;  // This controls whether the roles are shown
   message:string='';
  //inject service and route here 
  constructor(private authService: AuthService,
    private router: Router){}
    
    /*onRegister():void{

      if (!this.registerData.username || !this.registerData.email || !this.registerData.password || !this.registerData.role) {
        alert("Please fill in all required fields.");
        return;
      }
      this.authService.register(this.registerData).subscribe(
        (response)=>{
          alert("You have registered successfully!")
          console.log("You have registered successfully!", response)
          this.message="Now Login"
          this.router.navigate(['/login'])
        },  
        (error)=>{
          alert("There is an error in signup registration!")
          console.log("There is error in registration", error)
        }
      );
      
    }*/

      onRegister():void{
      if (!this.registerData.username || !this.registerData.email || !this.registerData.password || !this.registerData.role) {
        alert("Please fill in all required fields.");
        return;
      }
    
      this.authService.register(this.registerData).subscribe(
        (response) => {
          alert(`Congratulations! ${this.registerData.username}, you have successfully registered!`);
        console.log('registered successful:', response);
          this.router.navigate(['/login']);
        },
        (error) => {
          //error.statu will fetch error excetion form backend 
          if (error.status === 400 && typeof error.error === 'string') {
            alert(error.error); // <-- this will show: "Username already exist"
          } else {
            alert('Registration failed! Try again later.');
          }
        }
      );
    }

    //when the compnet will load it will fetch role from backend auomaictally 
    //without fecthing roles 'dropdown' menu will not work in select role field
    ngOnInit(): void {
      this.selectRole();  // Fetch roles when component initializes
    }
    
   selectRole():void{
    this.authService.getRole().subscribe(
      {
        next: (data) => {
          this.roles = data;
          //this.showRoles = true; // Set showRoles to true to display the roles

        },
        error: (err) => {
          console.error('Error fetching roles:', err);
        }
      });//as roles contain many values , use NgFor for this 
   }
}
