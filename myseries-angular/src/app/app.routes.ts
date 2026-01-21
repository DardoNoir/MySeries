import { Routes } from '@angular/router';
import { SeriesComponent } from './series/series';

export const Route = [
      { path: '', component: SeriesComponent },
      {path: '**', redirectTo: ''}
]
