
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TimedayComponent } from './timeday.component';
import { TimedayRouter } from './timeday.routes';
import { PaginationModule, ModalModule } from 'ngx-bootstrap';
import { FormsModule } from '@angular/forms';
import { DataService } from './../../core/services/data.service';
import { UtilityService } from './../../core/services/utility.service';
import { UploadService } from './../../core/services/upload.service';
import { Daterangepicker } from '../../shared/daterangepicker/daterangepicker.module';
import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';
import { BrowserModule } from '@angular/platform-browser';
import { AmazingTimePickerModule } from 'amazing-time-picker'; 


@NgModule({
  imports: [
    CommonModule,
    TimedayRouter,
    FormsModule,
    PaginationModule,
    ModalModule,
    Daterangepicker,
    MultiselectDropdownModule,
    AmazingTimePickerModule
  ],
   declarations: [TimedayComponent],
  providers: [DataService, UtilityService, UploadService ]
})
export class TimedayModule { }
