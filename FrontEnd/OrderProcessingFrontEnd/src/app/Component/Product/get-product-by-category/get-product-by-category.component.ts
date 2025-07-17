import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../../Services/Product/product.service';
import { FormControl, FormGroup, FormsModule, FormBuilder, Validators, NgForm, ReactiveFormsModule} from '@angular/forms';
import { CreateNewProduct, ProductsWithCategory } from '../../../../Models/Products';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-get-product-by-category',
  imports: [FormsModule,ReactiveFormsModule,CommonModule],
  templateUrl: './get-product-by-category.component.html',
  styleUrl: './get-product-by-category.component.css'
})
export class GetProductByCategoryComponent implements OnInit{

  constructor(private productService:ProductService,
    private fb:FormBuilder,

  ){}
  productList:ProductsWithCategory[]=[];
  categoryIdForm!:FormGroup;


  ngOnInit(){
     this.categoryIdForm=this.fb.group({
       categoryID:this.fb.control(0,[Validators.required])
     }
     );
  }
  onSubmit():void{
    console.log("Product By CategoryID has trigerred");
    if(this.categoryIdForm.invalid){
      console.warn("Form is invalid");
    }
    
    
    const categoryID = this.categoryIdForm.value.categoryID ; //const must be use inside some method but not use in class directly like we use inside onSubmit method

    this.productService.getProductByCategoryID(categoryID).subscribe({
      next:(products)=>{
        this.productList=products;
        console.log('Products fetched:', products);
      },
      error:(err)=>{
          alert("Failed to fetch product");
      }
    });
  }
}
