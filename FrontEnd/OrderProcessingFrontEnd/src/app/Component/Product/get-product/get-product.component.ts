import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../../Services/Product/product.service';
import { ProductsWithCategory } from '../../../../Models/Products';
import { MatTableModule } from '@angular/material/table';
import { routes } from '../../../app.routes';
//import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-get-product',
  standalone:true,
  imports: [MatTableModule,CommonModule],
  templateUrl: './get-product.component.html',
  styleUrls: ['./get-product.component.css']  // Fixed typo: styleUrl → styleUrls
})
//Product With Category
export class GetProductComponent implements OnInit{

  products:ProductsWithCategory[]=[];
constructor(private productService: ProductService){}

ngOnInit(): void {
    this.fetchProductsWithCategories();
  }
fetchProductsWithCategories() {
  this.productService.getProductWithCategory().subscribe({
    next: (response) => {
      this.products = response; //  `products` property 
      alert("Products have been fetched successfully");
      console.log("Fetched products:", this.products);
    },
    error: (error) => {
      console.error("Error fetching products:", error);
      alert("Failed to fetch products.");
    },
    complete: () => {
      console.log("Product fetch request completed.");
    }
  });
}
}
