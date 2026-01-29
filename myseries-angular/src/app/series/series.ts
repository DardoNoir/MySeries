import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SerieDto } from '../models/SerieDto';
import { SerieService } from '../services/serie.service';
import { WatchlistService } from '../services/watchlist.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-series',      
  standalone: true,                     
  imports: [CommonModule, FormsModule, RouterModule],  
  templateUrl: './series.html',
  styleUrls: ['./series.scss']
})

export class SeriesComponent {
  title = '';
  genre = ''; // valor seleccionado
  loading = false;
  series: SerieDto[] = [];
  error?: string;

  // ✅ Lista de géneros para el combo
  genres: string[] = [
   // '',
    'Action',
    'Comedy',
    'Drama',
    'Crime',
    'Sci-Fi',
    'Fantasy',
    'Thriller',
    'Romance',
    'Horror'
  ];

  constructor(
    private serieService: SerieService,
    private watchlistService: WatchlistService,
    private router: Router
  ) {}

  search(): void {
    if (!this.title.trim()) {
      this.error = 'Enter a title';
      return;
    }

    this.loading = true;
    this.series = [];
    this.error = undefined;

    this.serieService.searchSeries(this.title, this.genre).subscribe({
      next: result => {
        this.series = result;
        this.loading = false;
      },
      error: () => {
        this.error = 'Error fetching results';
        this.loading = false;
      }
    });
  }

  addToFavorites(serie: SerieDto) {
    const user = JSON.parse(localStorage.getItem('user')!);
    this.watchlistService.addSeries(serie.imdbId!, user.id).subscribe(() => {
      alert('Serie agregada a favoritas ⭐');
    });
  }

  goBack(): void {
    this.router.navigateByUrl('/menu');
  }
}
