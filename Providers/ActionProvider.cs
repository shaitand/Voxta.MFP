
using Microsoft.Extensions.Logging;
using Voxta.Model.Shared;
using Voxta.Model.WebsocketMessages.ClientMessages;
using Voxta.Model.WebsocketMessages.ServerMessages;
using Voxta.Providers.Host;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Voxta.SampleProviderApp.Providers;

public class ActionProvider : ProviderBase
{
    private UdpClient client;
    private IPEndPoint remoteEndPoint;

    public ActionProvider(IRemoteChatSession session, ILogger<ActionProvider> logger) : base(session, logger)
    {
        client = new UdpClient(new IPEndPoint(IPAddress.Loopback, 8000));
    }

    protected override async Task OnStartAsync()
    {
        await base.OnStartAsync();

        try
        {
            Console.WriteLine("Waiting for MFP Connection...");

            // Asynchronously waiting for an incoming message
            UdpReceiveResult receivedResult = await client.ReceiveAsync();
            remoteEndPoint = receivedResult.RemoteEndPoint;
            Console.WriteLine($"Received from {remoteEndPoint}");

            // Set the endpoint for subsequent sends
            client.Connect(remoteEndPoint);
            Console.WriteLine("Connected to MFP.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        // Register new action
        Send(new ClientUpdateContextMessage
        {
            SessionId = SessionId,
            ContextKey = "sexActions",
            CharacterFunctions =
            [
                new()
                {
                    Name = "playToggle",
                    Layer = "mfp_control",
                    Description = "When {{ char }} wants to play or pause interaction with {{ user }} penis.",
                    Effect = "{{ char }} begins to pleasure {{ user }}.",
                    Arguments = []
                },
                new()
                {
                    Name = "facefuck",
                    Layer = "mfp_control",
                    Description = "When {{ char }} wants to {{ user }} to fuck her throat and make her gag.",
                    Effect = "{{ char }} begins to gag herself on {{ user }} penis.",
                    Arguments = []
                },
                new()
                {
                    Name = "medium_deepthroat",
                    Layer = "mfp_control",
                    Description = "When {{ char }} wants to use medium deepthroat blowjob style on {{ user }} penis.",
                    Effect = "{{ char }} begins to deepthroat {{ user }} hard.",
                    Arguments = []
                },
                new()
                {
                    Name = "suck01",
                    Layer = "mfp_control",
                    Description = "When {{ char }} wants to suck {{ user }} penis.",
                    Effect = "{{ char }} sucks {{ user }} penis.",
                    Arguments = []
                },
                new()
                {
                    Name = "suck02",
                    Layer = "mfp_control",
                    Description = "When {{ char }} wants to use an alternate pattern of sucking {{ user }} penis.",
                    Effect = "{{ char }} sucks {{ user }} penis more.",
                    Arguments = []
                },
                new()
                {
                    Name = "tease01",
                    Layer = "mfp_control",
                    Description = "When {{ char }} wants to tease and suck {{ user }} penis.",
                    Effect = "{{ char }} teases the tip of {{ user }}.",
                    Arguments = []
                },
                new()
                {
                    Name = "deepthroat",
                    Layer = "mfp_control",
                    Description = "When {{ char }} wants to use deepthroat style blowjob {{ user }} penis.",
                    Effect = "{{ char }} begins to deepthroat {{ user }}.",
                    Arguments = []
                },
                new()
                {
                    Name = "noop",
                    Layer = "mfp_control",
                    Description = "When {{ char }} wants to continue the current style blowjob {{ user }} penis.",
                    Effect = "{{ char }} keeps giving {{ user }} a hot blowjob.",
                    Arguments = []
                }
            ]
        });

        // Act when an action is called
        HandleMessage<ServerActionMessage>(async message =>
        {
            if (message.Layer != "mfp_control") return;

            byte[] myMessage;
            int bytesSent;

            switch (message.Value)
            {
                case "playToggle":
                    myMessage = Encoding.UTF8.GetBytes("#playToggle:1\n");
                    bytesSent = await client.SendAsync(myMessage, myMessage.Length);
                    Console.WriteLine($"Sent message '{Encoding.UTF8.GetString(myMessage)}' to {remoteEndPoint}");
                    Logger.LogInformation("#playToggle:1");
                    break;
                case "facefuck":
                    myMessage = Encoding.UTF8.GetBytes("#facefuck:1\n");
                    bytesSent = await client.SendAsync(myMessage, myMessage.Length);
                    Console.WriteLine($"Sent message '{Encoding.UTF8.GetString(myMessage)}' to {remoteEndPoint}");
                    Logger.LogInformation("#facefuck:1");
                    break;
                case "medium_deepthroat":
                    myMessage = Encoding.UTF8.GetBytes("#medium_deepthroat:1\n");
                    bytesSent = await client.SendAsync(myMessage, myMessage.Length);
                    Console.WriteLine($"Sent message '{Encoding.UTF8.GetString(myMessage)}' to {remoteEndPoint}");
                    Logger.LogInformation("#medium_deepthroat:1");
                    break;
                case "suck01":
                    myMessage = Encoding.UTF8.GetBytes("#suck01:1\n");
                    bytesSent = await client.SendAsync(myMessage, myMessage.Length);
                    Console.WriteLine($"Sent message '{Encoding.UTF8.GetString(myMessage)}' to {remoteEndPoint}");
                    Logger.LogInformation("#suck01:1");
                    break;
                case "suck02":
                    myMessage = Encoding.UTF8.GetBytes("#suck02:1\n");
                    bytesSent = await client.SendAsync(myMessage, myMessage.Length);
                    Console.WriteLine($"Sent message '{Encoding.UTF8.GetString(myMessage)}' to {remoteEndPoint}");
                    Logger.LogInformation("#suck02:1");
                    break;
                case "tease01":
                    myMessage = Encoding.UTF8.GetBytes("#tease01:1\n");
                    bytesSent = await client.SendAsync(myMessage, myMessage.Length);
                    Console.WriteLine($"Sent message '{Encoding.UTF8.GetString(myMessage)}' to {remoteEndPoint}");
                    Logger.LogInformation("#tease01:1");
                    break;
                case "deepthroat":
                    myMessage = Encoding.UTF8.GetBytes("#deepthroat:1\n");
                    bytesSent = await client.SendAsync(myMessage, myMessage.Length);
                    Console.WriteLine($"Sent message '{Encoding.UTF8.GetString(myMessage)}' to {remoteEndPoint}");
                    Logger.LogInformation("#deepthroat:1");
                    break;
                case "noop":
                    //do nothing!
                    break;
            }
        });
    }
}
