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
        
        var have = _connection.ExecuteScalar<decimal>($"Select a.Balance From Accounts a join Customers c on c.Id = a.CustomerId and c.Id={transaction.FromAccountId};");
        var want = _connection.ExecuteScalar<decimal>($"Select t.Amount from Transactions t where t.FromAccountId = {transaction.FromAccountId};");
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
                       set Balance=@want
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