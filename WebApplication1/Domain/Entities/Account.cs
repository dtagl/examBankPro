namespace Domain.Entities;

public class Account
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; }
}