import { Component, HostBinding, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IMediaGalleryElement } from 'src/app/core/interfaces/i-media-gallery-element';
import { SharedModule } from 'src/app/shared/shared.module';

@Component({
  selector: 'app-simple-media-gallery',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './simple-media-gallery.component.html',
  styleUrls: ['./simple-media-gallery.component.scss']
})
export class SimpleMediaGalleryComponent {
  @Input({ required: true }) mediaList!: IMediaGalleryElement[];

  elementHoverText(element: IMediaGalleryElement) {
    return `${element.title ?? "no-title"} | ${element.tags ?? "no-tags"}`;
  }
}
