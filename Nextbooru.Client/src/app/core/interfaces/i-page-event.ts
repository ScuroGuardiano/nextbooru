import { PageEventType } from "../enums/page-event-type";

export interface IPageEvent {
  type: PageEventType;
  page: number;
}
