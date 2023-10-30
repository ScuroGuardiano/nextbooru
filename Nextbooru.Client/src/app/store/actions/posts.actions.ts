import { ListImagesQuery } from "src/app/backend/backend-types";

export class LoadPosts {
  static readonly type = '[Posts] Load Posts';

  constructor();
  constructor(query: ListImagesQuery);
  constructor(public query?: ListImagesQuery) {}
}

export class GoToPostsPage {
  static readonly type = '[Posts] Go To Posts Page'
  constructor(public page: number) {}
}

/**
 * For this action keyed-based pagination will be used, so it's very fast
 */
export class GoToPostsNextPage {
  static readonly type = '[Posts] Go To Posts Next Page'
}

/**
 * For this action key-based pagination ***may be*** used, so it's very fast.
 */
export class GoToPostsPreviousPage {
  static readonly type = '[Posts] Go To Posts Previous Page'
}

export class GoToPostsLastPage {
  static readonly type = '[Posts] Go To Posts Last Page'
}
