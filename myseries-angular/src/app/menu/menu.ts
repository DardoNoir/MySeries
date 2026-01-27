import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  standalone: true,
  selector: 'app-menu',
  templateUrl: './menu.html',
  styleUrls: ['./menu.scss']
})
export class MenuComponent {
  constructor(
    private router: Router,
    private auth: AuthService
  ) {}

  goToSearch() {
    this.router.navigateByUrl('/series');
  }

  goToWatchlist() {
  this.router.navigateByUrl('/watchlist');
  }


  logout() {
    this.auth.logout();
    this.router.navigateByUrl('/login');
  }
}
