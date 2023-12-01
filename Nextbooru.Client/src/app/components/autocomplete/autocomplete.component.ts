import { ChangeDetectionStrategy, ChangeDetectorRef, Component, ContentChild, EventEmitter, HostListener, Input, OnChanges, OnDestroy, OnInit, Output, Renderer2, SimpleChanges, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AutocompleteSuggestionDirective } from './autocomplete-suggestion-template.directive';

@Component({
  selector: 'app-autocomplete',
  standalone: true,
  imports: [CommonModule, AutocompleteSuggestionDirective],
  templateUrl: './autocomplete.component.html',
  styleUrls: ['./autocomplete.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AutocompleteComponent implements OnInit, OnDestroy, OnChanges {
  private readonly renderer = inject(Renderer2);
  // I need to manually detect changes when events from bounded element fires
  // Because if this element or any other element uses `ChangeDetectionStrategy.OnPush`
  // then changes are not detected after event from bounded element fires.
  // TODO: Probably I can fix that after Angular releases signal based components
  private readonly changeDetector = inject(ChangeDetectorRef);

  private listeners: (() => void)[] = []

  @Input({ required: true }) bindTo!: HTMLElement;
  @Input() data: any[] = [];
  @Output("select") selectEmitter = new EventEmitter<any>();
  @ContentChild(AutocompleteSuggestionDirective) suggestionTemplate?: AutocompleteSuggestionDirective;

  shouldBeVisibleOnData = false;
  visible = false;
  selected = '';
  selectedIdx = 0;

  ngOnInit(): void {
    this.listenOnBound();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes["data"]) {
      this.selected = changes["data"].currentValue[0] ?? '';
      this.selectedIdx = 0;
      if (changes["data"].currentValue.length > 0) {
        this.visible = this.shouldBeVisibleOnData;
      }
      else {
        this.visible = false;
      }
    }

    if (changes["bindTo"] && !changes["bindTo"].firstChange) {
      this.unlistenOnBound();
      this.listenOnBound();
    }
  }

  ngOnDestroy(): void {
    this.unlistenOnBound();
  }

  @HostListener('mousedown')
  private onMousedown(e: Event) {
    // So input won't get blurred
    e.preventDefault();
  }

  private onKeyDown(e: KeyboardEvent) {
    if (!this.visible) {
      return;
    }

    switch (e.code) {
      case "Tab":
      case "Enter":
        e.preventDefault();
        this.select(this.selected);
        this.changeDetector.detectChanges();
        break;

      case "ArrowDown":
        e.preventDefault();
        this.next();
        this.changeDetector.detectChanges();
        break;

      case "ArrowUp":
        e.preventDefault();
        this.prev();
        this.changeDetector.detectChanges();
        break;
    }
  }

  select(element: string) {
    this.selectEmitter.emit(element);
  }

  private next() {
    if (this.selectedIdx < (this.data.length - 1)) {
      this.selectedIdx++;
      this.selected = this.data[this.selectedIdx];
    }
  }

  private prev() {
    if (this.selectedIdx > 0) {
      this.selectedIdx--;
      this.selected = this.data[this.selectedIdx];
    }
  }

  private listenOnBound() {
    this.listeners.push(this.renderer.listen(this.bindTo, 'blur', e => {
      this.visible = false;
      this.shouldBeVisibleOnData = false;
      this.changeDetector.detectChanges();
    }));

    this.listeners.push(this.renderer.listen(this.bindTo, 'focus', e => {
      this.shouldBeVisibleOnData = true;
      if (this.data.length > 0) {
        this.visible = true;
      }
      this.changeDetector.detectChanges();
    }))

    this.listeners.push(this.renderer.listen(this.bindTo, 'keydown', this.onKeyDown.bind(this)))
  }
  private unlistenOnBound() {
    this.listeners.forEach(l => l());
    this.listeners = [];
  }
}
