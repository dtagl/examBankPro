using System.Data;
using Dapper;
using Domain.Entities;
using Npgsql;
namespace Application.Services;

public interface ICrudCustomers
{
    public void Create(Customer customer);
    public void Update(Customer customer, int id);
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
            $"""
             Insert into Customers(FullName, Phone, Email,RegisterAt,IsActive) 
             values
                 ({customer.FullName},{customer.Phone},{customer.Email},{customer.RegistredAt},{customer.IsActive});
             """;
        _connection.Execute(sql);
    }

    public void Update(Customer customer, int id)
    {
        var sql = $"""
                  Update Customers(FullName, Phone, Email, RegisterAt, IsActive)
                  Set values({customer.FullName},{customer.Phone},{customer.Email},{customer.RegistredAt},{customer.IsActive})
                  where id={customer.Id};
                  """;
        _connection.Execute(sql);
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