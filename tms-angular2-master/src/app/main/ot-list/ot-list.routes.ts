import { OtListComponent } from './ot-list.component';
import { Routes, RouterModule } from '@angular/router';
const routes: Routes = [
    { path: '', redirectTo: 'index', pathMatch: 'full' },
    { path: 'index', component: OtListComponent }
];
export const OTListRouter = RouterModule.forChild(routes);