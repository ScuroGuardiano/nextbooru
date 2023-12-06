import { HttpParams } from "@angular/common/http";

export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  password: string;
  email?: string;
}

export interface SessionResponse {
  user:       SessionUser;
  loggedInIP: string;
  lastIP:     string;
  userAgent:  string;
  createdAt:  string;
  lastAccess: string;
}

export interface SessionUser {
  username:    string;
  displayName: string;
}

export interface ApiErrorResponse {
  type: "ApiErrorResponse",
  statusCode: number;
  message?: string;
  errorClrType?: string;
  errorCode?: string;
}

export interface BasicUserInfo {
  username: string;
  displayName: string;
  isAdmin: boolean;
}

export interface ListImagesQuery {
  page?: number;
  resultsOnPage?: number;
  tags?: string;
}

export interface ListTagsQuery {
  page?: number;
  resultsOnPage?: number;
  name?: string;
  sortOrder?: string;
  orderBy?: string;
}

export interface TagDto {
  name: string;
  tagType: string;
  count: number;
}

export interface ImageDto {
  id: number;
  url: string;
  thumbnailUrl: string;
  title?: string;
  source?: string;
  contentType?: string;
  extension?: string;
  width: number;
  height: number;
  sizeInBytes: number;
  isPublic: boolean;
  tags?: TagDto[];
  uploadedBy?: BasicUserInfo;
}

export interface ListResponse<T> {
  data: T[];
  page: number;
  totalPages: number;
  totalRecords: number;
  recordsPerPage: number;
  lastRecordId: number;
}
