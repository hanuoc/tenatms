import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule, ModalModule } from 'ngx-bootstrap';
import { FormsModule } from '@angular/forms';
import { DataService } from './../../core/services/data.service';
import { UtilityService } from './../../core/services/utility.service';
import { UploadService } from './../../core/services/upload.service';
import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';
import { ExplanationRequestComponent } from './explanation-request.component';
import { ExplanationRequestRouter } from './explanation-request.routes';
import { Daterangepicker } from '../../shared/daterangepicker/daterangepicker.module';

@NgModule({
    imports: [
        CommonModule,
        ExplanationRequestRouter,
        FormsModule,
        PaginationModule,
        ModalModule,
        Daterangepicker,
        MultiselectDropdownModule
    ],
    declarations: [ExplanationRequestComponent],
    providers: [DataService, UtilityService, UploadService]
})
export class ExplanationRequestModule { }
