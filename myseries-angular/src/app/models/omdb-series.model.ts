export interface OmdbSeriesDto {
  imdbId: string;   // corresponds to imdbID
  title: string;
  year: string;
  genre?: string;
  plot?: string;
  country?: string;
  poster?: string;
  imdbRating?: string;
  totalSeasons?: string;
}
