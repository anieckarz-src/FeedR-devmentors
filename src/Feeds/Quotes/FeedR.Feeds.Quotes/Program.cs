using FeedR.Feeds.Quotes.Pricing.Requests;
using FeedR.Feeds.Quotes.Pricing.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<PricingRequestChannel>();
builder.Services.AddSingleton<IPricingGenerator, PricingGenerator>();
builder.Services.AddHostedService<PricingBackgroundService>();


var app = builder.Build();

app.MapGet("/", () => "FeedR Quotes feed");

app.MapPost("/pricing/start", async (PricingRequestChannel channel) =>
{
    await channel.Requests.Writer.WriteAsync(new StartPricing());
    return Results.Accepted();
});

app.MapPost("/pricing/stop", async (PricingRequestChannel channel) =>
{
    await channel.Requests.Writer.WriteAsync(new StopPricing());
    return Results.Ok();
});

app.Run();