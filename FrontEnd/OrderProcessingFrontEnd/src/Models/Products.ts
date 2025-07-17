import { Category } from "./Category";

export interface ProductsWithCategory{
    productID?:number;
    productName:string;
    price:number;
    stock:number,
    category:Category;
}

export interface CreateNewProduct{
    productID?:number,
    productName:string,
    price:number,
    stock:number,
    categoryID:number,
    createdAt?:Date
}

