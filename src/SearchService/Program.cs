using SearchService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.WebHost.UseUrls("http://localhost:7002");
builder.Services.AddControllers();
builder.Services.AddHttpClient<AuctionServiceHttpClient>();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();
try
{
    await DbInitializer.InitiDb(app);
}
catch (Exception e)
{
    Console.WriteLine(e);
}

app.Run();

