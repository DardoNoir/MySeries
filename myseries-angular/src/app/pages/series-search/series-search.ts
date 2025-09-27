import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SeriesService } from '../../services/series.service';
import { OmdbSeriesSearchDto, OmdbSeriesSearchItemDto } from '../../models/omdb-search.model';

@Component({
  selector: 'app-series-search',
  standalone: true,                     // Standalone component
  imports: [CommonModule, FormsModule, RouterModule],  // ✅ This is the key
  templateUrl: './series-search.html',
  styleUrls: ['./series-search.scss']
})
export class SeriesSearchComponent {
  title = '';
  genre = '';
  results: OmdbSeriesSearchItemDto[] = [];
  loading = false;
  error?: string;

  constructor(private seriesService: SeriesService) {}

  search() {
    if (!this.title) { this.error = 'Enter a title'; return; }
    this.loading = true;
    this.error = undefined;

    // choose the right method based on your Swagger (GET or POST)
    this.seriesService.searchSeries(this.title, this.genre)
      .subscribe({
        next: res => {
          this.results = res?.search ?? [];
          this.loading = false;
        },
        error: err => {
          console.error(err);
          this.error = 'Error fetching results';
          this.loading = false;
        }
      });
    // Implement service call here later
    console.log('Searching', this.title, this.genre);
  }
}
