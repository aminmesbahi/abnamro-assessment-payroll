using Assessment.ApplicationCore;
using Assessment.ApplicationCore.Interfaces;
using Assessment.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy",
                      builder =>
                      {
                          //builder.WithOrigins(baseUrlConfig.WebBase.Replace("host.docker.internal", "localhost").TrimEnd('/'));
                          builder.AllowAnyOrigin();
                          builder.AllowAnyMethod();
                          builder.AllowAnyHeader();
                      });
});

builder.Services.AddControllers().AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (!builder.Environment.IsDevelopment()) { 
    builder.Services.AddDbContext<PayrollContext>(c =>
                c.UseSqlServer(builder.Configuration.GetConnectionString("PayrollConnection")));

}
else
{
    builder.Services.AddDbContext<PayrollContext>(c =>
      c.UseInMemoryDatabase("Catalog"));
   // builder.Services.AddDbContext<PayrollContext>(c =>
     //         c.UseSqlServer(builder.Configuration.GetConnectionString("PayrollConnection")));
}

builder.Services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
builder.Services.AddAutoMapper(typeof(Program).Assembly);
var baseUrlConfig = new BaseUrlConfiguration();
builder.Configuration.Bind(BaseUrlConfiguration.CONFIG_NAME, baseUrlConfig);

var app = builder.Build();
app.UseCors("CorsPolicy");
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {        
        var payrollContext = services.GetRequiredService<PayrollContext>();
        await payrollContext.Database.EnsureCreatedAsync();
        await PayrollContextSeed.SeedAsync(payrollContext, loggerFactory);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
