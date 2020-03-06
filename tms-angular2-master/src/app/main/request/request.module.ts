
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RequestComponent } from './request.component';
import { RequestRouter } from './request.routes';
import { PaginationModule, ModalModule } from 'ngx-bootstrap';
import { FormsModule } from '@angular/forms';
import { DataService } from './../../core/services/data.service';
import { UtilityService } from './../../core/services/utility.service';
import { UploadService } from './../../core/services/upload.service';
import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';
import {Daterangepicker} from '../../shared/daterangepicker/daterangepicker.module';

@NgModule({
  imports: [
        CommonModule,
    RequestRouter,
    FormsModule,
    PaginationModule,
    ModalModule,
    Daterangepicker,
    MultiselectDropdownModule
  ],
   declarations: [RequestComponent],
  providers: [DataService, UtilityService, UploadService]
})
export class RequestModule { }
