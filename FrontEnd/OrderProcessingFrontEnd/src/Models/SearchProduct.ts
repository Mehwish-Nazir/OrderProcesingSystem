export  interface ProductSearchRequest{
    searchText:string;
    pageNumber:number;
    pageSize:number;
}

//Response INTERFACE
export interface ProductResponse {
  productID: number;
  productName: string;
  price: number;
  stock: number;
  categoryName: string;
  createdAt: string;      // use string if coming as ISO date
  stockStatus: string;    // "Out of Stock", "In Stock", etc.
}



export interface PagedProductResponse {
  products: ProductResponse[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}
