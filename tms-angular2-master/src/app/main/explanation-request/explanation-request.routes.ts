import { Routes, RouterModule } from '@angular/router';
import { ExplanationRequestComponent } from './explanation-request.component';
const routes: Routes = [
    { path: '', redirectTo: 'index', pathMatch: 'full' },
    { path: 'index', component: ExplanationRequestComponent }
];
export const ExplanationRequestRouter = RouterModule.forChild(routes);