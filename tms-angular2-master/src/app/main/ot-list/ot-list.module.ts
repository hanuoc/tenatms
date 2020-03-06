
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OtListComponent } from './ot-list.component';
import { OTListRouter } from './ot-list.routes';
import { PaginationModule, ModalModule } from 'ngx-bootstrap';
import { FormsModule } from '@angular/forms';
import { DataService } from './../../core/services/data.service';
import { UtilityService } from './../../core/services/utility.service';
import { UploadService } from './../../core/services/upload.service';
import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';
import { Daterangepicker } from '../../shared/daterangepicker/daterangepicker.module';

@NgModule({
    imports: [
      CommonModule,
      OTListRouter,
      FormsModule,
      PaginationModule,
      ModalModule,
      Daterangepicker,
      MultiselectDropdownModule
    ],
     declarations: [OtListComponent],
    providers: [DataService, UtilityService, UploadService]
  })
  export class OTListModule { }
  