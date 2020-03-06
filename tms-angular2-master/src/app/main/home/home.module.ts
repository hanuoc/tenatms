import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { Routes, RouterModule } from '@angular/router';
import { Daterangepicker } from '../../shared/daterangepicker/daterangepicker.module';
import { AmazingTimePickerModule } from 'amazing-time-picker';
import{ FlotCmp }  from '../../shared/directives/network-activities.component';
import { FormsModule } from '@angular/forms';
import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';
const homeRoutes: Routes = [
   //localhost:4200/main/user
  { path: '', redirectTo: 'index', pathMatch: 'full' },
   //localhost:4200/main/home/index
  { path: 'index', component: HomeComponent }
]
@NgModule({
  imports: [
    CommonModule,
    Daterangepicker,
    FormsModule,
    MultiselectDropdownModule,
    RouterModule.forChild(homeRoutes)
  ],
  declarations: [HomeComponent,FlotCmp]
})
export class HomeModule { }
