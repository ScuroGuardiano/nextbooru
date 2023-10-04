import { Component, HostBinding, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SpinnerComponent } from '../spinner/spinner.component';

@Component({
  selector: 'app-spinner-overlay',
  standalone: true,
  imports: [CommonModule, SpinnerComponent],
  templateUrl: './spinner-overlay.component.html',
  styleUrls: ['./spinner-overlay.component.scss']
})
export class SpinnerOverlayComponent {
  @HostBinding("class.fullscreen") _fullscreen = false;

  @HostBinding("class.enabled")
  @Input() enabled: boolean = false;

  @Input() set fullscreen(v: string | boolean | undefined) {
    if (v) {
      this._fullscreen = true;
    }
    else if (typeof v === 'string') {
      this.fullscreen = true;
    }
    else {
      this.fullscreen = false;
    }
  }
  @Input() size: "large" | "normal" | "small" | undefined;
  @Input() color: string | undefined;
}
