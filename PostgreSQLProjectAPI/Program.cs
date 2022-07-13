using API.Data;
using API.Data.Repositories.Clients;
using API.Data.Repositories.Banks;
using API.Data.Repositories.Accounts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

ConfigurationManager configuration = builder.Configuration;

var postgreSQLConnectionConfig = new PostgreSQLConfiguration(configuration.GetConnectionString("PostgreSQLConnectionConfig"));

builder.Services.AddSingleton(postgreSQLConnectionConfig);

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IBankRepository, BankRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

