using System.Data;
using System.Transactions;
using Dapper;
using Transaction = Domain.Entities.Transaction;

namespace Application.Services;

public interface IFilterTransactions
{
    public List<Transaction> ByCustomerId(int customerId);
    public List<Transaction>  ByDate(DateTime fromDate, DateTime toDate);
    public List<Transaction>  ByAmount(decimal minamount, decimal maxamount);
    public List<Transaction>  ByStatus(string status);


}


public class FilterTransactions : IFilterTransactions
{
    IDbConnection _connection;
    
    public FilterTransactions(IDbConnection connection)
    {
        _connection = connection;
    }

    public List<Transaction> ByCustomerId(int customerId)
    {
        var sql = $"Select * from Transactions where FromAccountId = {customerId} or ToAccountId = {customerId};";
        var result = _connection.Query<Transaction>(sql);
        return result.ToList();
    }

    public List<Transaction>  ByDate(DateTime fromDate, DateTime toDate)
    {
        var sql = $"Select * from transactions where PerformedAt>={fromDate} and PerformedAt<={toDate};";
        var result = _connection.Query<Transaction>(sql);
        return result.ToList();
    }

    public List<Transaction>  ByAmount(decimal minamount, decimal maxamount)
    {
        var sql = $"Select * from transactions where Amount>={minamount} and Amount<={maxamount};";
        var result = _connection.Query<Transaction>(sql);
        return result.ToList();
    }

    public List<Transaction>  ByStatus(string status)
    {
        var sql = $"Select * from transactions where Status={status};";
        var result = _connection.Query<Transaction>(sql);
        return result.ToList();
    }
    
}