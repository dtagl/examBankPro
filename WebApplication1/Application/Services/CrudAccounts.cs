
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
             Insert into Accounts(CustomerId, AccountNumber, Balance, Curency) 
             values
                 ({account.CustomerId},{account.AccountNumber},{account.Balance},{account.Curency});
             """;
        _connection.Execute(sql);
    }

    public void Update(Account account, int id)
    {
        var sql = $"""
                   Update Accounts(CustomerId, AccountNumber, Balance, Curency)
                   Set values({account.CustomerId},{account.AccountNumber},{account.Balance},{account.Curency})
                   where id={account.Id};
                   """;
        _connection.Execute(sql);
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