import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormsModule, FormBuilder, Validators, NgForm, ReactiveFormsModule} from '@angular/forms';
import { ProductService } from '../../../Services/Product/product.service';
import { CreateNewProduct, ProductsWithCategory } from '../../../../Models/Products';
import { CategoryService } from '../../../Services/Category/category.service';
import { Category } from '../../../../Models/Category';
import { Route, Navigation , RouterOutlet, Router, RouterModule} from '@angular/router';
import { LogoutComponent } from '../../logout/logout.component';
@Component({
  selector: 'app-add-product',
  standalone:true,
  imports: [CommonModule,ReactiveFormsModule, RouterModule],
  templateUrl: './add-product.component.html',
  styleUrl: './add-product.component.css'
})
export class AddProductComponent implements OnInit{

  productForm!:FormGroup;
  productList:ProductsWithCategory[]=[];
  categories:Category[]=[];
  constructor(private productService:ProductService,
              private fb:FormBuilder,
              private categoryService:CategoryService,
              private router:Router
  ){
//check in constructor whether is valid use is logged in or not 
    const token = localStorage.getItem('token');
  if (!token) {
    this.router.navigate(['/login']);
  }
  };

  ngOnInit(): void {
this.selectCategory();
//create product fields 
this.productForm = this.fb.group({
  productName: this.fb.control('', [Validators.required, Validators.maxLength(100)]),
  price: this.fb.control(0, [Validators.required]),
  stock: this.fb.control(0, [Validators.required, Validators.pattern(`^[0-9]+$`)]),
  categoryID: this.fb.control('', [Validators.required])
  //I use categoryID: this.fb.control('', [Validators.required])
  //<option value="" disabled>Select Category</option>  as categoryID is string based selection

//categoryID: [0, Validators.required]  //this will cause errro 

});
  }

  onSubmit():void{
    console.log("Adding Product form trigerred.");
    if(this.productForm.invalid)
    {
      console.warn("Form is invalid");
    }


  
    //fetch all product to check that product already exist
      const formData: CreateNewProduct = this.productForm.value;  //const must be use inside some method but not use in class directly like we use inside onSubmit method

     this.productService.getProductWithCategory().subscribe((productList: ProductsWithCategory[]) => {
  this.productList = productList;
  console.log("products have fetched", this.productList);
   //how to get propety in reactive form
   const enteredNameRaw = this.productForm.get('productName')?.value;
    const enteredName = enteredNameRaw ? enteredNameRaw.trim().toLowerCase() : '';
        const enteredCategoryID = this.productForm.get('categoryID')?.value;

      //var enteredName=this.productForm.get('productName')?.value.toLowerCase()
      const exists= this.productList.some(p=>p.productName&&
        p.productName.trim().toLowerCase()==enteredName&&
              p.category.categoryID === enteredCategoryID

      );
      if(exists){
        console.warn(`Product ${enteredName} already exist`);
          if (exists) {
          alert(`Product "${enteredName}" already exists in selected category.`);
          return;
        }

        return;
      }

      this.productService.addProduct(formData).subscribe(
        (response)=>{
          alert("Product added successfully");
          console.log("Product added sucessfuly on ", response);
          this.productForm.reset();
        },
       (error)=>{
          console.error("Error adding product:", error);
           alert("Failed to add product");
       }
      );
     }
    );
  }


   selectCategory():void{
     //fetch Allcategory to fetch categoryID 
    this.categoryService.fetchAllCategories().subscribe((categoryList:Category[])=>
    {
      this.categories=categoryList;
        console.log("Categories fetched successfully", this.categories);
    });
   }
 
}
