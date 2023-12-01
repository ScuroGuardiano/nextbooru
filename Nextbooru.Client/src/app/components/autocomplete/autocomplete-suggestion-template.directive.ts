import { Directive, TemplateRef } from '@angular/core';

@Directive({
  selector: '[appAutocompleteSuggestion]',
  standalone: true
})
export class AutocompleteSuggestionDirective {

  constructor(public template: TemplateRef<unknown>) { }
}
