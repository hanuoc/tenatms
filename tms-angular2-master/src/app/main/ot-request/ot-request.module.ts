
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OTRequestComponent } from './ot-request.component';
import { OTRequestRouter } from './ot-request.routes';
import { PaginationModule, ModalModule } from 'ngx-bootstrap';
import { FormsModule } from '@angular/forms';
import { DataService } from './../../core/services/data.service';
import { UtilityService } from './../../core/services/utility.service';
import { UploadService } from './../../core/services/upload.service';
import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';
import { Daterangepicker } from '../../shared/daterangepicker/daterangepicker.module';
import { AmazingTimePickerModule } from 'amazing-time-picker';
//import { SimpleTinyComponent } from '../../shared/simple-tiny/simple-tiny.component';

@NgModule({
  imports: [
    CommonModule,
    OTRequestRouter,
    FormsModule,
    PaginationModule,
    ModalModule,
    Daterangepicker,
    MultiselectDropdownModule,
    AmazingTimePickerModule
  ],
   declarations: [OTRequestComponent],
  providers: [DataService, UtilityService, UploadService]
})
export class OTRequestModule { }
