import { Component, OnInit } from '@angular/core';
import { PlaceOrder } from '../../../../Models/PlaceOrder';
import { PlaceOrderService } from '../../../Services/PlaceOrder/place-order.service';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { Router } from '@angular/router';
import { ProductsWithCategory } from '../../../../Models/Products';
import { PaymentMethod } from '../../../../Models/Transactions';
import { TransactionService } from '../../../Services/Transaction/transaction.service';
import { ProductService } from '../../../Services/Product/product.service';
import { TokenService } from '../../../Services/TokenService/token-service.service';
import { CategoryService } from '../../../Services/Category/category.service';
import { Category } from '../../../../Models/Category';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { HeaderComponent } from '../../header/header.component';
import { HeaderService } from '../../../Services/Header/header.service';
import { LogoutComponent } from '../../logout/logout.component';

@Component({
  selector: 'app-place-order',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, LogoutComponent],
  templateUrl: './place-order.component.html',
  styleUrls: ['./place-order.component.css']
})
export class PlaceOrderComponent implements OnInit {

  categories: Category[] = [];
  productByCategory: ProductsWithCategory[] = [];
  selectedProductStock: number[] = [];
  paymentMethodOptions: string[] = [];
  placeOrderForm!: FormGroup;

  constructor(
    private payment: TransactionService,
    private productService: ProductService,
    private categoryService: CategoryService,
    private fb: FormBuilder,
    private placeOrderService: PlaceOrderService,
    private tokenService: TokenService,
    private router: Router,
    private loggingAndErrorService:HeaderService

  ) {}

  ngOnInit() {
    this.getAllCategories();
    this.getPaymentMethod();

    this.placeOrderForm = this.fb.group({
      selectedCategory: ['', Validators.required],
      items: this.fb.array([this.createPlaceOrderItem()]),  //created below
      paymentMethod: ['', Validators.required],
      orderDate: [new Date().toISOString().split('T')[0]]
    });

    this.placeOrderForm.get('selectedCategory')?.valueChanges.subscribe((categoryID) => {
      if (categoryID) {
        this.onCategoryChange(categoryID);
      } else {
        this.productByCategory = [];
      }
    });
  }

  get items(): FormArray {
    return this.placeOrderForm.get('items') as FormArray;
  }

  createPlaceOrderItem(): FormGroup {
    return this.fb.group({
      productName: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]],
      //priceAtPurchase: [0, [Validators.required, Validators.min(0)]]   
      //don't send price frm frontEnd just display becasue it is already placed ith each product on backend
      displayPrice: [{ value: 0, disabled: true }] //  UI only, not sent to backend

    });
  }

  addItem(): void {
    this.items.push(this.createPlaceOrderItem());
  }

  getAllCategories() {
    this.categoryService.fetchAllCategories().subscribe((categories) => {
      this.categories = categories;
    });
  }

  getPaymentMethod() {
    this.payment.getPaymentMethod().subscribe((methods) => {
      this.paymentMethodOptions = methods;
    });
  }

  onCategoryChange(categoryID: number): void {
    this.productService.getProductByCategoryID(categoryID).subscribe((products) => {
      this.productByCategory = products;
      this.items.clear();
      this.addItem();
      this.selectedProductStock = [];
    });
  }

  onProductChange(index: number, selectedProductName: string) {
    const selectedProduct = this.productByCategory.find(p => p.productName === selectedProductName);
    if (selectedProduct) {
      this.items.at(index).get('displayPrice')?.setValue(selectedProduct.price);
      this.selectedProductStock[index] = selectedProduct.stock;
    }
  }

  increaseQuantity(index: number): void {
    const control = this.items.at(index).get('quantity');
    const current = control?.value || 0;
    control?.setValue(current + 1);
  }

  decreaseQuantity(index: number): void {
    const control = this.items.at(index).get('quantity');
    const current = control?.value || 1;
    if (current > 1) {
      control?.setValue(current - 1);
    }
  }

  onSubmit() {
  if (this.placeOrderForm.invalid) {
    alert('Please fill the form correctly before submitting.');
    return;
  }

  const token = this.tokenService.getToken();
  if (!token) {
    alert('You are not logged in. Please login again.');
    this.router.navigate(['/login']);
    return;
  }

   // Show confirmation popup
  if (!confirm('Do you want to place this order?')) {
    return;
  }
  // Build the PlaceOrder object from form
  const formValue = this.placeOrderForm.value;

  const placeOrderData: PlaceOrder = {
    paymentMethod: formValue.paymentMethod,
    orderDate: new Date(), // current date, server uses this anyway
    items: formValue.items.map((item: any) => ({
      productName: item.productName,
      quantity: item.quantity,
      //priceAtPurchase: item.priceAtPurchase // optional, backend recalculates
    }))
  };

   for (let i = 0; i < this.items.length; i++) {
  const productName = this.items.at(i).get('productName')?.value;
  const quantity = this.items.at(i).get('quantity')?.value;
  const stock = this.selectedProductStock[i];

  if (!productName) {
    alert(`Product name is required for item ${i + 1}`);
    return;
  }

  if (quantity > stock) {
    alert(`Quantity exceeds available stock for item ${i + 1}`);
    return;
  }
}

  this.placeOrderService.placeOrder(placeOrderData).subscribe({
    next: (response) => {
      alert(response.message); // or use a toast/snackbar
      //this.router.navigate(['/orders']); // navigate to orders page
    },
    error: (error) => {
      console.error('Error placing order:', error);
      alert(error?.error?.message || 'An error occurred while placing the order.');
    }
  });

}

onLogoutSuccess() {  //based on emit parametr in child compnnet , here paramters are provided 
    this.loggingAndErrorService.log(` you have logout successfully `); //instead of using cosnole.log 
    // Perform additional actions after logout, like updating UI or navigating
  }


}
