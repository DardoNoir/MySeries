import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient, HttpParams } from '@angular/common/http';

@Component({
  standalone: true,
  selector: 'app-qualification',
  imports: [CommonModule, FormsModule],
  templateUrl: './qualifications.html',
  styleUrls: ['./qualifications.scss']
})
export class QualificationComponent implements OnInit {

  serieId!: number;
  userId!: number;

  score = 5;
  review = '';
  loading = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    this.serieId = Number(this.route.snapshot.paramMap.get('id'));
    const user = JSON.parse(localStorage.getItem('user')!);
    this.userId = user.id;
  }

  submit() {
    this.loading = true;

    const params = new HttpParams()
      .set('userId', this.userId)
      .set('serieId', this.serieId)
      .set('score', this.score)
      .set('review', this.review);

    this.http
      .get('/api/app/qualifications/qualifications-series', { params })
      .subscribe({
        next: () => {
          this.loading = false;
          this.router.navigateByUrl('/watchlist');
        },
        error: () => this.loading = false
      });
  }

  cancel() {
    this.router.navigateByUrl('/watchlist');
  }
}
