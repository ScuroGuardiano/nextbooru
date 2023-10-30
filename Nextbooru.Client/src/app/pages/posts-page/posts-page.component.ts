import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Store } from '@ngxs/store';
import { LoadPosts } from 'src/app/store/actions/posts.actions';

@Component({
  selector: 'app-posts-page',
  standalone: true,
  imports: [CommonModule],
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
}
