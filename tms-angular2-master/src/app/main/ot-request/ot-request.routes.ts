import { OTRequestComponent } from './ot-request.component';
import { Routes, RouterModule } from '@angular/router';
const routes: Routes = [
    { path: '', redirectTo: 'index', pathMatch: 'full' },
    { path: 'index', component: OTRequestComponent }
];
export const OTRequestRouter = RouterModule.forChild(routes);