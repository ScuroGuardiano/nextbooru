import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ImageDto, ListImagesQuery, ListResponse } from '../backend/backend-types';
import { BackendEndpoints } from '../backend/backend-enpoints';

@Injectable({
  providedIn: 'root'
})
export class ImagesService {

  constructor(private httpClient: HttpClient) { }

  public listImages(query: ListImagesQuery) {
    let params = new HttpParams();
    if (query.page != null) params = params.set('page', query.page);
    if (query.resultsOnPage != null) params = params.set('resultsOnPage', query.resultsOnPage);
    if (query.tags != null) params = params.set('tags', query.tags);

    return this.httpClient.get<ListResponse<ImageDto>>(BackendEndpoints.images.list, {
      params
    });
  }

  public getImage(id: number) {
    return this.httpClient.get<ImageDto>(BackendEndpoints.images.get(id));
  }
}
