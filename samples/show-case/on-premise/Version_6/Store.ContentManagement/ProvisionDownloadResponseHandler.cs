﻿namespace Store.ContentManagement
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Messages.Events;
    using Messages.RequestResponse;
    using NServiceBus;
    using Common;

    public class ProvisionDownloadResponseHandler : IHandleMessages<ProvisionDownloadResponse>
    {
        IBus bus;

        public ProvisionDownloadResponseHandler(IBus bus)
        {
            this.bus = bus;
        }

        Dictionary<string, string> productIdToUrlMap = new Dictionary<string, string>
            {
                {"videos", "http://particular.net/videos-and-presentations"},
                {"training", "http://particular.net/onsite-training"},
                {"documentation", "http://docs.particular.net/"},
                {"customers", "http://particular.net/customers"},
                {"platform", "http://particular.net/service-platform"},
            };

        public async Task Handle(ProvisionDownloadResponse message)
        {
            if (DebugFlagMutator.Debug)
            {
                Debugger.Break();
            }

            Console.WriteLine("Download for Order # {0} has been provisioned, Publishing Download ready event", message.OrderNumber);

            await bus.PublishAsync<DownloadIsReady>(e =>
            {
                e.OrderNumber = message.OrderNumber;
                e.ClientId = message.ClientId;
                e.ProductUrls = new Dictionary<string, string>();

                foreach (var productId in message.ProductIds)
                {
                    e.ProductUrls.Add(productId, productIdToUrlMap[productId]);
                }
            });

            Console.WriteLine("Downloads for Order #{0} is ready, publishing it.", message.OrderNumber);
        }
    }
}
