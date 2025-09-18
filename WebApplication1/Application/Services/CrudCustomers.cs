using System.Data;
using Dapper;
using Domain.Entities;
using Npgsql;
namespace Application.Services;

public interface ICrudCustomers
{
    public void Create(Customer customer);
    public void Update(Customer customer);
    public void Delete(int id);
    public IEnumerable<Customer> SelectAll();

}

public class CrudCustomers: ICrudCustomers
{
    private IDbConnection _connection;
    public CrudCustomers(IDbConnection connection)
    {
        _connection = connection;
    }

    public void Create(Customer customer)
    {
        var sql =
            """
             Insert into Customers(FullName, Phone, Email,RegisteredAt,IsActive) 
             values
              (@FullName,@Phone,@Email,@RegistredAt,@IsActive);
             """;
        _connection.Execute(sql, customer);
    }

    public void Update(Customer customer)
    {
        var sql = """
                  Update Customers 
                  set FullName = @FullName,
                      Phone = @Phone,
                      Email = @Email,
                      RegisteredAt = @RegisteredAt,
                      IsActive = @IsActive
                  where Id = @id;
                  """;
        _connection.Execute(sql, customer);
    }

    public void Delete(int id)
    {
        var sql=$"Delete from Customers where id={id};";
        _connection.Execute(sql);
    }

    public IEnumerable<Customer> SelectAll()
    {
        var sql="Select * From Customers;";
        return _connection.Query<Customer>(sql);
    }
}