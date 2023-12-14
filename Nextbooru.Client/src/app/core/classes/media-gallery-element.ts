import { ImageDto, MinimalImageDto } from "src/app/backend/backend-types";
import { IMediaGalleryElement } from "../interfaces/i-media-gallery-element";
import { MEDIA_MODE, MediaMode } from "../enums/media-mode.enum";

export interface MediaGalleryElementInit {
  id: number;
  title?: string;
  tags?: string;
  width: number;
  height: number;
  isPublic: boolean;
  imagePageUrl: string;
  imageUrl: string;
  thumbnailUrl: string;
}

export default class MediaGalleryElement implements IMediaGalleryElement {
  constructor(init: MediaGalleryElementInit) {
    this.id = init.id;
    this.title = init.title;
    this.tags = init.tags;
    this.width = init.width;
    this.height = init.height;
    this.isPublic = init.isPublic;
    this.imagePageUrl = init.imagePageUrl;
    this.imageUrl = init.imageUrl;
    this.thumbnailUrl = init.thumbnailUrl;
  }

  static fromMinimalImageDto(imageDto: MinimalImageDto) {
    return new MediaGalleryElement({
      id: imageDto.id,
      title: imageDto.title,
      tags: imageDto.tags.join(" "),
      width: imageDto.width,
      height: imageDto.height,
      isPublic: imageDto.isPublic,
      imagePageUrl: `/posts/${imageDto.id}`,
      imageUrl: imageDto.url,
      thumbnailUrl: imageDto.thumbnailUrl
    });
  }

  id: number;
  title?: string;
  tags?: string;
  width: number;
  height: number;
  isPublic: boolean;
  imagePageUrl: string;
  getImageUrl(width: number, mode?: MediaMode): string {
    const modeParam = mode ? `&mode=${mode}` : "";
    if (mode === MEDIA_MODE.THUMBNAIL) {
      return `${this.thumbnailUrl}?w=${width}${modeParam}`;
    }
    return `${this.imageUrl}?w=${width}${modeParam}`;
  }

  private imageUrl: string;
  private thumbnailUrl: string;
}
