create database OrderProcessingDb;
Use OrderProcessingDb
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(250) NOT NULL,
    Email NVARCHAR(250) NOT NULL,
    Role NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

Drop table Users
ALTER TABLE OrderProcessingDb.dbo.Users
DROP COLUMN CustomerName;
DELETE FROM OrderProcessingDb.dbo.Users;
 Use OrderProcessingDb
CREATE TABLE Customers (
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,          -- Auto-increment PK

    FirstName NVARCHAR(100) NOT NULL,                 -- Required
    LastName NVARCHAR(100) NOT NULL,                  -- Required
    Email NVARCHAR(255) NOT NULL,                     -- Required
    PhoneNumber NVARCHAR(20) NOT NULL,                -- Required, nullable in model but marked [Required] attribute
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),-- Default to UTC now

    UserID INT UNIQUE NULL,                           -- Nullable FK to Users (one-to-one)
    CONSTRAINT FK_Customers_Users FOREIGN KEY (UserID)
        REFERENCES Users(UserID)
);

select *from OrderProcessingDb.dbo.Users;
select *from OrderProcessingDb.dbo.Customers;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              select *from OrderProcessingDb.dbo.Orders;
select *from OrderProcessingDb.dbo.Category;
select *from OrderProcessingDb.dbo.OrderProducts;
select *from OrderProcessingDb.dbo.Product;
select *from OrderProcessingDb.dbo.Orders;
select *from OrderProcessingDb.dbo.Transactions;
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE '%Orders%'
Delete  from OrderProcessingDb.dbo.Product where ProductName='Fridge' AND CategoryID=1;

delete from  OrderProcessingDb.dbo.Customers where UserID is NULL;
EXEC sp_help 'Customers';
Exec sp_help 'Users';


ALTER TABLE Customers ADD UserID INT UNIQUE;
ALTER TABLE Customers ADD CONSTRAINT FK_User FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE SET NULL;
ALTER TABLE OrderProcessingDb.dbo.Customers
DROP CONSTRAINT UQ__Customer__1788CCAD46297EBC;

CREATE UNIQUE NONCLUSTERED INDEX UQ_Customer_PhoneNumber
ON OrderProcessingDb.dbo.Customers (PhoneNumber)
WHERE PhoneNumber IS NOT NULL;

ALTER TABLE OrderProcessingDb.dbo.Customers
ADD CONSTRAINT UQ__Customer__1788CCAD46297EBC UNIQUE (PhoneNumber) 
WHERE PhoneNumber IS NOT NULL;
-- Drop the UNIQUE constraint
ALTER TABLE Customers DROP CONSTRAINT FK_User;
ALTER TABLE Customers
ALTER COLUMN UserID INT NULL;
select * from OrderProcessingDb.dbo.Customers;
select * from OrderProcessingDb.dbo.Orders;
Use OrderProcessingDb 
  CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerID INT NOT NULL, -- Foreign key from Customers
    OrderDate DATETIME DEFAULT GETUTCDATE(), -- Always store in UTC
    TotalAmount DECIMAL(10,2) NOT NULL CHECK (TotalAmount >= 0), -- Ensures valid total amount
    OrderStatus VARCHAR(50) NOT NULL CHECK (OrderStatus IN ('Pending', 'Processing', 'Shipped', 'Delivered', 'Cancelled')), 

    CONSTRAINT FK_Customer FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID) ON DELETE CASCADE
);
ALTER TABLE OrderProcessingDb.dbo.Orders
ALTER COLUMN OrderDate DATETIME NOT NULL;

select *from OrderProcessingDb.dbo.Orders;
DELETE FROM OrderProcessingDb.dbo.Orders 
WHERE ProductID IS NULL;

ALTER TABLE OrderProcessingDb.dbo.Orders
ADD ProductID INT ;

ALTER TABLE Orders
ADD CONSTRAINT FK_Order_Product
FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
ON DELETE SET NULL;



select *from OrderProcessingDb.dbo.Customers;
Insert Into OrderProcessingDb.dbo.Customers(FirstName, LastName, Email, PhoneNumber,UserID)
Values
('Ali', 'Khan', 'ali.khan@example.com', '03001234567',2),
('Sara', 'Ahmed', 'sara.ahmed@example.com', '03111234567',3),
('Zain', 'Malik', 'zain.malik@example.com', '03221234567',6),
('Ayesha', 'Noor', 'ayesha.noor@example.com', '03331234567', 7);


Use OrderProcessingDb
CREATE TABLE Transactions (
    TransactionID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL, -- Foreign key from Orders
    PaymentMethod VARCHAR(50) CHECK (PaymentMethod IN ('Credit Card', 'PayPal', 'Bank Transfer', 'Cash')),
    AmountPaid DECIMAL(10,2) NOT NULL CHECK (AmountPaid > 0), 
    TransactionDate DATETIME DEFAULT GETUTCDATE(), -- Always store in UTC
    TransactionStatus VARCHAR(50) CHECK (TransactionStatus IN ('Success', 'Failed', 'Pending')),
    CONSTRAINT FK_Order FOREIGN KEY (OrderID) REFERENCES Orders(OrderID) ON DELETE CASCADE
);
select *from Customers;
use OrderProcessingDb
create table Category(
 CategoryID INT identity(1,1) Primary Key,
 CategoryName varchar(100) NOT NULL UNIQUE,
 CretaedAt DateTime Default GETUTCDATE()
);

USE OrderProcessingDb
create table Product(
ProductID int Identity(1,1) PRIMARY KEY,
ProductName varchar(100) Not Null,
Price Decimal(10,2) Not Null Check(Price>=0),
Stock INT NOT NULL CHECK(stock>=0),
CreatedAt DateTime Default GETUTCDATE(),
CategoryID int Not Null,
   CONSTRAINT FK_Category_Product FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID) ON DELETE CASCADE
);

select *from OrderProcessingDb.dbo.Product;

CREATE TABLE OrderProducts (
    OrderProductID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1, 
    PriceAtPurchase DECIMAL(10,2),
    CreatedAt DATETIME DEFAULT GETUTCDATE(),

    CONSTRAINT FK_OrderProduct_Order FOREIGN KEY (OrderID) REFERENCES Orders(OrderID) ON DELETE CASCADE,
    CONSTRAINT FK_OrderProduct_Product FOREIGN KEY (ProductID) REFERENCES Product(ProductID) ON DELETE CASCADE
);

select *from OrderProcessingDb.dbo.Category;
EXEC sp_rename 'OrderProcessingDb.dbo.Category.CretaedAt', 'CreatedAt', 'COLUMN';
ALTER TABLE OrderProcessingDb.dbo.Category DROP COLUMN CretaedAt;
ALTER TABLE OrderProcessingDb.dbo.Category DROP CONSTRAINT DF__Category__Cretae__70DDC3D8;
ALTER TABLE OrderProcessingDb.dbo.Category ADD CreatedAt DATETIME DEFAULT GETUTCDATE();


DELETE FROM __EFMigrationsHistory;
SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('Product');
CREATE INDEX IX_Product_ProductName
ON OrderProcessingDb.dbo.Product (ProductName);
-----Create INDEXES----
CREATE INDEX IX_Category_CategoryName
ON OrderProcessingDb.dbo.Category (CategoryName);