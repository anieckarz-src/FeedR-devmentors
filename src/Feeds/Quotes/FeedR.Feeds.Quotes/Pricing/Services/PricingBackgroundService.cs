using FeedR.Feeds.Quotes.Pricing.Requests;

namespace FeedR.Feeds.Quotes.Pricing.Services;

internal class PricingBackgroundService : BackgroundService
{

    private int _runningStatus;
    private readonly IPricingGenerator _pricingGenerator;
    private readonly PricingRequestChannel _requestChannel;

    public PricingBackgroundService(IPricingGenerator pricingGenerator,PricingRequestChannel requestChannel)
    {
        _pricingGenerator = pricingGenerator;
        _requestChannel = requestChannel;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var request in _requestChannel.Requests.Reader.ReadAllAsync(stoppingToken))
        {
            var _ = request switch
            {
                StartPricing => StartGeneratorAsync(),
                StopPricing=> StopGeneratorAsync(),
                _ => Task.CompletedTask
            };
        }
    }

    private async Task StartGeneratorAsync()
    {
        if (Interlocked.Exchange(ref _runningStatus, 1) == 1)
        {
            return;
        }

        await _pricingGenerator.StartAsync();
    }

    private async Task StopGeneratorAsync()
    {
        if (Interlocked.Exchange(ref _runningStatus, 0) == 0)
        { 
            return;
        }

        await _pricingGenerator.StopAsync();
    }
}