import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';
import { provideHttpClient } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
/*bootstrapApplication(AppComponent, appConfig)
  .catch((err) => console.error(err));*/
  
bootstrapApplication(AppComponent,{
  providers: [
    provideRouter(routes),    provideHttpClient() //  This provides HttpClient globally
  ],
}).catch((err) => console.error(err));
