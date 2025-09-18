using System.Data;
using Dapper;

namespace Application.Services;

public interface ITopClients
{
    
}
public class TopCustomerDto
{
    public string? FullName { get; set; }
    public decimal TotalTurnover { get; set; }
}
public class TopClient : ITopClients
{
    private IDbConnection _connection;
    public TopClient(IDbConnection connection)
    {
        _connection = connection;
    }
    public void FindTop5()
    {
        var sql = """
                  SELECT c.FullName as FullName,SUM(t.Amount) as TotalTurnover
                  from Transactions t
                  join Accounts a on a.Id = t.FromAccountId or c.Id = t.ToAccountId
                  join Customers c on a.customerId = c.Id 
                  GROUP BY a.customerId order by TotalTurnover limit 5;
                  """;
        var result = _connection.Query<TopCustomerDto>(sql);
    }
}