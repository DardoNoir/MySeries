import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { WatchlistService } from '../services/watchlist.service';
import { WatchlistSerieDto } from '../models/WatchlistSerieDto';

@Component({
  standalone: true,
  selector: 'app-watchlist',
  imports: [CommonModule, FormsModule],
  templateUrl: './watchlists.html',
  styleUrls: ['./watchlists.scss']
})
export class WatchlistComponent implements OnInit {

  series: WatchlistSerieDto[] = [];
  searchText = '';
  loading = false;
  userId!: number;

  constructor(
    private watchlistService: WatchlistService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const user = JSON.parse(localStorage.getItem('user')!);
    this.userId = user.id;
    this.loadWatchlist();
  }
  // Cargar la Watchlist
  loadWatchlist() {
    this.loading = true;
    this.watchlistService.getWatchlist(this.userId).subscribe({
      next: res => {
        this.series = res;
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  filteredSeries() {
    return this.series.filter(s =>
      s.title.toLowerCase().includes(this.searchText.toLowerCase())
    );
  }

  // Eliminar Series
  removeSerie(serie: WatchlistSerieDto) {
    if (!serie.id) return;

    this.watchlistService.removeSeries(serie.id, this.userId).subscribe(() => {
      this.loadWatchlist();
    });
  }

  goToSearch() {
    this.router.navigateByUrl('/series');
  }

  goToQualification(serie: WatchlistSerieDto) {
  this.router.navigate(['/qualification', serie.id]);
}


  goToMenu() {
    this.router.navigateByUrl('/menu');
  }
}
