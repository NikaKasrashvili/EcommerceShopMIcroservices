var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(connectionString!);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName); // make username identity field
}).UseLightweightSessions();

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddScoped<IBasketRespository, BasketRepository>();



var app = builder.Build();

app.MapCarter();


app.MapGet("/", () => "Hello World!");

app.Run();
