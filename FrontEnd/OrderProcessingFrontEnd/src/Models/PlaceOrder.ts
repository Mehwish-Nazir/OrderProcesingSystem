export interface PlaceOrderItem{
    productName:string,
    quantity:number,
   // priceAtPurchase:number,
}

export interface PlaceOrder{
    items:PlaceOrderItem[],
    paymentMethod:string,
    orderDate:Date,
}

//To get the detail of placed order

export interface GetOrderDetail{
    orderID:number,
     orderDate:Date,
     paymentMethod:string,
     orderStatus:string,
     totalAmount:number,
     items:GetOrderItem[],
     customerName:string

}

export interface GetOrderItem{
productName:string,
quantity:number,
priceAtPurchase:number
}