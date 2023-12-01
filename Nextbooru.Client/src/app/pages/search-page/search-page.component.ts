import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { catchError, debounceTime, of, switchMap } from 'rxjs';
import { TagDto } from 'src/app/backend/backend-types';
import { AutocompleteSuggestionDirective } from 'src/app/components/autocomplete/autocomplete-suggestion-template.directive';
import { AutocompleteComponent } from 'src/app/components/autocomplete/autocomplete.component';
import { SpinnerOverlayComponent } from 'src/app/components/spinner-overlay/spinner-overlay.component';
import { TagsService } from 'src/app/services/tags.service';
import { SharedModule } from 'src/app/shared/shared.module';

@Component({
  selector: 'app-search-page',
  standalone: true,
  imports: [
    SharedModule,
    SpinnerOverlayComponent,
    AutocompleteComponent,
    AutocompleteSuggestionDirective
  ],
  templateUrl: './search-page.component.html',
  styleUrls: ['./search-page.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SearchPageComponent {
  private readonly router = inject(Router);
  private readonly tagsService = inject(TagsService);

  searchTerm = new FormControl('', [Validators.required]);

  autocompleteData$ = this.searchTerm.valueChanges.pipe(
    switchMap(input => {
      const phrase = this.getAutocompletePhrase(input);
      if (phrase) {
        return this.tagsService.autocomplete(phrase);
      }
      return of([]);
    }),
    catchError(err => {
      console.error("Autocompletion error");
      console.error(err);
      return of([]);
    })
  )

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

  applyAutocomplete(suggestion: TagDto) {
    const currentWords = this.searchTerm.value?.split(" ");
    if (!currentWords?.length || currentWords.length === 0) {
      return;
    }
    currentWords[currentWords.length - 1] = suggestion.name;
    this.searchTerm.setValue(currentWords.join(" ") + " ");
  }

  private getAutocompletePhrase(input: string | undefined | null) {
    if (input == null || input === "") {
      return null;
    }

    return input.split(" ").at(-1);
  }
}
