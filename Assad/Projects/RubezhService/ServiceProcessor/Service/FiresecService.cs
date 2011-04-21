﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceApi;
using Firesec;
using System.ServiceModel;

namespace ServiseProcessor.Service
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Single)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class FiresecService : IFiresecService
    {
        public FiresecService()
        {
            callbacks = new List<IFiresecCallback>();
            FiresecEventAggregator.NewEventAvaliable += new Action<int, string>(OnNewEventsAvailable);
        }

        static List<IFiresecCallback> callbacks;

        public static void OnNewEventsAvailable(int eventMask, string obj)
        {
            foreach (IFiresecCallback callback in callbacks)
            {
                try
                {
                    if (callback != null)
                    {
                        callback.NewEventsAvailable(eventMask, obj);
                    }
                }
                catch
                {
                    //callback = null;
                    //callbacks.Remove(callback);
                }
            }
        }

        public void Initialize()
        {
            IFiresecCallback callback = OperationContext.Current.GetCallbackChannel<IFiresecCallback>();
            callbacks.Add(callback);
        }

        public string Ping()
        {
            return "Pong";
        }

        public string GetCoreConfig()
        {
            return NativeFiresecClient.GetCoreConfig();
        }

        public string GetCoreState()
        {
            return NativeFiresecClient.GetCoreState();
        }

        public string GetMetaData()
        {
            return NativeFiresecClient.GetMetaData();
        }

        public string GetCoreDeviceParams()
        {
            return NativeFiresecClient.GetCoreDeviceParams();
        }

        public string ReadEvents(int fromId, int limit)
        {
            return NativeFiresecClient.ReadEvents(fromId, limit);
        }

        public void SetNewConfig(string config)
        {
            NativeFiresecClient.SetNewConfig(config);
        }

        public void DeviceWriteConfig(string config, string devicePath)
        {
            NativeFiresecClient.DeviceWriteConfig(config, devicePath);
        }

        public void ResetStates(string config)
        {
            NativeFiresecClient.ResetStates(config);
        }
    }
}