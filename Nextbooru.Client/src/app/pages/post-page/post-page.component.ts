import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input, computed, inject, signal } from '@angular/core';
import { BehaviorSubject, Observable, catchError, firstValueFrom, map, of, switchMap, tap } from 'rxjs';
import { ImageDto } from 'src/app/backend/backend-types';
import { ErrorService } from 'src/app/services/error.service';
import { ImagesService } from 'src/app/services/images.service';
import { SharedModule } from 'src/app/shared/shared.module';
import { BytesPipe } from "../../pipes/bytes.pipe";
import { VoteComponent } from "../../components/vote/vote.component";

interface ImageMetadataResult {
  data: ImageDtoExtended | null;
  error?: string | null;
  errorCode?: number | null;
}

interface ImageDtoExtended extends ImageDto {
  tagsMap: Map<string, string[]>;
  tagTypes: string[];
  userVoteStr: "upvote" | "downvote" | "none"
}

const userVoteMapping: { [key: number]: "upvote" | "downvote" | "none" } = {
  [1]: "upvote",
  [0]: "none",
  [-1]: "downvote"
}

@Component({
  standalone: true,
  selector: 'app-post-page',
  templateUrl: './post-page.component.html',
  styleUrls: ['./post-page.component.scss'],
  imports: [SharedModule, BytesPipe, VoteComponent],
})
export class PostPageComponent {
  private readonly errorService = inject(ErrorService);
  private readonly imagesService = inject(ImagesService);

  @Input()
  set id(v: string) {
    this._id = v;
    this.id$.next(v);
  }
  get id() {
    return this._id;
  }

  score = signal(0);
  userVote = signal<'upvote' | 'downvote' | 'none'>('none');
  processingVote = signal(false);

  isPublic = signal<boolean | null>(null);
  processingVisibilityChange = signal(false);
  visibility = computed(() => {
    if (this.isPublic() === null) {
      return '???';
    }
    return this.isPublic() ? 'public' : 'non public';
  });

  private _id!: string;
  private id$ = new BehaviorSubject<string>(this._id);

  public imageMetadata$: Observable<ImageMetadataResult> = this.id$.pipe(
    switchMap((id) => {
      const idNum = parseInt(id);
      if (!isFinite(idNum)) {
        return of({ data: null, error: 'Invalid ID format.' });
      }

      return this.imagesService.getImage(idNum).pipe(
        map((data) => ({ data: this.extendImageDto(data) })),
        catchError((err) => of(this.handleError(err)))
      );
    }),
    tap((x) => {
      // Maybe will later rewrite it to be fully declarative
      x.data && this.score.set(x.data.score);
      x.data && this.userVote.set(x.data.userVoteStr);
      x.data && this.isPublic.set(x.data.isPublic);
    })
  );

  async vote(id: number, vote: 'upvote' | 'downvote') {
    try {
      this.processingVote.set(true);
      const change = await firstValueFrom(this.imagesService.vote(id, vote));
      this.score.set(change.score);
      this.userVote.set(this.voteScoreToStr(change.voteScore));
    } catch (err) {
      // Error silently (for an user), for now.
      console.error('Vote error');
      console.error(err);
    } finally {
      this.processingVote.set(false);
    }
  }

  async makePublic(imageId: number) {
    try {
      this.processingVisibilityChange.set(true);
      await firstValueFrom(this.imagesService.makePublic(imageId));
      this.isPublic.set(true);
    }
    catch(err) {
      // TODO: Time to add toastrs :)
      console.error("Make public error");
      console.error(err);
    }
    finally {
      this.processingVisibilityChange.set(false);
    }
  }

  async makeNonPublic(imageId: number) {
    try {
      this.processingVisibilityChange.set(true);
      await firstValueFrom(this.imagesService.makeNonPublic(imageId));
      this.isPublic.set(false);
    }
    catch(err) {
      // TODO: Time to add toastrs :)
      console.error("Make non public error");
      console.error(err);
    }
    finally {
      this.processingVisibilityChange.set(false);
    }
  }

  private handleError(err: unknown): ImageMetadataResult {
    const errorMessage = this.errorService.errorToHuman(err);
    const errorCode = err instanceof HttpErrorResponse ? err.status : null;

    return { data: null, error: errorMessage, errorCode };
  }

  private extendImageDto(imageDto: ImageDto): ImageDtoExtended {
    const tagsMap = new Map<string, string[]>();

    imageDto.tags?.forEach((t) => {
      if (tagsMap.has(t.tagType)) {
        tagsMap.get(t.tagType)!.push(t.name);
      } else {
        tagsMap.set(t.tagType, [t.name]);
      }
    });

    return {
      ...imageDto,
      tagsMap: tagsMap,
      tagTypes: [...tagsMap.keys()],
      userVoteStr: this.voteScoreToStr(imageDto.userVote),
    };
  }

  private voteScoreToStr(voteScore: 1 | 0 | -1 | undefined | null) {
    return userVoteMapping[voteScore ?? 0] ?? 'none';
  }
}
