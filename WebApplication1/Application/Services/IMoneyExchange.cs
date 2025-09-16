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
        var want = _connection.ExecuteScalar<decimal>($"Select Amount from Transactions t where t.FromAccountId = {transaction.FromAccountId};");
        if (have >= want)
        {
            
            var sql = $"""
                       Update Accounts(Amount)
                       Set values({have-want})
                       where id={transaction.FromAccountId};
                       """;
            _connection.Execute(sql);
            var sql2 = $"""
                       Update Accounts(Amount)
                       Set values({have+want})
                       where id={transaction.ToAccountId};
                       """;
            _connection.Execute(sql2);
            var sql3 = $"""
                        Update Transactions(Status)
                        Set values({"Success"})
                        where id="{transaction.Id};";
                        """;
            _connection.Execute(sql3);
            

        }
        else
        {
            var sql = $"""
                        Update Transactions(Status)
                        Set values({"Failed"})
                        where id="{transaction.Id};";
                        """;
            _connection.Execute(sql);
        }
    }
    
    
    
}