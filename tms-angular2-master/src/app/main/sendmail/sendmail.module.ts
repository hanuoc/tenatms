import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SendmailComponent } from './sendmail.component';
import { SendMailRouter } from './sendmail.routes';
import { PaginationModule, ModalModule } from 'ngx-bootstrap';
import { FormsModule } from '@angular/forms';
import { DataService } from './../../core/services/data.service';
import { UtilityService } from './../../core/services/utility.service';
import { UploadService } from './../../core/services/upload.service';
import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';
import { SimpleTinyComponent } from '../../shared/simple-tiny/simple-tiny.component';

@NgModule({
    imports: [
      CommonModule,
      SendMailRouter,
      FormsModule,
      PaginationModule,
      ModalModule,
      MultiselectDropdownModule,
      
    ],
     declarations: [SendmailComponent,SimpleTinyComponent],
    providers: [DataService, UtilityService, UploadService]
  })
  export class SendMailModule { }