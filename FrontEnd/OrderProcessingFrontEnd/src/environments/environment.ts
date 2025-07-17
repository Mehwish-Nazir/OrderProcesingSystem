export const environment = {
    production: false,
    loginURL: 'https://localhost:5000/api/auth/login',
    registerURL:'http://localhost:5000/api/auth/register',
    getRoleURL:'https://localhost:5000/api/auth/roles',
    getCustomerURL:'https://localhost:5000/api/customers',
    getCustomerByIdURL: 'https://localhost:5000/api/customers',
    getCustomerWithUserURL:'https://localhost:5000/api/customers/with-users' , 
    addCustomerURL:'https://localhost:5000/api/customers/AddCustomers',
    logoutURL:'https://localhost:5000/api/auth/logout',
    addCategoryURL:'https://localhost:5000/api/categories/add_category',
    getAllCategroiesURL:'https://localhost:5000/api/categories/allCategories',
    getAllProductsWithCategoriesURL:'https://localhost:5000/api/product/fecthProductWithCategory',
    addProductURL: 'https://localhost:5000/api/product/add-product', 
    getProductbyCategoryIdURL:'https://localhost:5000/api/product/getProductbyCategory',
    paymentMethodURL:'https://localhost:5000/api/transaction/PaymentMethod',
    placeOrderURL: 'https://localhost:5000/api/OrderPlace/placeOrder',
    orderDetailsURL:'https://localhost:5000/api/OrderPlace/orderDetails',
    searchProductURL:'https://localhost:5000/api/product/search',
    getPrfileNameURL:'https://localhost:5000/api/auth/getProfile'

};