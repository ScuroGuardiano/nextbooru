import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ListResponse, ListTagsQuery, TagDto } from '../backend/backend-types';
import { BackendEndpoints } from '../backend/backend-enpoints';

@Injectable({
  providedIn: 'root'
})
export class TagsService {

  constructor(private httpClient: HttpClient) {}

  public autocomplete(phrase: string) {
    return this.httpClient.get<TagDto[]>(
      BackendEndpoints.tags.autocomplete,
      {
        params: { phrase }
      }
    );
  }

  public getOrderableFields() {
    return this.httpClient.get<string[]>(
      BackendEndpoints.tags.orderableFields
    );
  }

  public list(query: ListTagsQuery = {}) {
    return this.httpClient.get<ListResponse<TagDto>>(
      BackendEndpoints.tags.list,
      { params: query as any }
    );
  }
}
