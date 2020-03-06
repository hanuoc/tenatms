import { ReportComponent } from './report.component'
import { Routes, RouterModule } from '@angular/router';
const routes: Routes = [
    { path: '', redirectTo: 'index', pathMatch: 'full' },
    { path: 'index', component: ReportComponent },

];
export const ReportRouter = RouterModule.forChild(routes);