import { MediaMode } from "../enums/media-mode.enum";

export interface IMediaGalleryElement {
  id: number;
  title?: string;
  tags?: string;
  width: number;
  height: number;
  isPublic: boolean;
  imagePageUrl: string;
  getImageUrl(width: number, mode?: MediaMode): string;
}
