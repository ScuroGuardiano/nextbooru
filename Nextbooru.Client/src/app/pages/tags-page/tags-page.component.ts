import { ChangeDetectionStrategy, Component, Input, OnChanges, OnDestroy, SimpleChanges, inject, signal } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable, Subscription, catchError, map, of, tap } from 'rxjs';
import { ListResponse, ListTagsQuery, TagDto } from 'src/app/backend/backend-types';
import { ErrorService } from 'src/app/services/error.service';
import { TagsService } from 'src/app/services/tags.service';
import { SharedModule } from 'src/app/shared/shared.module';
import { TagsTableComponent } from "../../components/tags-table/tags-table.component";
import { QueryParamsPaginatorComponent } from "../../components/query-params-paginator/query-params-paginator.component";

@Component({
    selector: 'app-tags-page',
    standalone: true,
    templateUrl: './tags-page.component.html',
    styleUrl: './tags-page.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        SharedModule,
        TagsTableComponent,
        QueryParamsPaginatorComponent
    ]
})
export class TagsPageComponent implements OnChanges, OnDestroy {
  private readonly tagsService = inject(TagsService);
  private readonly errorService = inject(ErrorService);
  private readonly router = inject(Router);

  ngOnChanges(changes: SimpleChanges): void {
    const query: ListTagsQuery = this.inputsToListTagsQuery();

    this.form.patchValue({
      name: query.name,
      onPage: query.resultsOnPage?.toString() ?? this.defaultQuery.resultsOnPage.toString(),
      orderBy: query.orderBy,
      orderDirection: query.orderDirection ?? this.defaultQuery.orderDirection
    });

    this.loadTags(query);
  }

  ngOnDestroy(): void {
    this.tagsSubscription?.unsubscribe();
  }

  @Input() page?: string = "1";
  @Input() name?: string;
  @Input() orderBy?: string;
  @Input() orderDirection?: string = "asc";
  @Input() onPage?: string = "50";

  private readonly orderableFieldsDisplays = {
    "TagType": "Tag type",
    "ImagesCount": "Images count",
    "CreatedAt": "Created at",
    "UpdatedAt": "Updated at"
  } as { [key: string]: string };

  private readonly defaultQuery = {
    page: 1,
    resultsOnPage: 50,
    orderDirection: "asc"
  } as const;

  // TODO: move it to URL
  form = new FormGroup({
    name: new FormControl<string>(""),
    orderBy: new FormControl<string | null>(null),
    orderDirection: new FormControl<string>(this.defaultQuery.orderDirection),
    onPage: new FormControl<string>(this.defaultQuery.resultsOnPage.toString())
  });

  orderableFields$ = this.tagsService.getOrderableFields().pipe(
      map(v => {
        return v.map(x => ({ key: x, display: this.orderableFieldsDisplays[x] ?? x }))
      }),
      tap(v => {
        if (!v.some(x => x.key === this.form.value.orderBy)) {
          this.form.patchValue({ orderBy: v[0]?.key })
        }
      })
  );

  loadingTags = signal(false);
  tagsError = signal<string | undefined>(undefined);
  tagsListResponse = signal<ListResponse<TagDto> | undefined>(undefined);

  private tagsSubscription?: Subscription;

  public onSubmit(event: Event) {
    event.preventDefault();
    const params = { ...this.form.value };
    if (!params.name) {
      params.name = null; // I don't wan empty string in query.
    }

    this.router.navigate([], {
      queryParams: params,
      queryParamsHandling: 'merge'
    });
  }

  private inputsToListTagsQuery(): ListTagsQuery {
    const query: ListTagsQuery = {};

    const parsedPage = parseInt(this.page!);
    if (isFinite(parsedPage) && parsedPage > 0) {
      query.page = parsedPage;
    }

    const parsedOnPage = parseInt(this.onPage!);
    if (isFinite(parsedOnPage) && parsedOnPage > 0) {
      query.resultsOnPage = parsedOnPage;
    }

    if (this.name) {
      query.name = this.name;
    }

    // Yeah, I could turn orderableFields$ to promise, wait for it
    // and then check if it's valid. But it would make page to load
    // longer, so nope. Server has it's own validation.
    if (this.orderBy) {
      query.orderBy = this.orderBy;
    }

    if (this.orderDirection && ["asc", "desc"].includes(this.orderDirection.toLowerCase())) {
      query.orderDirection = this.orderDirection
    }

    return { ...this.defaultQuery, ...query };
  }

  // For future me:
  // I really tried to do it fully declarative but I am clueless
  // Basically I want to refresh table with data only when new Observable emits value.
  // When I was changing observable then `tags$ | async` in template was null and
  // my condition stopped displaying the table.
  // So fuck this, I made it with the power of ***IMPERATIVE*** paradigm!
  private loadTags(query: ListTagsQuery) {
    // Cancel pending request if any
    this.tagsSubscription?.unsubscribe();

    this.loadingTags.set(true);

    type TagsResult = { error?: string, response?: ListResponse<TagDto> };

    const tags$: Observable<TagsResult> = this.tagsService.list(query).pipe(
      map(v => ({ response: v })),
      catchError(err => {
        console.error("Error while fetching tags");
        console.error(err);
        return of({ error: this.errorService.errorToHuman(err) });
      }),
      tap(() => this.loadingTags.set(false))
    );

    this.tagsSubscription = tags$.subscribe(tags => {
      if (tags.error) {
        this.tagsError.set(tags.error);
        return this.tagsListResponse.set(undefined);
      }

      this.tagsError.set(undefined);
      this.tagsListResponse.set(tags.response);
    });
  }
}
