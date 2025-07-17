#  Order Processing System (E-Commerce Related)

A full-stack Order Processing System built with **ASP.NET Core (Web API)** and **Angular**, featuring **JWT-based authentication** and comprehensive order management.

---

##  Authentication APIs

###  Register
- Endpoint: `POST /api/Auth/register`
- Registers a new user.

###  Login
- Endpoint: `POST /api/Auth/login`
-**Authenticates the user and returns a JWT token, which is included in the Authorization header for all protected API requests from the frontend**.


---

##  Customer APIs

###  Add Customer
- Endpoint: `POST /api/Customers`
- Adds a new customer to the system.
### View All Customers
- Endpoint: `GET /api/Customers`
- Get All regitsered Customer based on UserID
### View Customer by ID 
- Endpoint: `Get /api/Customers/{id}`
- Get Customer by customer ID 
### View Customers With User Detail
- Endpoint: `Get /api/Customers/with-users`

---


### Category Management 

### Get Categories
- Endpoint: `Get /api/Category`
- Get All the Categories from DataBase
### Get Category By CategoryID 
- Endpoint: `Get /api/Categories/{id}`
- Fetch the Category by the category ID
### Add Category 
- Endpoint: `POST /api/Category/addCategory`
- This Api is used to add new Category into Databse 




---

##  Product Management

### Add Category
- Endpoint: `POST /api/Categories`
- Creates a new product category.

###  Add Product
- Endpoint: `POST /api/Products`
- Adds a new product under a selected category and their Stocks.

###  View/Search Products
- Endpoint: `GET /api/Products`
- Supports viewing all products and filtering/searching by name or category.

### View Product by ID 

- Endpoint: `GET/api/Product/{id}`
- Fetch Product By ProductID 

### Fecth Product based on  Category 
- Endpoint: `GET/api/Product/fetchProductWithCategory`
- This fetch all Products with related Categories
---

##  Order Management

###  Place Order
- Endpoint: `POST /api/Orders`
- Places a new order for one or more products by a customer.

###  Get Order Details
- Endpoint: `GET /api/Orders/{orderId}`
- Retrieves full order details including products and customer info.

---



##  Technologies Used

- **Backend**: ASP.NET Core Web API
- **Frontend**: Angular
- **Database**: SSMS (Sql Server Management Studio)
- **Authentication**: JWT (JSON Web Tokens)
- **Architecture**: Repository-Service-Controller pattern
- **API Testing Tool**: SwaggerUI



---

##  Setup Instructions

1. Clone the repository
2. Set up the SSMS Database and update the connection string in `appsettings.json`
3. Run backend with `dotnet run`
4. Navigate to `/FrontEnd/OrderProcessingFrontEnd/` and run:
   ```bash
   npm install
   ng serve

5. Swagger API is accessible via:
http://localhost:5000/swagger/index.html


