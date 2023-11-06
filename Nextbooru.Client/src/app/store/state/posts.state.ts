import { Action, StateContext, StateToken } from "@ngxs/store";
import { ResettableState } from "../resettable-state/resettable-state";
import { Injectable } from "@angular/core";
import { LoadPosts } from "../actions/posts.actions";
import { ImagesService } from "src/app/services/images.service";
import { tap } from "rxjs";
import { ImageDto } from "src/app/backend/backend-types";

export const POSTS_STATE_TOKEN = new StateToken<PostsStateModel>('posts');

export interface PostsStateModel {
  posts: ImageDto[];
  tags?: string;
  page: number;
  totalPages?: number;
  totalRecords?: number;
  postsOnPage?: number;
  lastRecordId?: number;
}

type Context = StateContext<PostsStateModel>;

@ResettableState<PostsStateModel>({
  name: POSTS_STATE_TOKEN,
  defaults: {
    posts: [],
    page: 1
  }
})
@Injectable()
export class PostsState {
  constructor(private imagesService: ImagesService) {}

  @Action(LoadPosts, { cancelUncompleted: true })
  private loadPosts(ctx: Context, action: LoadPosts) {
    if (action.query) {
      ctx.patchState({
        page: action.query.page,
        // We want to actually remember this setting
        postsOnPage: action.query.resultsOnPage ?? ctx.getState().postsOnPage,
        tags: action.query.tags
      });
    }

    const { page, tags, postsOnPage } = ctx.getState();

    return this.imagesService.listImages({
      page, tags, resultsOnPage: postsOnPage
    }).pipe(
      tap(res => {
        ctx.patchState({
          posts: res.data,
          page: res.page || page,   // With key-based pagination page will be set on 0 here.
          postsOnPage: res.recordsPerPage,
          totalPages: res.totalPages,
          totalRecords: res.totalRecords,
          lastRecordId: res.lastRecordId
        });
      })
    )
  }
}
