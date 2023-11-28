import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { IMediaGalleryElement } from 'src/app/core/interfaces/i-media-gallery-element';
import { IMediaList } from 'src/app/core/interfaces/i-media-list';
import { SharedModule } from 'src/app/shared/shared.module';

@Component({
  selector: 'app-simple-media-gallery',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './simple-media-gallery.component.html',
  styleUrls: ['./simple-media-gallery.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SimpleMediaGalleryComponent {
  @Input({ required: true }) mediaList!: IMediaList<IMediaGalleryElement>;

  elementHoverText(element: IMediaGalleryElement) {
    return `${element.title ?? "no-title"} | ${element.tags ?? "no-tags"}`;
  }

  private readonly totalDisplayedPages = 9;

  mediaListTrackBy(index: number, el: IMediaGalleryElement) {
    return el.id;
  }

  getDisplayedPages(): number[] {
    const displayedPages = Math.min(this.totalDisplayedPages, this.mediaList.totalPages);
    const halfDisplayedPages = Math.floor(this.totalDisplayedPages / 2);

    const pages = [];

    let pagesLeft =
      this.mediaList.page > halfDisplayedPages
      ? halfDisplayedPages
      : this.mediaList.page - 1;

    if ((this.mediaList.page + halfDisplayedPages) > this.mediaList.totalPages) {
      pagesLeft = Math.min(
        displayedPages - 1,
        pagesLeft + Math.abs(this.mediaList.totalPages - (this.mediaList.page + halfDisplayedPages))
      );
    }

    const startPage = this.mediaList.page - pagesLeft;

    for (let i = 0; i < displayedPages; i++) {
      pages.push(startPage + i);
    }
    return pages;
  }
}
