import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login.component';
import { FormsModule } from '@angular/forms';
import { NotificationService } from '../core/services/notification.service';
import { AuthenService } from '../core/services/authen.service';
import { ModalModule } from 'ngx-bootstrap/modal';

import { Mock } from 'protractor/built/driverProviders';
export const routes: Routes = [
  //localhost:4200/login
  { path: '', component: LoginComponent }
];
@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild(routes),
    ModalModule.forRoot()
  ],
  providers: [AuthenService, NotificationService],
  declarations: [LoginComponent]
})
export class LoginModule { }
