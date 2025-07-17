import { RouterModule, RouterOutlet, Routes } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthComponent } from './Component/Auth/auth.component';
import { HttpClientModule } from '@angular/common/http';
import { RegisterComponent } from './Component/register/register.component';
import { GetCustomersComponent } from './Component/Customers/get-customers/get-customers.component';
import { GetByIdComponent } from './Component/Customers/get-by-id/get-by-id.component';
import { CustomerWithUserDetailComponent } from './Component/Customers/customer-with-user-detail/customer-with-user-detail.component';
import { AddCustomerComponent } from './Component/Customers/add-customer/add-customer.component';
import { AppComponent } from './app.component';
import { authGuard } from './guards/auth.guard';
import { LogoutComponent } from './Component/logout/logout.component';
import { HeaderComponent } from './Component/header/header.component';
import { AddCategoryComponent } from './Component/Category/add-category/add-category.component';
import { GetProductComponent } from './Component/Product/get-product/get-product.component';
import { AddProductComponent } from './Component/Product/add-product/add-product.component';
import { GetProductByCategoryComponent } from './Component/Product/get-product-by-category/get-product-by-category.component';
import { PlaceOrderComponent } from './Component/PlaceOrder/place-order/place-order.component';
import { GetOrderDetailComponent } from './Component/PlaceOrder/get-order-detail/get-order-detail.component';
import { SearchProductComponent } from './Component/Product/search-product/search-product.component';

// Admin and Customer Dashboards
import { AdminDashboardComponent } from './Component/Dashboard/admin-dashboard/admin-dashboard.component';
import { CustomerDashboardComponent } from './Component/Dashboard/customer-dashboard/customer-dashboard.component';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },

  { path: 'login', component: AuthComponent },
  { path: 'register', component: RegisterComponent },

  {
    path: 'add-customer',
    component: AddCustomerComponent,
    canActivate: [authGuard],
    data: { allowedRoles: ['Customer'] }
  },

  // Admin Dashboard Routes
  {
    path: 'admin',
    component: AdminDashboardComponent,
    canActivate: [authGuard],
    data: { allowedRoles: ['Admin'] },
    children: [
      {
        path: 'add-category',
        loadComponent: () =>
          import('./Component/Category/add-category/add-category.component').then(m => m.AddCategoryComponent)
      },
      {
        path: 'add-product',
        loadComponent: () =>
          import('./Component/Product/add-product/add-product.component').then(m => m.AddProductComponent)
      },
      {
        path: 'get-customers',
        loadComponent: () =>
          import('./Component/Customers/get-customers/get-customers.component').then(m => m.GetCustomersComponent)
      },
      {
        path: 'get-customer-by-id',
        loadComponent: () =>
          import('./Component/Customers/get-by-id/get-by-id.component').then(m => m.GetByIdComponent)
      },
      {
        path: 'customer-with-user',
        loadComponent: () =>
          import('./Component/Customers/customer-with-user-detail/customer-with-user-detail.component').then(
            m => m.CustomerWithUserDetailComponent
          )
      },
      {
        path: 'get-products',
        loadComponent: () =>
          import('./Component/Product/get-product/get-product.component').then(m => m.GetProductComponent)
      },
      {
        path: 'get-product-by-category',
        loadComponent: () =>
          import('./Component/Product/get-product-by-category/get-product-by-category.component').then(
            m => m.GetProductByCategoryComponent
          )
      },
      {
        path: 'search-product',
        loadComponent: () =>
          import('./Component/Product/search-product/search-product.component').then(m => m.SearchProductComponent)
      },
      {
        path: 'place-order',
        loadComponent: () =>
          import('./Component/PlaceOrder/place-order/place-order.component').then(m => m.PlaceOrderComponent)
      },
      {
        path: 'order-details',
        loadComponent: () =>
          import('./Component/PlaceOrder/get-order-detail/get-order-detail.component').then(
            m => m.GetOrderDetailComponent
          )
      }
    ]
  },

  // Customer Dashboard Routes
  {
    path: 'customer',
    component: CustomerDashboardComponent,
    canActivate: [authGuard],
    data: { allowedRoles: ['Customer'] },
    children: [
      {
        path: 'customer-dashboard',
        component: CustomerDashboardComponent
      },
      {
        path: 'view-products',
        loadComponent: () =>
          import('./Component/Product/get-product/get-product.component').then(m => m.GetProductComponent)
      },
      {
        path: 'get-product-by-category',
        loadComponent: () =>
          import('./Component/Product/get-product-by-category/get-product-by-category.component').then(
            m => m.GetProductByCategoryComponent
          )
      },
      {
        path: 'search-product',
        loadComponent: () =>
          import('./Component/Product/search-product/search-product.component').then(m => m.SearchProductComponent)
      },
      {
        path: 'place-order',
        loadComponent: () =>
          import('./Component/PlaceOrder/place-order/place-order.component').then(m => m.PlaceOrderComponent)
      },
      {
        path: 'order-details',
        loadComponent: () =>
          import('./Component/PlaceOrder/get-order-detail/get-order-detail.component').then(
            m => m.GetOrderDetailComponent
          )
      }
    ]
  },

  { path: 'logout', component: LogoutComponent },
  { path: 'header', component: HeaderComponent },
  { path: '**', redirectTo: 'login' } // fallback route
];
