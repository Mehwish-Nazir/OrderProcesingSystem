import { Component } from '@angular/core';
import { Category } from '../../../../Models/Category';
import { CategoryService } from '../../../Services/Category/category.service';
import { Route, Router,RouterModule,RouterOutlet } from '@angular/router';
import { NgForm, NgModel } from '@angular/forms';
import { HeaderService } from '../../../Services/Header/header.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-add-category',
  standalone:true,
  imports: [ FormsModule, CommonModule],
  templateUrl: './add-category.component.html',
  styleUrl: './add-category.component.css'
})
export class AddCategoryComponent {

  constructor(private categoryService:CategoryService,
    router:Router,

  ){};
  category:Category={
    categoryName:''
  }
  
 
  onSubmit(form: NgForm) {
    console.log("onSubmit triggered");
    if (form.invalid) {
      console.warn('Form is invalid');
      return;
    }
  
    console.log("Fetching all categories...");
    this.categoryService.fetchAllCategories().subscribe((category: Category[]) => {
      console.log("Fetched categories:", category);
      const exists = category.some(cat => cat.categoryName.toLowerCase() === this.category.categoryName.toLowerCase());
  
      if (exists) {
        console.log("Category already exists!");
        alert('Category already exists!');
      } else {
        console.log("Category is unique. Proceed to add.");
        // Proceed to add the category
        this.categoryService.addCategory(this.category).subscribe({
          next: (response) => {
            console.log(`Category with id ${response.categoryID} has been successfully added`, response);
            alert('Category added successfully!');
            form.resetForm(); // Optional
          },
          error: (error) => {
            console.error('Failed to add category:', error);
            alert('Failed to add category.');
          }
        });
      }
    });
  }
  
}
