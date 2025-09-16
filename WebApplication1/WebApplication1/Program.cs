using System.Data;
using Application.Services;
using Dapper;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDbConnection>(sp => new NpgsqlConnection("Host=localhost;Port=5432;Database=BankPro;Username=postgres;Password=dentager2005"));
builder.Services.AddScoped<ICrudAccounts, CrudAccounts>();
builder.Services.AddScoped<ICrudCustomers, CrudCustomers>();
builder.Services.AddScoped<IFilterTransactions, FilterTransactions>();
builder.Services.AddScoped<ITopClients, TopClient>();
builder.Services.AddScoped<IMoneyExchange, MoneyExchange>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c=>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
