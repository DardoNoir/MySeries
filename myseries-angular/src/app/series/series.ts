import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SerieDto } from '../models/SerieDto';
import { SerieService } from '../services/serie.service';

@Component({
  selector: 'app-series',      
  standalone: true,                     
  imports: [CommonModule, FormsModule, RouterModule],  
  templateUrl: './series.html',
  styleUrls: ['./series.scss']
})

export class SeriesComponent {
  title = '';
  loading = false;
  series: SerieDto[]=[];
  error?: string;

  constructor(private serieService: SerieService) {}


  search(): void{
    if(!this.title.trim()){
      this.error = 'Enter a title';
      return;
    }

    this.loading = true;
    this.series = [];
    this.error = undefined

    this.serieService.searchSeries(this.title).subscribe({
      next: (result) => {
        this.series = result;
        this.loading = false;
      },
      error: err => {
        console.error(err);
        this.error = 'Error fetching results';
        this.loading = false;
      }
    });
    console.log('Searching', this.title);
  }
}
