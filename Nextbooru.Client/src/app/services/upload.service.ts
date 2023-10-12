import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BackendEndpoints } from '../backend/backend-enpoints';

export interface IFileUploadPayload {
  file: File;
  tags: string;
  title?: string;
  source?: string;
}

@Injectable({
  providedIn: 'root'
})
export class UploadService {

  constructor(private http: HttpClient) {}

  public upload(fileData: IFileUploadPayload) {
    const formData = new FormData;
    formData.append("file", fileData.file, fileData.file.name);
    formData.append("tags", fileData.tags);
    fileData.title && formData.append("title", fileData.title);
    fileData.source && formData.append("source", fileData.source);

    return this.http.post(BackendEndpoints.upload, formData);
  }
}
