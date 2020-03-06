import { RouterModule, Routes } from '@angular/router';
import { HolidayComponent } from './holiday.component';
const routes: Routes = [
    { path: '', redirectTo: 'index', pathMatch: 'full' },
    { path: 'index', component: HolidayComponent }
];
export const HolidayRouter = RouterModule.forChild(routes);