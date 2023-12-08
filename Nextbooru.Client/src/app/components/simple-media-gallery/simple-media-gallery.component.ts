import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { IMediaGalleryElement } from 'src/app/core/interfaces/i-media-gallery-element';
import { IMediaList } from 'src/app/core/interfaces/i-media-list';
import { SharedModule } from 'src/app/shared/shared.module';
import { QueryParamsPaginatorComponent } from "../query-params-paginator/query-params-paginator.component";

@Component({
    selector: 'app-simple-media-gallery',
    standalone: true,
    templateUrl: './simple-media-gallery.component.html',
    styleUrls: ['./simple-media-gallery.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [SharedModule, QueryParamsPaginatorComponent]
})
export class SimpleMediaGalleryComponent {
  @Input({ required: true }) mediaList!: IMediaList<IMediaGalleryElement>;

  elementHoverText(element: IMediaGalleryElement) {
    return `${element.title ?? "no-title"} | ${element.tags ?? "no-tags"}`;
  }

  mediaListTrackBy(index: number, el: IMediaGalleryElement) {
    return el.id;
  }
}
