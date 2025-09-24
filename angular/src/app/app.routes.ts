import { Routes } from '@angular/router';
import { SeriesSearch } from './components/series-search/series-search';

export const routes: Routes = [
  { path: 'series-search', component: SeriesSearch },
  { path: '', redirectTo: 'series-search', pathMatch: 'full' } // opcional: home = búsqueda
];
