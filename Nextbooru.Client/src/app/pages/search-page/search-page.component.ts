import { Component } from '@angular/core';
import { FormControl } from '@angular/forms';
import { SpinnerOverlayComponent } from 'src/app/components/spinner-overlay/spinner-overlay.component';
import { SharedModule } from 'src/app/shared/shared.module';

@Component({
  selector: 'app-search-page',
  standalone: true,
  imports: [SharedModule, SpinnerOverlayComponent],
  templateUrl: './search-page.component.html',
  styleUrls: ['./search-page.component.scss']
})
export class SearchPageComponent {
  searchTerm = new FormControl('');
}
