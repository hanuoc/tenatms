import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule,Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { appRoutes } from './app.routes';
import { AuthGuard } from './core/guards/auth.guard';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';


import { CommonModule } from '@angular/common';
import { Daterangepicker } from 'ng2-daterangepicker';
import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';
import { NgIdleKeepaliveModule } from '@ng-idle/keepalive';
import { UtilityService } from 'app/core/services/utility.service';
import { AuthenService } from 'app/core/services/authen.service';
import { HttpClientModule, HttpClient } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [CommonModule,Daterangepicker,MultiselectDropdownModule,
    BrowserModule,
    FormsModule,
    HttpModule,
    HttpClientModule,
    RouterModule.forRoot(appRoutes, { useHash: true }),
    PaginationModule.forRoot(),
    NgIdleKeepaliveModule.forRoot()
    
  ],
  providers: [AuthGuard,UtilityService,AuthenService],
  bootstrap: [AppComponent]
})
export class AppModule { }
