import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule, NgFor, NgIf } from '@angular/common';
import { SeriesService } from '../../services/series.service';
import { OmdbSeriesDto } from '../../models/omdb-series.model';

@Component({
  selector: 'app-series-detail',
  standalone: true,
  imports: [CommonModule], // ✅ This is key
  templateUrl: './series-detail.html',
  styleUrls: ['./series-detail.scss']
})
export class SeriesDetailComponent implements OnInit {
  series: OmdbSeriesDto | null = null;
  loading = true; // ✅ Add this

  constructor(
    private route: ActivatedRoute,
    private seriesService: SeriesService
  ) {}

  ngOnInit() {
    const imdbId = this.route.snapshot.paramMap.get('id');
    if (imdbId) {
      this.seriesService.getFromOmdb_get(imdbId).subscribe({
        next: (res) => {
          this.series = res;
          this.loading = false;
        },
        error: (err) => {
          console.error(err);
          this.loading = false;
        }
      });
    } else {
      this.loading = false;
    }
  }
}
