import { Action, StateContext, StateToken } from "@ngxs/store";
import { ResettableState } from "../resettable-state/resettable-state";
import { Injectable } from "@angular/core";
import { LoadPosts, PostsJumpToPage, PostsNextPage, PostsPreviousPage } from "../actions/posts.actions";
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
        // We want to remember this setting
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
    );
  }


  // TODO: [Performance Issue] - get rid of `ctx.patchState` here, make it differently
  /*
    Dobra kurwa, napiszę po polsku bo mnie chuj jebie.
    W momencie kiedy robię `ctx.patchState` emitowany jest nowy state
    A z nim każda wartość bazująca na nim albo go transformująca
    Zatem, jeżeli gdzieś używam `pipe` i np. operatora `map` do
    zmapowania wartości na inny format to się wykona to ponownie.
    Może nie jest to wielką katastrofą, bo komputery są szybkie,
    ale nie lubię wykonywać niepotrzebnej pracy obliczeniowej, więc do fixa
  */
  // FIXME:
  // BLYAT
  @Action(PostsNextPage)
  private nextPage(ctx: Context) {
    ctx.patchState({
      page: ctx.getState().page + 1
    });

    return ctx.dispatch(new LoadPosts());
  }

  @Action(PostsPreviousPage)
  private previousPage(ctx: Context) {
    const current = ctx.getState().page;
    if (current === 1) {
      return;
    }

    ctx.patchState({
      page: current - 1
    });

    return ctx.dispatch(new LoadPosts());
  }

  @Action(PostsJumpToPage)
  private jumpToPage(ctx: Context, action: PostsJumpToPage) {
    ctx.patchState({
      page: action.page
    });

    return ctx.dispatch(new LoadPosts());
  }
}
