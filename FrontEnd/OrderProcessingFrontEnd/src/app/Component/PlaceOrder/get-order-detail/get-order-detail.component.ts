import { Component, OnInit } from '@angular/core';
import { PlaceOrderService } from '../../../Services/PlaceOrder/place-order.service';
import { TokenService } from '../../../Services/TokenService/token-service.service';
import { FormBuilder, FormControl, ReactiveFormsModule, FormGroup, Validators ,} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { GetOrderDetail } from '../../../../Models/PlaceOrder';
@Component({
  selector: 'app-get-order-detail',
  standalone:true,
  imports: [ReactiveFormsModule,CommonModule],
  templateUrl: './get-order-detail.component.html',
  styleUrl: './get-order-detail.component.css'
})
export class GetOrderDetailComponent implements OnInit{
    orderDetailForm!:FormGroup;
    customerName:string|null=null;
    orderDetails:GetOrderDetail|null=null;
    errorMsg:string | null=null;

    //OR
    //orderDetailForm:FormGroup=new FormGroup({});

  constructor(private orderDetailService:PlaceOrderService,
    private tokenService:TokenService,
    private fb:FormBuilder
  ){
     this.orderDetailForm=this.fb.group({
    orderID:[0,[Validators.required]]
  })
  }

ngOnInit(): void {

  const decodedToken = this.tokenService.getDecodedToken();
  
  this.customerName = this.tokenService.getCustomerName();
  console.log('CustomerName while loading component:', this.customerName);
 
}

onSubmit():void{
  if(this.orderDetailForm.invalid){
 console.warn('Invalid GetOrder Page');
  }
  else{
    // get order Id value from .valu of orderDetailfORM
    const orderID=this.orderDetailForm.value.orderID;

    this.orderDetailService.getOrderDetails(orderID).subscribe({
       next: (response) => {
      this.orderDetails = response;
      this.errorMsg = null; // Clear any previous error
    },
    error: (error) => {
      if (error.status === 404) {
        this.errorMsg = 'Order ID does not exist.';
      } else {
        this.errorMsg = 'An error occurred while fetching the order.';
      }
      this.orderDetails = null; // Clear previous data
    }
    }
 

    )
  }
} 
}
