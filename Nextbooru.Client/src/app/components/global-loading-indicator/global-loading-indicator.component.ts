import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { debounceTime } from 'rxjs';
import { LoadingIndicatorService } from 'src/app/services/loading-indicator.service';

@Component({
  selector: 'app-global-loading-indicator',
  templateUrl: './global-loading-indicator.component.html',
  styleUrls: ['./global-loading-indicator.component.scss'],
  standalone: true,
  imports: [CommonModule]
})
export class GlobalLoadingIndicatorComponent {
  constructor(private loadingIndicatorService: LoadingIndicatorService) {}

  loading$ = this.loadingIndicatorService.loading$.pipe(
    debounceTime(100)
  );
}
