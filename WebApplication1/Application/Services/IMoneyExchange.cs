using System.Data;
using Dapper;
using Domain.Entities;

namespace Application.Services;

public interface IMoneyExchange
{
    public void CreateTransaction(Transaction transaction);
    public void CheckTransaction(Transaction transaction);
}

public class MoneyExchange : IMoneyExchange
{
    IDbConnection _connection;
    public MoneyExchange(IDbConnection connection)
    {
        _connection = connection;
    }
    public void CreateTransaction(Transaction transaction)
    {
        var sql = $"""
                   Insert into Transactions(FromAccountId,ToAccountId,Amount,PerformedAt) 
                   values
                       ({transaction.FromAccountId},{transaction.ToAccountId},{transaction.Amount},{transaction.PerformedAt})
                   """;
        _connection.Execute(sql);
        CheckTransaction(transaction);
    }

    public void CheckTransaction(Transaction transaction)
    {
        var sqlhave =
            "Select a.Balance from Accounts a join Transactions t on a.Id = t.FromAccountId and t.Id=@id";
        var have = _connection.ExecuteScalar<decimal>(sqlhave,new {id = transaction.Id});
        var sqlwant = "Select Amount from Transactions where Id = @Id;";
        var want = _connection.ExecuteScalar<decimal>(sqlwant,new {id = transaction.Id});
        if (have >= want)
        {
            
            var sql = """
                       Update Accounts 
                       set Balance=balance-@want
                       where id=@FromAccountId;
                       """;
            _connection.Execute(sql, new {want=want ,FromAccountId = transaction.FromAccountId});
            var sql2 = """
                       Update Accounts
                       set Balance=balance + @want
                       where id=@ToAccountId;
                       """;
            _connection.Execute(sql2, new {want=want ,ToAccountId = transaction.ToAccountId});
            var sql3 = """
                        Update Transactions
                        set Status='Success'
                        where id=@Id;
                        """;
            _connection.Execute(sql3,  new {id=transaction.Id});
        }
        else
        {
            var sql = """
                        Update Transactions
                        set Status='Failed'
                        where id=Id;
                        """;
            _connection.Execute(sql, new {Id=transaction.Id});
        }
    }
    
    
    
}