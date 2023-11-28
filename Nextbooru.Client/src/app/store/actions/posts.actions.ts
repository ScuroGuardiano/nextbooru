import { ListImagesQuery } from "src/app/backend/backend-types";

export class LoadPosts {
  static readonly type = '[Posts] Load posts';

  constructor();
  constructor(query: ListImagesQuery);
  constructor(public query?: ListImagesQuery) {}
}

export class PostsJumpToPage {
  static readonly type = '[Posts] Jump to page'
  constructor(public page: number) {}
}

/**
 * ***IN THE FUTURE*** for this action keyed-based pagination will be used ***when possible***, so it's very fast
 */
export class PostsNextPage {
  static readonly type = '[Posts] Next page'
}

/**
 * ***IN THE FUTURE*** for this action keyed-based pagination will be used ***when possible***, so it's very fast
 */
export class PostsPreviousPage {
  static readonly type = '[Posts] Previous page'
}

export class PostsLastPage {
  static readonly type = '[Posts] Last page'
}
