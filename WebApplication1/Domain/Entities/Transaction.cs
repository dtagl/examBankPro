namespace Domain.Entities;

public class Transaction
{
    public int Id { get; set; }
    public int FromAccountId { get; set; }
    public int ToAccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PerformedAt { get; set; }
    public string Status { get; set; }
}