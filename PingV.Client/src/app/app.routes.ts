import { Routes } from '@angular/router';
import { AvailabilityPage } from './components/pages/availability-page/availability-page';
import { MainLayout } from './components/layout/main-layout/main-layout';


export const routes: Routes = [
  {
    path: '',
    component: MainLayout,
    children: [
      { path: '', component: AvailabilityPage },
      // add more pages  
    ]
  },
  { path: '**', redirectTo: '' } 
  // routes outside the layout (e.g., 404 page)  
  // { path: '**', component: NotFoundComponent }
];