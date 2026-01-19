using Volo.Abp.Domain.Entities;

public class Qualification : AggregateRoot<int>
{
    public int UserId { get; private set; }
    public int SerieId { get; private set; }
    public int Score { get; set; }
    public string? Review { get; set; }

    protected Qualification()
    {
        // Requerido por EF Core
    }

    public Qualification(int userId, int serieId, int score, string? review)
    {
        UserId = userId;
        SerieId = serieId;
        Score = score;
        Review = review;
    }
}
