import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';  // para *ngIf y *ngFor
import { FormsModule } from '@angular/forms';    // para [(ngModel)]
import { OmdbService, OmdbSeriesSearchItemDto } from '../../../services/omdb.services';

@Component({
  selector: 'app-series-search',
  standalone: true,
  imports: [CommonModule, FormsModule], // 👈 acá van
  templateUrl: './series-search.html',
  styleUrls: ['./series-search.scss']
})
export class SeriesSearch {
  searchTerm = '';
  results: OmdbSeriesSearchItemDto[] = [];
  loading = false;

  constructor(private omdbService: OmdbService) {}

  search() {
    if (!this.searchTerm.trim()) return;

    this.loading = true;
    this.omdbService.searchByTitle(this.searchTerm).subscribe({
      next: (res) => {
        this.results = res.search || [];
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }
}
