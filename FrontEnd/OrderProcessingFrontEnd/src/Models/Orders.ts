export interface Orders{
    orderID:number,
    orderDate:Date,
    totalAmount:number,
    orderStatus:string,
    CustomerID?:number
}