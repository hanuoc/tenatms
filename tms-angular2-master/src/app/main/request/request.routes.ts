import { RequestComponent } from './request.component';
import { Routes, RouterModule } from '@angular/router';
const routes: Routes = [
    { path: '', redirectTo: 'index', pathMatch: 'full' },
    { path: 'index', component: RequestComponent }
];
export const RequestRouter = RouterModule.forChild(routes);