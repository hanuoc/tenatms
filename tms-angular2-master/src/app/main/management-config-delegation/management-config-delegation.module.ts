import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManagementConfigDelegationComponent } from './management-config-delegation.component';
import { ManagementConfigDelegationRouter } from './management-config-delegation.routes';
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
      ManagementConfigDelegationRouter,
      FormsModule,
      PaginationModule,
      ModalModule,
      MultiselectDropdownModule,
      Daterangepicker
    ],
     declarations: [ManagementConfigDelegationComponent],
    providers: [DataService, UtilityService, UploadService]
  })
  export class ManagementConfigDelegationModule { }