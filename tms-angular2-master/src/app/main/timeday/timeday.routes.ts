import { TimedayComponent } from './timeday.component';
import { Routes, RouterModule } from '@angular/router';
const routes: Routes = [
    { path: '', redirectTo: 'index', pathMatch: 'full' },
    { path: 'index', component: TimedayComponent }
];
export const TimedayRouter = RouterModule.forChild(routes);