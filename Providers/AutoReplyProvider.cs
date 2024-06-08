using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Voxta.Model.WebsocketMessages.ClientMessages;
using Voxta.Providers.Host;

namespace Voxta.SampleProviderApp.Providers;

// This example shows how to use auto-reply and read from the appsettings.json
public class AutoReplyProvider : ProviderBase
{
    private readonly IOptions<SampleProviderAppOptions> _options;

    public AutoReplyProvider(
        IRemoteChatSession session,
        ILogger<AutoReplyProvider> logger,
        IOptions<SampleProviderAppOptions> options)
        : base(session, logger)
    {
        _options = options;
    }

    protected override async Task OnStartAsync()
    {
        await base.OnStartAsync();

        // Automatically reply when the user does not speak
        ConfigureAutoReply(TimeSpan.FromMilliseconds(_options.Value.AutoReplyDelay), OnAutoReply);
    }



    private void OnAutoReply()
    {
        Logger.LogInformation("Auto-replying after delay of {Delay}ms of inactivity", _options.Value.AutoReplyDelay);
        Send(new ClientSendMessage
        {
            SessionId = SessionId,
            Text = "[{{ char }} considers changing blowjob style.]"
        });
    }
}
