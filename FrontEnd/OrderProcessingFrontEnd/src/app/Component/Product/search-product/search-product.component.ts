import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ProductSearchRequest, ProductResponse, PagedProductResponse } from '../../../../Models/SearchProduct';
import { SearchProductService } from '../../../Services/SearchProduct/search-product.service';

@Component({
  selector: 'app-search-product',
  standalone: true,
  templateUrl: './search-product.component.html',
  styleUrls: ['./search-product.component.css'],
  imports: [CommonModule, ReactiveFormsModule, MatSnackBarModule]  // FormsModule not needed anymore
})
export class SearchProductComponent implements OnInit {
  searchForm!: FormGroup;
  products: ProductResponse[] = [];
  totalPages = 0;
  totalCount = 0;
  pageNumber = 1;
  pageSize = 10;
  pages: number[] = [];
  loading = false;
  pageSizeOptions = [5, 10, 20, 50];

  constructor(
    private productService: SearchProductService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.searchForm = new FormGroup({
      searchText: new FormControl('', Validators.required)
    });
  }

  onSearch(): void {
    if (this.searchForm.invalid) {
      this.showSnackBar('Please enter a product or category name.');
      return;
    }

    this.pageNumber = 1;
    this.fetchProducts();
  }

  fetchProducts(): void {
    const request: ProductSearchRequest = {
      searchText: this.searchForm.value.searchText.trim(),
      pageNumber: this.pageNumber,
      pageSize: this.pageSize
    };

    this.loading = true;
    this.productService.searchProduct(request).subscribe({
      next: (response: PagedProductResponse) => {
        this.products = response.products;
        this.totalPages = response.totalPages;
        this.totalCount = response.totalCount;
        this.pageNumber = response.pageNumber;
        this.pageSize = response.pageSize;
        this.pages = Array.from({ length: this.totalPages }, (_, i) => i + 1);
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        this.products = [];
        this.showSnackBar(err.message || 'Something went wrong.');
      }
    });
  }

  goToPage(page: number): void {
    if (page !== this.pageNumber) {
      this.pageNumber = page;
      this.fetchProducts();
    }
  }

  nextPage(): void {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.fetchProducts();
    }
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.fetchProducts();
    }
  }

  // ✅ Type-safe change handler
  onPageSizeChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const newSize = parseInt(selectElement.value, 10);
    this.pageSize = newSize;
    this.pageNumber = 1;
    this.fetchProducts();
  }

  private showSnackBar(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      verticalPosition: 'top',
      horizontalPosition: 'center',
      panelClass: 'custom-snackbar'
    });
  }
}
