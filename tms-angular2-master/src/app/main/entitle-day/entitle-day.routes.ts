import { EntitleDayComponent } from './entitle-day.component';
import { Routes, RouterModule } from '@angular/router';
const routes: Routes = [
    { path: '', redirectTo: 'index', pathMatch: 'full' },
    { path: 'index', component: EntitleDayComponent }
];
export const EntitleDayRouter = RouterModule.forChild(routes);