import { Component, Input, OnChanges, OnInit, SimpleChanges, inject } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Observable, catchError, map, of, tap } from 'rxjs';
import { ListTagsQuery, TagDto } from 'src/app/backend/backend-types';
import { ErrorService } from 'src/app/services/error.service';
import { TagsService } from 'src/app/services/tags.service';
import { SharedModule } from 'src/app/shared/shared.module';

@Component({
  selector: 'app-tags-page',
  standalone: true,
  imports: [
    SharedModule
  ],
  templateUrl: './tags-page.component.html',
  styleUrl: './tags-page.component.scss'
})
export class TagsPageComponent implements OnInit, OnChanges {
  private readonly tagsService = inject(TagsService);
  private readonly errorService = inject(ErrorService);

  ngOnInit(): void {
    const query: ListTagsQuery = {};

    let page: number;
    if (this.page && isFinite(page = parseInt(this.page))) {
      query.page = page;
    }

    this.tags$ = this.tagsService.list(query).pipe(
      map(v => ({ data: v.data })),
      catchError(err => {
        console.error("Error while fetching tags");
        console.error(err);
        return of({ error: this.errorService.errorToHuman(err) });
      })
    );

  }
  ngOnChanges(changes: SimpleChanges): void {

  }

  @Input() page?: string = "1";

  // TODO: move it to URL
  form = new FormGroup({
    name: new FormControl<string>(""),
    orderBy: new FormControl<string | null>(null),
    orderDirection: new FormControl<string>("asc"),
    onPage: new FormControl<string>("20")
  });

  orderableFields$ = this.tagsService.getOrderableFields().pipe(
      map(v => {
        return v.map(x => ({ key: x, display: this.orderableFieldsDisplays[x] ?? x }))
      }),
      tap(v => {
        this.form.patchValue({ orderBy: v[0]?.key })
      })
  );

  tags$?: Observable<{
    error?: string,
    data?: TagDto[]
  }>;

  public onSubmit(event: Event) {
    event.preventDefault();
  }

  private readonly orderableFieldsDisplays = {
    "TagType": "Tag type",
    "ImagesCount": "Images count",
    "CreatedAt": "Created at",
    "UpdatedAt": "Updated at"
  } as { [key: string]: string };

  private changePage(page: number) {
    if (!isFinite(page) || page < 1) {
      console.error(`Invalid page num`);
      return;
    }

    throw new Error("Shit is not implemented");
  }
}
