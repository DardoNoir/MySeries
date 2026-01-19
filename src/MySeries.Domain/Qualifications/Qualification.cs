using Volo.Abp.Domain.Entities;

public class Qualification : AggregateRoot<int>
{
    public int UserId { get; private set; }
    public int SerieId { get; private set; }
    public int Score { get; set; }
    public string? Review { get; set; }

    protected Qualification()
    {
        public int UserId { get; set; }
        public int SerieId { get; set; }
        public int Score { get; set; } // Puntuación del 1 al 10
        public string? Review { get; set; } // Comentario opcional

    public Qualification(int userId, int serieId, int score, string? review)
    {
        UserId = userId;
        SerieId = serieId;
        Score = score;
        Review = review;
    }
}
