import { Routes } from '@angular/router';
import { SeriesSearchComponent } from './pages/series-search/series-search';
import { SeriesDetailComponent } from './pages/series-detail/series-detail';

export const routes: Routes = [
  { path: '', component: SeriesSearchComponent },
  { path: 'detail/:id', component: SeriesDetailComponent },
  { path: '**', redirectTo: '' }
];

