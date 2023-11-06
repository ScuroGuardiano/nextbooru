// Typescript enums aren't very cool - https://www.youtube.com/watch?v=jjMbPt_H3RQ

/**
 * Media mode provides information to The Server for media purposes, allowing it to
 * determine how to optimize the image. The Server has the power to decide how the image
 * for It is omniscient and omnipotent!
 */
export const MEDIA_MODE = {
  /**
   * Thumbnail mode - trade quality for small size, server ***should*** return compressed jpeg, webp or gif.
   */
  THUMBNAIL: "thumbnail",

  /**
   * Preview mode - server ***should*** return compressed jpeg, webp or gif ***possibly*** with little bit better quality than thumbnail
   */
  PREVIEW: "preview",

  /**
   * High quality mode - server returns high quality image, possibly with original filetype.
   */
  HIGH_QUALITY: "high_quality",

  /**
   * Server ignores any resizing parameters and just return raw image as is.
   */
  RAW: "raw"
} as const;

export type MediaMode = typeof MEDIA_MODE[keyof typeof MEDIA_MODE];
