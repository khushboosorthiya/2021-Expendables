﻿using RealtimeNotifier.Feature.ItemActivities.Models;
using RealtimeNotifier.Foundation.SignalR.Services;
using Sitecore.Diagnostics;
using Sitecore.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealtimeNotifier.Feature.ItemActivities
{
    /// <summary>
    /// Class that handle to send push notification when item is saved by the user.
    /// </summary>
    public class ItemDeletedNotification
    {
        private ISignalRService signalRService;
        public ItemDeletedNotification(ISignalRService signalRService)
        {
            this.signalRService = signalRService;
        }

        protected void OnItemDeleted(object sender, EventArgs args)
        {
            Sitecore.Data.Items.Item item = Event.ExtractParameter<Sitecore.Data.Items.Item>(args, 0);
            if (item.Paths.FullPath.ToLowerInvariant().StartsWith("/sitecore/content") && signalRService != null)
            {
                signalRService.ItemActivitySignal(new ItemModel()
                {
                    ItemName = item.Name,
                    ItemID = item.ID.Guid.ToString("N"),
                    UserName = item.Statistics.UpdatedBy,
                    UserFullName = Sitecore.Context.User.Profile.FullName,
                    ItemPath = item.Paths.FullPath,
                    Message = $"{item.Name} has been deleted.",
                    DateTime = DateTime.Now.ToString()
                });
                Log.Info($"ItemSavedNotification.OnItemDeleted: Triggered realtime notification for {item.ID}", this);
            }
        }
    }
}