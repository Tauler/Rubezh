﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssadDevices;
using System.Threading;
using ClientApi;
using ServiceApi;
using System.Windows.Forms;

namespace AssadProcessor
{
    public class Controller
    {
        internal static Controller Current { get; private set; }
        AssadWatcher assadWather { get; set; }
        public ServiceClient serviceClient { get; set; }

        public Controller()
        {
            Current = this;
        }

        void StartAssad()
        {
            Services.NetManager.Start();

            assadWather = new AssadWatcher();
            assadWather.Start();
        }

        public void Start()
        {
            serviceClient = new ServiceClient();
            serviceClient.Start();
            StartAssad();
        }

        internal void AssadConfig(Assad.MHconfigTypeDevice innerDevice, bool all)
        {
            if (all)
            {
                AssadServices.AssadDeviceManager.Config(innerDevice);
                if (waitForConfiguration)
                {
                    waitForConfiguration = false;

                    ServiceApi.StateConfiguration configuration = AssadToServiceConverter.Convert();
                    serviceClient.SetNewConfig(configuration);
                }
            }
        }

        bool waitForConfiguration = false;

        void SetNewConfiguration()
        {
            waitForConfiguration = true;
            Assad.CPqueryConfigurationType cPqueryConfigurationType = new Assad.CPqueryConfigurationType();
            NetManager.Send(cPqueryConfigurationType, null);
        }

        public void AssadExecuteCommand(Assad.MHdeviceControlType controlType)
        {
            AssadBase assadDevice = AssadConfiguration.Devices.First(x => x.DeviceId == controlType.deviceId);
            if (controlType.cmdId == "Записать Конфигурацию")
            {
                // провести первичную проверку валидности
                if (AssadConfiguration.IsValid)
                {
                    SetNewConfiguration();
                }
                else
                {
                    MessageBox.Show("Неправильная конфигурация");
                }
            }
            else
            {
                Device device = Helper.ConvertDevice(assadDevice);
                serviceClient.ExecuteCommand(device, controlType.cmdId);
            }
        }

        public void CopyStatesFromService()
        {
            // сделать наоборот
            //foreach (Device device in ServiceClient.Configuration.Devices)
            //{
            //    AssadBase assadDevice = Helper.ConvertDevice(device);
            //    if (assadDevice != null)
            //    {
            //        assadDevice.State.State = device.State;
            //        assadDevice.SourceState = device.SourceState;
            //    }
            //}

            foreach (AssadBase assadBase in AssadConfiguration.Devices)
            {
                if (assadBase is AssadZone)
                {
                    AssadZone assadZone = assadBase as AssadZone;
                    if (ServiceClient.Configuration.Zones.Any(x => x.Id == assadZone.ZoneId))
                    {
                        Zone zone = ServiceClient.Configuration.Zones.FirstOrDefault(x => x.Id == assadZone.ZoneId);
                        assadZone.MainState = zone.State;
                    }
                    else
                    {
                        assadZone.MainState = "Отсутствует в конфигурации сервера оборудования";
                    }
                }
                else
                {
                    if (ServiceClient.Configuration.Devices.Any(x=>x.Path == assadBase.Path))
                    {
                        Device device = ServiceClient.Configuration.Devices.FirstOrDefault(x=>x.Path == assadBase.Path);
                        assadBase.MainState = device.State;
                        assadBase.Parameters = new List<AssadParameter>();

                        foreach (ClientApi.Parameter parameter in device.Parameters)
                        {
                            if (parameter.Visible == false)
                            {
                                if (string.IsNullOrEmpty(parameter.Value))
                                    continue;

                                if (parameter.Value == "<NULL>")
                                    continue;
                            }

                            AssadParameter assadParameter = new AssadParameter();
                            assadParameter.Name = parameter.Caption;
                            assadParameter.Value = parameter.Value;
                            assadParameter.Visible = parameter.Visible;
                            assadBase.Parameters.Add(assadParameter);
                        }

                        assadBase.States = new List<string>();
                        foreach (string state in device.States)
                        {
                            assadBase.States.Add(state);
                        }
                    }
                    else
                    {
                        assadBase.MainState = "Отсутствует в конфигурации сервера оборудования";
                    }
                }
            }
        }

        public void Stop()
        {
            Services.LogEngine.Save();
            serviceClient.Stop();
            Services.NetManager.Stop();
        }

        bool ready = false;
        internal bool Ready
        {
            get { return ready; }
            set
            {
                ready = true;
            }
        }
    }
}
