import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Select, Store } from '@ngxs/store';
import { LoadPosts } from 'src/app/store/actions/posts.actions';
import { PostsState, PostsStateModel } from 'src/app/store/state/posts.state';
import { Observable, map } from 'rxjs';
import MediaGalleryElement from 'src/app/core/classes/media-gallery-element';
import { SimpleMediaGalleryComponent } from 'src/app/components/simple-media-gallery/simple-media-gallery.component';

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
export class PostsPageComponent implements OnInit {
  constructor(private store: Store) {}

  private _tags: string = '';

  ngOnInit() {
    const action = this.tags === '' ?
      new LoadPosts() :
      new LoadPosts({ tags: this.tags });

    this.store.dispatch(action);
  }

  @Input() set tags(v: string) {
    this._tags = v;
  }
  get tags() {
    return this._tags;
  }

  @Select(PostsState) postsState$!: Observable<PostsStateModel>;
  mediaElements$ = this.postsState$.pipe(
    map(st => st.posts.map(MediaGalleryElement.fromImageDto))
  )
}
