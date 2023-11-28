export interface IMediaList<T> {
  page: number;
  totalPages: number;
  totalRecords: number;
  data: T[];
}
