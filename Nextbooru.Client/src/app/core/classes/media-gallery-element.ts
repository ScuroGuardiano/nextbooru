import { ImageDto } from "src/app/backend/backend-types";
import { IMediaGalleryElement } from "../interfaces/i-media-gallery-element";
import { MediaMode } from "../enums/media-mode.enum";

export interface MediaGalleryElementInit {
  title?: string;
  tags?: string;
  width: number;
  height: number;
  isPublic: boolean;
  imagePageUrl: string;
  imageUrl: string;
}

export default class MediaGalleryElement implements IMediaGalleryElement {
  constructor(init: MediaGalleryElementInit) {
    this.title = init.title;
    this.tags = init.tags;
    this.width = init.width;
    this.height = init.height;
    this.isPublic = init.isPublic;
    this.imagePageUrl = init.imagePageUrl;
    this.imageUrl = init.imageUrl;
  }

  static fromImageDto(imageDto: ImageDto) {
    return new MediaGalleryElement({
      title: imageDto.title,
      tags: imageDto.tags?.map(t => t.name).join(" "),
      width: imageDto.width,
      height: imageDto.height,
      isPublic: imageDto.isPublic,
      imagePageUrl: `/posts/${imageDto.id}`,
      imageUrl: imageDto.url
    });
  }

  title?: string;
  tags?: string;
  width: number;
  height: number;
  isPublic: boolean;
  imagePageUrl: string;
  getImageUrl(width: number, mode?: MediaMode): string {
    const modeParam = mode ? `&mode=${mode}` : "";
    return `${this.imageUrl}?w=${width}${modeParam}`;
  }

  private imageUrl: string;
}
