import { Routes } from '@angular/router';
import { SeriesComponent } from './series/series';
import { authGuard } from './login/auth.guard';
import { LoginComponent } from './login/login';
import { MenuComponent } from './menu/menu';
import { RegisterComponent } from './register/register';
import { WatchlistComponent } from './watchlists/watchlists';


export const Route: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent},
  { path: 'menu', component: MenuComponent, canActivate: [authGuard] },
  { path: 'series', component: SeriesComponent, canActivate: [authGuard] },
  { path: 'watchlist', component: WatchlistComponent, canActivate: [authGuard] },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' }
];
