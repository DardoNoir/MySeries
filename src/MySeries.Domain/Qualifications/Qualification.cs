using Volo.Abp.Domain.Entities;

public class Qualification : AggregateRoot<int>
{
    public int UserId { get; private set; }
    public int SerieId { get; private set; }
    public int Score { get; set; }
    public string? Review { get; set; }

    // Constructor para EF Core
    protected Qualification()
    {
    }

    // Constructor de dominio
    public Qualification(int userId, int serieId, int score, string? review)
    {
        UserId = userId;
        SerieId = serieId;
        Score = score;
        Review = review;
    }
}