using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExamWebController: ControllerBase
{
    
    private readonly ICrudAccounts _crudAccounts;
    private readonly ICrudCustomers _crudCustomers;
    private readonly IMoneyExchange _moneyExchange;
    private readonly IFilterTransactions _filterTransactions;
    private readonly ITopClients _topClients;

    public ExamWebController(ICrudAccounts crudAccounts, ICrudCustomers crudCustomers, IFilterTransactions filterTransactions, IMoneyExchange moneyExchange, ITopClients topClients)
    {
        _crudAccounts = crudAccounts;
        _crudCustomers = crudCustomers;
        _filterTransactions = filterTransactions;
        _moneyExchange = moneyExchange;
        _topClients = topClients;
    }
    
    [HttpGet("GetAllAccounts")]
    public List<Account> GetAccounts()
    {
        return _crudAccounts.SelectAll().ToList();
    }

    [HttpPost("CreateAccount")]
    public IActionResult CreateAccount([FromBody] Account account)
    {
        _crudAccounts.Create(account);
        return Ok();
    }

    [HttpPut("UpdateAccount")]
    public IActionResult UpdateAccount([FromBody] Account account,[FromQuery] int id)
    {
        _crudAccounts.Update(account, id);
        return Ok();
    }

    [HttpDelete("DeleteAccount")]
    public IActionResult DeleteAccount([FromQuery] int id)
    {
        _crudAccounts.Delete(id);
        return Ok();
    }
    /////////////////////////////////////////////

    [HttpGet("GetAllCustomers")]
    public List<Customer> GetCustomers()
    {
        return _crudCustomers.SelectAll().ToList();
    }

    [HttpPost("createCustomer")]
    public IActionResult CreateCustomer([FromBody] Customer customer)
    {
        _crudCustomers.Create(customer);
        return Ok();
    }

    [HttpPut("UpdateCustomer")]
    public IActionResult UpdateCustomer([FromBody] Customer customer,[FromQuery] int id)
    {
        _crudCustomers.Update(customer,id);
        return Ok();
    }

    [HttpDelete("DeleteCustomer")]
    public IActionResult DeleteCustomer([FromQuery] int id)
    {
        _crudCustomers.Delete(id);
        return Ok();
    }

    [HttpPost("CreateTransaction")]
    public IActionResult CreateTransaction([FromBody] Transaction transaction)
    {
        _moneyExchange.CheckTransaction(transaction);
        return Ok();
    }

    [HttpGet("GetTransactionByCustomerId")]
    public List<Transaction> GetTransactionByCustomerId([FromQuery]int id)
    {
        return _filterTransactions.ByCustomerId(id);
    }
    [HttpGet("GetTransactionByDate")]
    public List<Transaction> GetTransactionByDate([FromQuery]DateTime fromdate, [FromQuery]DateTime todate)
    {
        return _filterTransactions.ByDate(fromdate,todate);
    }
    [HttpGet("GetTransactionByAmount")]
    public List<Transaction> GetTransactionByAmount([FromQuery]decimal minamount, [FromQuery]decimal maxamount)
    {
        return _filterTransactions.ByAmount(minamount,maxamount);
    }
    [HttpGet("GetTransactionByStatus")]
    public List<Transaction> GetTransactionByStatus([FromQuery]string status)
    {
        return _filterTransactions.ByStatus(status);
    }
}