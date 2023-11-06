import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-flex-media-gallery',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './flex-media-gallery.component.html',
  styleUrls: ['./flex-media-gallery.component.scss']
})
export class FlexMediaGalleryComponent {

  /**
   * Horizontal gap between elements in pixels.
   */
  @Input() horizontalGap: number = 10;

  /**
   * Verital gap between elements in pixels
   */
  @Input() verticalGap: number = 10;

  /**
   * Minimal row height in pixels
   */
  @Input() minRowHeight: number = 250;
}
