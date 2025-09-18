
using System.Data;
using Dapper;
using Domain.Entities;

namespace Application.Services;

public interface ICrudAccounts
{
    public void Create(Account account);
    public void Update(Account account,int id);
    public void Delete(int id);
    public IEnumerable<Account> SelectAll();





}
public class CrudAccounts:ICrudAccounts
{
    
    private IDbConnection _connection;
    public CrudAccounts(IDbConnection connection)
    {
        _connection = connection;
    }

    public void Create(Account account)
    {
        var sql =
            $"""
             Insert into Accounts(CustomerId, AccountNumber, Balance, Currency) 
             values
                 ({account.CustomerId},'{account.AccountNumber}',{account.Balance},'{account.Currency}');
             """;
        _connection.Execute(sql);
    }

    public void Update(Account account, int id)
    {
        var sql = $"""
                    UPDATE Accounts
                    SET CustomerId = {account.CustomerId},
                        AccountNumber = '{account.AccountNumber}',
                        Balance = {account.Balance},
                        Currency = '{account.Currency}'
                    WHERE Id = {id};
                    """;
        _connection.ExecuteScalar(sql);
    }

    public void Delete(int id)
    {
        var sql=$"Delete from Accounts where id={id};";
        _connection.Execute(sql);
    }

    public IEnumerable<Account> SelectAll()
    {
        var sql="Select * From Accounts;";
        return _connection.Query<Account>(sql);
    }
}