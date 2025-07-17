export interface Transactions{
    transactionID? :number,
    paymnetMethod:PaymentMethod,
    amountPaid:number,
    transactionDate:Date,
    transactionStatus:TransactionStatus,
    OrderID:number
}
export enum PaymentMethod{
    
        CreditCard='CreditCard',
        PayPal='PayPal',
        Bank='Bank',
        Cash='Cash'
}
export enum TransactionStatus{
    Success='Success',
    Failed='Failed',
    Pending='Pending'
}