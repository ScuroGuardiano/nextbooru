import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TagDto } from '../backend/backend-types';
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
}
