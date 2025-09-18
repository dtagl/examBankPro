
using System.Data;
using Dapper;
using Domain.Entities;

namespace Application.Services;

public interface ICrudAccounts
{
    public void Create(Account account);
    public void Update(Account account);
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
            """
             Insert into Accounts(CustomerId, AccountNumber, Balance, Currency) 
             values
                 (@CustomerId,@AccountNumber,@Balance,@Currency);
             """;
        _connection.Execute(sql,new {CustomerId = account.CustomerId, AccountNumber = account.AccountNumber, Balance = account.Balance, Currency = account.Currency});
    }

    public void Update(Account account)
    {
        var sql = """
                    UPDATE Accounts
                    SET CustomerId = @CustomerId,
                        AccountNumber = @AccountNumber,
                        Balance = @Balance,
                        Currency = @Currency
                    WHERE Id = @id;
                    """;
        _connection.Execute(sql, account);
    }

    public void Delete(int id)
    {
        var sql="Delete from Accounts where id=@id;";
        _connection.Execute(sql, new  { id = id });
    }

    public IEnumerable<Account> SelectAll()
    {
        var sql="Select * From Accounts;";
        return _connection.Query<Account>(sql);
    }
}