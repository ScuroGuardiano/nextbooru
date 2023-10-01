import { Component, signal } from '@angular/core';
import { FormControl } from '@angular/forms';
import { SharedModule } from 'src/app/shared/shared.module';

@Component({
  selector: 'app-search-page',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './search-page.component.html',
  styleUrls: ['./search-page.component.scss']
})
export class SearchPageComponent {
  searchTerm = new FormControl('');
}
