import { HttpInterceptorFn } from '@angular/common/http';
import { HttpRequest, HttpEvent, HttpHandler, HttpHandlerFn } from '@angular/common/http';
//Register this interceptor in app.compnnet liek this 
/*
 providers: [
    provideHttpClient(
      withInterceptors([authInterceptor])   //  ✅ <-- registers globally
    )
  ]
*/ 


//Actual impmetation


/*
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  //get token
  const token=localStorage.getItem('token')
  if(token){
    const authRequest=req.clone({
   setHeaders:{'Authorization': `Bearer ${token}`}
    });
    return next(authRequest);   //next the clone request to backend 
  }else{
     //if no token get, then just return the original request like if Api are not annotated with 'Authorize'
  return next(req);
  }
  
 
};
*/
