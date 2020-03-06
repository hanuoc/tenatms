
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TimeSheetComponent } from './time-sheet.component';
import { TimeSheetRouter } from './time-sheet.routes';
import { PaginationModule, ModalModule } from 'ngx-bootstrap';
import { FormsModule } from '@angular/forms';
import { DataService } from './../../core/services/data.service';
import { UtilityService } from './../../core/services/utility.service';
import { UploadService } from './../../core/services/upload.service';
import { Daterangepicker } from '../../shared/daterangepicker/daterangepicker.module';
import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';

@NgModule({
  imports: [
    CommonModule,
    TimeSheetRouter,
    FormsModule,
    PaginationModule,
    ModalModule,
    Daterangepicker,
    MultiselectDropdownModule
  ],
   declarations: [TimeSheetComponent],
  providers: [DataService, UtilityService, UploadService]
})
export class TimeSheetModule { }
