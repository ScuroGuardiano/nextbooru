import { Component, ComponentRef, Directive, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Optional, Output, ViewContainerRef } from '@angular/core';
import { AutocompleteComponent } from '../components/autocomplete/autocomplete.component';
import { TagsService } from '../services/tags.service';
import { NgControl } from '@angular/forms';
import { AutocompleteSuggestionDirective } from '../components/autocomplete/autocomplete-suggestion-template.directive';
import { SharedModule } from '../shared/shared.module';
import { Observable, Subscription, catchError, of, switchMap } from 'rxjs';
import { TagDto } from '../backend/backend-types';

// TODO: Make it to take 100% width of input element and position on it's bottom or top
// depending on free space. In other words make this work without wrapping input in a container.

// TODO: Also it would be could if in text-areas element would appear below current line of text
// instead of at the bottom. With higher textareas it looks terrible.

/**
 * It will add tags autocomplete into input. Input **must have** FormControlDirective on it.
 *
 * Autocomplete element takes 100% width of it's parent, display itself on bottom
 * and has absolute positioning. So you have to wrap input you're attaching to
 * in a container with relative positioning and there you can control width or it.
 *
 * This is probably most cursed thing in this project but I really wanted to do it
 * with one directive, mainly for science, so here I am xD
 *
 * Usage:
 * ```html
 * <div>
 *   <input type="search" [formControl]="searchTerm" appTagsAutocomplete>
 * </div>
 * ```
 */
@Directive({
  selector: '[appTagsAutocomplete]',
  standalone: true
})
export class TagsAutocompleteDirective implements OnInit, OnDestroy {
  constructor(
    private elementRef: ElementRef,
    private viewContainerRef: ViewContainerRef,
    @Optional() private control: NgControl,
    private tagsService: TagsService
  ) {
    // I added this check only to know that if idk I will come back in this project after 1 year
    // and use this directive on normal input without formControl then I will have clean error message
    // Not some Angular messy error
    if (!this.control) {
      throw new Error("FormControl is null! TagsAutocompleteDirective can be only used on inputs with formControl directive.")
    }
  }

  autocompleteData$!: Observable<TagDto[]>;

  private component?: ComponentRef<TagsAutocompleteComponent>;
  private selectSubscription?: Subscription;

  ngOnInit(): void {
    // It has to here, not in the constructor, coz valueChanges are not available
    // if it's used in constructor :c
    this.autocompleteData$ = this.control?.valueChanges?.pipe(
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
    )!;

    this.component = this.viewContainerRef.createComponent(TagsAutocompleteComponent);
    this.component.setInput("bindTo", this.elementRef.nativeElement);
    this.component.setInput("data", this.autocompleteData$);
    this.selectSubscription = this.component.instance.select.subscribe(suggestion => {
      this.applyAutocomplete(suggestion);
    })
  }

  ngOnDestroy(): void {
    this.component?.destroy();
    this.selectSubscription?.unsubscribe();
  }

  private applyAutocomplete(suggestion: TagDto) {
    const currentWords = this.control.value?.split(" ");
    if (!currentWords?.length || currentWords.length === 0) {
      return;
    }
    currentWords[currentWords.length - 1] = suggestion.name;
    this.control.control?.setValue(currentWords.join(" ") + " ");
  }


  private getAutocompletePhrase(input: string | undefined | null) {
    if (input == null || input === "") {
      return null;
    }

    return input.split(" ").at(-1);
  }
}

@Component({
  selector: "app-tags-autocomplete-component",
  template: `
    <app-autocomplete
      [bindTo]="bindTo"
      [data]="(data | async) ?? []"
      (select)="select.emit($event)">
      <ng-template appAutocompleteSuggestion let-suggestion>
        <span class="tag-type-{{ suggestion.tagType }}">{{ suggestion.name }} ({{ suggestion.count }})</span>
      </ng-template>
    </app-autocomplete>`,
  styles: [`
    :host {
      display: block;
      margin-top: 5px;
      position: absolute;
      width: 100%;
      z-index: 100;
    }
    app-autocomplete {
      width: 100%;
    }`
  ],
  standalone: true,
  imports: [
    SharedModule,
    AutocompleteComponent,
    AutocompleteSuggestionDirective
  ]
})
class TagsAutocompleteComponent {
  @Input() bindTo!: HTMLElement;
  @Input() data?: Observable<TagDto[]>;
  @Output() select = new EventEmitter();
}
