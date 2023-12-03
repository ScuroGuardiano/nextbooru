import { Component, ComponentRef, Directive, ElementRef, EventEmitter, HostBinding, Input, OnDestroy, OnInit, Optional, Output, Renderer2, ViewContainerRef } from '@angular/core';
import { AutocompleteComponent } from '../components/autocomplete/autocomplete.component';
import { TagsService } from '../services/tags.service';
import { NgControl } from '@angular/forms';
import { AutocompleteSuggestionDirective } from '../components/autocomplete/autocomplete-suggestion-template.directive';
import { SharedModule } from '../shared/shared.module';
import { Observable, Subscription, catchError, of, switchMap } from 'rxjs';
import { TagDto } from '../backend/backend-types';
import getCarretPosition from 'textarea-caret';

// TODO: Make it to position on it's bottom or top depending on free space.

// DONE! Also it would be could if in text-areas element would appear below current line of text
// instead of at the bottom. With higher textareas it looks terrible.
// I leave it here just to say it was terrible to find solution and to implemen that
// I went with old package `textarea-caret` that returns carret x and y position, but doesn't return
// carret height because some bug or something. Anyways it works well enough.

/**
 * It will add tags autocomplete into input. Input **must have** FormControlDirective on it.
 *
 * Autocomplete element will have 100% width of input element and will be positioned on bottom of this element.
 * If an input element is textarea then it will position below current cursor position.
 *
 * This is probably most cursed thing in this project but I really wanted to do it
 * with one directive, mainly for science, so here I am xD
 *
 * Usage:
 * ```html
 * <input type="search" [formControl]="searchTerm" appTagsAutocomplete>
 * ```
 * or
 * ```html
 * <textarea [formControl]="tags" appTagsAutocomplete></textarea>
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
    private tagsService: TagsService,
    private renderer: Renderer2
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
  private listeners: (() => void)[] = [];

  ngOnInit(): void {
    console.log(this.elementRef);

    // It has to here, not in the constructor, coz valueChanges are not available
    // if it's used in constructor :c
    this.autocompleteData$ = this.control?.valueChanges?.pipe(
      switchMap(input => {
        const phrase = this.getAutocompletePhrase(input);
        if (phrase) {
          return this.tagsService.autocomplete(phrase).pipe(
            catchError(err => {
              console.error("Autocompletion error");
              console.error(err);
              return of([]);
            })
          );
        }
        return of([]);
      })
    )!;

    const { offsetWidth, offsetHeight, offsetLeft, offsetTop } = this.elementRef.nativeElement as HTMLElement;

    this.component = this.viewContainerRef.createComponent(TagsAutocompleteComponent);
    this.component.setInput("bindTo", this.elementRef.nativeElement);
    this.component.setInput("data", this.autocompleteData$);

    if (this.elementRef.nativeElement instanceof HTMLTextAreaElement) {
      const textarea = this.elementRef.nativeElement;

      this.listeners.push(this.renderer.listen(textarea, 'click', () => this.handleTextareaCursorChange(textarea)));
      this.listeners.push(this.renderer.listen(textarea, 'keyup', () => this.handleTextareaCursorChange(textarea)));
    }

    // Note, this shit won't react to changes in size of parent element
    // But I don't care about that at least for now
    this.component.setInput("width", offsetWidth + "px");
    this.component.setInput("top", offsetTop + offsetHeight + "px");
    this.component.setInput("left", offsetLeft + "px");

    this.selectSubscription = this.component.instance.select.subscribe(suggestion => {
      this.applyAutocomplete(suggestion);
    });
  }

  ngOnDestroy(): void {
    this.component?.destroy();
    this.selectSubscription?.unsubscribe();
    this.listeners.forEach(l => l());
  }

  private applyAutocomplete(suggestion: TagDto) {
    const currentWords = this.control.value?.split(/\s+/g) ?? [];
    if (currentWords.length === 0) {
      return;
    }
    currentWords[currentWords.length - 1] = suggestion.name;
    this.control.control?.setValue(currentWords.join(" ") + " ");
  }

  private getAutocompletePhrase(input?: string | null) {
    if (!input) {
      return null;
    }

    return input.split(/\s+/g).pop();
  }

  private handleTextareaCursorChange(el: HTMLTextAreaElement)
  {
    const position = getCarretPosition(el, el.selectionEnd);
    const fontSize = parseFloat(document.defaultView?.getComputedStyle(el).fontSize ?? "");

    // I am adding 2px so it's better positioned I guess.
    // TODO: Try to fix this 'textarea-caret' so it would return 'height' properly.
    this.component?.setInput("top", el.offsetTop + position.top + fontSize + 2 + "px");
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
      top: var(--__tags-autocomplete-top);
      left: var(--__tags-autocomplete-left);
      width: var(--__tags-autocomplete-width);
      text-align: center;
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
  @HostBinding("style.--__tags-autocomplete-width")
  @Input() width: string = "0";

  @HostBinding("style.--__tags-autocomplete-left")
  @Input() left: string = "0";

  @HostBinding("style.--__tags-autocomplete-top")
  @Input() top: string = "0";

  @Input() bindTo!: HTMLElement;
  @Input() data?: Observable<TagDto[]>;
  @Output() select = new EventEmitter();
}
