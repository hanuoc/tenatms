import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {platformBrowserDynamic} from '@angular/platform-browser-dynamic';

import { UserComponent } from './user.component';
// platformBrowserDynamic().bootstrapModule(UserComponent);
import { Routes, RouterModule } from '@angular/router';
import { DataService } from '../../core/services/data.service';
import { NotificationService } from '../../core/services/notification.service';
import { UploadService } from '../../core/services/upload.service';
import { Daterangepicker } from '../../shared/daterangepicker/daterangepicker.module';

import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap/modal';
import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';
// import { Daterangepicker } from 'ng2-daterangepicker';
import { UserDetailComponent } from 'app/main/user/user-detail/user-detail.component';

const userRoutes: Routes = [
  //localhost:4200/main/user
  { path: '', redirectTo: 'index', pathMatch: 'full' },
  //localhost:4200/main/user/index
  { path: 'index', component: UserComponent },
  //localhost:4200/main/user/detail
  { path: 'detail/', component: UserDetailComponent }
]
@NgModule({
  imports: [
    CommonModule,
    PaginationModule,
    FormsModule,
    MultiselectDropdownModule,
    Daterangepicker,
    ModalModule.forRoot(),
    RouterModule.forChild(userRoutes)
  ],
  declarations: [UserComponent, UserDetailComponent],
  providers: [DataService, NotificationService,UploadService]
})
export class UserModule { }