import { Component, OnInit } from '@angular/core';
import { MonitoringService } from '../services/monitoring.service';
import { MonitoringDto } from '../models/MonitoringDto';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  standalone: true,
  selector:'app-monitoring',
  templateUrl: './monitoring.html',
  styleUrls: ['./monitoring.scss'],
  imports: [CommonModule] 
})
export class MonitoringComponent implements OnInit {
  stats?: MonitoringDto;

  constructor(private monitoringService: MonitoringService, private router: Router) {}

  // Obtener estadísticas
  ngOnInit(): void {
    this.monitoringService.getStats()
      .subscribe(data => this.stats = data);
  }

  // Volver al menú
  goBack(): void {
    this.router.navigateByUrl('/menu');
  }
}