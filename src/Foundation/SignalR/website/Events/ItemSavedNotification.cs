﻿using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Sitecore.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealtimeNotifier.Foundation.SignalR.Events
{
    public class ItemSavedNotification
    {
        protected void OnItemSaved(object sender, EventArgs args)
        {
            Sitecore.Data.Items.Item item = Event.ExtractParameter<Sitecore.Data.Items.Item>(args, 0);
            if (item.Paths.FullPath.ToLowerInvariant().StartsWith("/sitecore/content"))
            {
                var hub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                hub.Clients.All.notify(new Notification() { ItemName = item.Name });
            }
        }
    }

    public class Notification
    {
        [JsonProperty("itemName")]
        public String ItemName { get; set; }
    }
}