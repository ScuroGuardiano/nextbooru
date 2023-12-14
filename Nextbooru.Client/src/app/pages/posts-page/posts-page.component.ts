import { Component, Input, OnChanges, OnInit, SimpleChanges, WritableSignal, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Select, Store } from '@ngxs/store';
import { LoadPosts, PostsJumpToPage, PostsNextPage, PostsPreviousPage } from 'src/app/store/actions/posts.actions';
import { PostsState, PostsStateModel } from 'src/app/store/state/posts.state';
import { Observable, map } from 'rxjs';
import MediaGalleryElement from 'src/app/core/classes/media-gallery-element';
import { SimpleMediaGalleryComponent } from 'src/app/components/simple-media-gallery/simple-media-gallery.component';
import { IMediaList } from 'src/app/core/interfaces/i-media-list';
import { IMediaGalleryElement } from 'src/app/core/interfaces/i-media-gallery-element';

@Component({
  selector: 'app-posts-page',
  standalone: true,
  imports: [
    CommonModule,
    SimpleMediaGalleryComponent
  ],
  templateUrl: './posts-page.component.html',
  styleUrls: ['./posts-page.component.scss']
})
export class PostsPageComponent implements OnInit, OnChanges {
  constructor(private store: Store) {}

  private _tags: string = '';

  ngOnInit() {
    const action = this.tags === '' ?
      new LoadPosts() :
      new LoadPosts({ tags: this.tags });

    let page: number;
    if (this.page && isFinite(page = parseInt(this.page))) {
      if (action.query) {
        action.query.page = page;
      } else {
        action.query = { page };
      }
    }

    this.store.dispatch(action);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['page'] && !changes['page'].firstChange) {
      const pageNum = parseInt(changes['page'].currentValue);
      this.changePage(pageNum);
    }
  }

  @Input() set tags(v: string) {
    this._tags = v;
  }
  get tags() {
    return this._tags;
  }

  @Input() page?: string;

  @Select(PostsState) postsState$!: Observable<PostsStateModel>;
  mediaElements$: Observable<IMediaList<IMediaGalleryElement>> = this.postsState$.pipe(
    map(st => ({
      data: st.posts.map(MediaGalleryElement.fromMinimalImageDto),
      page: st.page!,
      totalPages: st.totalPages!,
      totalRecords: st.totalRecords!
    }))
  );

  private changePage(page: number) {
    if (!isFinite(page) || page < 1) {
      console.error(`Invalid page num`);
      return;
    }

    const currentPage = this.store.selectSnapshot<PostsStateModel>(PostsState).page;
    const pageDiff = page - currentPage;

    // TODO: this is so future optimization can work, if I won't implement it
    // Make it simpler and throw the fuck away those ifs

    if (pageDiff === 1) {
      return this.store.dispatch(new PostsNextPage());
    }
    if (pageDiff === -1) {
      return this.store.dispatch(new PostsPreviousPage());
    }

    return this.store.dispatch(new PostsJumpToPage(page));
  }
}
