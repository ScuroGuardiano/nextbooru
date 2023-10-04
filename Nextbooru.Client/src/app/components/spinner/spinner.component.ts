import { Component, HostBinding, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-spinner',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './spinner.component.html',
  styleUrls: ['./spinner.component.scss']
})
export class SpinnerComponent {
  @HostBinding('class') class = "";
  @HostBinding('style') style = "";

  @Input() set size(v: "large" | "normal" | "small" | undefined) {
    this.class = v ?? "";
  };

  @Input() set color(v: string | undefined) {
    v ? this.style = `--spinner-color: ${v}` : this.style = "";
  }
}
