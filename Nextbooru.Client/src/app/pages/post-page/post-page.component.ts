import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input, inject } from '@angular/core';
import { BehaviorSubject, Observable, catchError, map, of, switchMap } from 'rxjs';
import { ImageDto } from 'src/app/backend/backend-types';
import { ErrorService } from 'src/app/services/error.service';
import { ImagesService } from 'src/app/services/images.service';
import { SharedModule } from 'src/app/shared/shared.module';
import { BytesPipe } from "../../pipes/bytes.pipe";

interface ImageMetadataResult {
  data: ImageDtoWithTagsMap | null;
  error?: string | null;
  errorCode?: number | null;
}

interface ImageDtoWithTagsMap extends ImageDto {
  tagsMap: Map<string, string[]>,
  tagTypes: string[];
}

@Component({
    standalone: true,
    selector: 'app-post-page',
    templateUrl: './post-page.component.html',
    styleUrls: ['./post-page.component.scss'],
    imports: [SharedModule, BytesPipe]
})
export class PostPageComponent {
  private readonly errorService = inject(ErrorService);
  private readonly imagesService = inject(ImagesService);

  @Input()
  set id(v: string) {
    this._id = v;
    this.id$.next(v);
  };
  get id() {
    return this._id;
  }

  private _id!: string;
  private id$ = new BehaviorSubject<string>(this._id);

  public imageMetadata$: Observable<ImageMetadataResult> = this.id$.pipe(
    switchMap(id => {
      const idNum = parseInt(id);
      if (!isFinite(idNum)) {
        return of({ data: null, error: "Invalid ID format." })
      }

      return this.imagesService.getImage(idNum).pipe(
        map(data => ({ data: this.imageDtoWithTagsMap(data) })),
        catchError(err => of(this.handleError(err)))
      )
    })
  );

  private handleError(err: unknown): ImageMetadataResult {
    const errorMessage = this.errorService.errorToHuman(err);
    const errorCode = err instanceof HttpErrorResponse ? err.status : null;

    return { data: null, error: errorMessage, errorCode };
  }

  private imageDtoWithTagsMap(imageDto: ImageDto): ImageDtoWithTagsMap {
    const tagsMap = new Map<string, string[]>()

    imageDto.tags?.forEach(t => {
      if (tagsMap.has(t.tagType)) {
        tagsMap.get(t.tagType)!.push(t.name);
      } else {
        tagsMap.set(t.tagType, [ t.name ]);
      }
    });

    return {
      ...imageDto,
      tagsMap: tagsMap,
      tagTypes: [...tagsMap.keys()]
    }
  }
}
