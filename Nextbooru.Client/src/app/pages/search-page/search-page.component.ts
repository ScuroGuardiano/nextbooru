import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SpinnerOverlayComponent } from 'src/app/components/spinner-overlay/spinner-overlay.component';
import { TagsAutocompleteDirective } from 'src/app/directives/tags-autocomplete.directive';
import { SharedModule } from 'src/app/shared/shared.module';

@Component({
  selector: 'app-search-page',
  standalone: true,
  imports: [
    SharedModule,
    SpinnerOverlayComponent,
    TagsAutocompleteDirective
  ],
  templateUrl: './search-page.component.html',
  styleUrls: ['./search-page.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SearchPageComponent {
  private readonly router = inject(Router);

  searchTerm = new FormControl('', [Validators.required]);

  search(e: Event) {
    e.preventDefault();
    if (!this.searchTerm.value) {
      return;
    }

    const tags = this.searchTerm.value.toLowerCase()
      .replaceAll(/\s+/gm, ",");

    this.router.navigate(["/posts"], {
      queryParams: { tags }
    });
  }
}
