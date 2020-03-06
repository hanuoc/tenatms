import { AbnormalcaseComponent } from './abnormalcase.component';
import { Routes, RouterModule } from '@angular/router';
const routes: Routes = [
    { path: '', redirectTo: 'index', pathMatch: 'full' },
    { path: 'index', component: AbnormalcaseComponent }
];
export const  AbnormalcaseRouter = RouterModule.forChild(routes);