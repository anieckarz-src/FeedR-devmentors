using FeedR.Feeds.Quotes.Pricing.Models;

namespace FeedR.Feeds.Quotes.Pricing.Services;

internal sealed class PricingGenerator : IPricingGenerator
{
    private readonly Random _random = new Random();

    private readonly Dictionary<string, decimal> _currencyPairs = new()
    {
        ["EURUSD"] = 1.12m,
        ["EURGBP"] = 0.88m,
        ["EURJPY"] = 129.55m,
        ["EURCHF"] = 1.05m,
        ["EURCAD"] = 1.51m,
        ["EURAUD"] = 1.62m,
        ["EURNZD"] = 1.77m,
    };

    private bool _isRunning;

    public async Task StartAsync()
    {
        _isRunning = true;
        while (_isRunning)
        {
            foreach (var (symbol, pricing) in _currencyPairs)
            {
                if (!_isRunning)
                {
                    return;
                }

                var tick = NextTick();
                var newPricing = pricing + tick;
                _currencyPairs[symbol] = newPricing;

                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                var currencyPair = new CurrencyPair(symbol, newPricing, timestamp);
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }

    public Task StopAsync()
    {
        _isRunning = false;
        throw new NotImplementedException();
    }

    private decimal NextTick()
    {
        var sign = _random.Next(0, 2) == 0 ? 1 : -1;
        var tick = _random.NextDouble() / 20;
        return (decimal)(sign * tick);
    }
}