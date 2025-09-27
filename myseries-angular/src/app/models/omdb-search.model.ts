export interface OmdbSeriesSearchItemDto {
  imdbId: string;
  title: string;
  year: string;
  type: string;
  poster?: string;
  genre?: string;
}

export interface OmdbSeriesSearchDto {
  search: OmdbSeriesSearchItemDto[];
  totalResults?: string;
  response?: string;
}
