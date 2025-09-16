namespace Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public DateTime RegistredAt { get; set; }
    public bool IsActive { get; set; }
}
