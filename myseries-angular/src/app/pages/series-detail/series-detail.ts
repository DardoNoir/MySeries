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
  imdbId?: string;
  model?: OmdbSeriesDto;
  loading = false;
  constructor(private route: ActivatedRoute, private service: SeriesService) {}

  ngOnInit() {
    this.imdbId = this.route.snapshot.paramMap.get('id') ?? undefined;
    if (!this.imdbId) return;
    this.loading = true;
    // choose get/post variant matching Swagger
    this.service.getFromOmdb_get(this.imdbId).subscribe({
      next: r => { this.model = r; this.loading = false; },
      error: e => { console.error(e); this.loading = false; }
    });
  }
}

