﻿using System;
using System.Collections.Generic;
using System.Linq;
using FiresecAPI.Models;
using FiresecClient;
using Infrastructure.Common;

namespace DevicesModule.ViewModels
{
    public class DeviceDetailsViewModel : DialogContent
    {
        public DeviceDetailsViewModel(Guid deviceUID)
        {
            _device = FiresecManager.DeviceConfiguration.Devices.FirstOrDefault(x => x.UID == deviceUID);
            var deviceState = FiresecManager.DeviceStates.DeviceStates.FirstOrDefault(x => x.UID == _device.UID);
            deviceState.StateChanged += new Action(deviceState_StateChanged);
            DeviceControlViewModel = new DeviceControlViewModel(_device);

            Title = _device.Driver.ShortName + " " + _device.DottedAddress;
        }

        Device _device;
        DeviceControls.DeviceControl _deviceControl;
        public DeviceControlViewModel DeviceControlViewModel { get; private set; }

        public Driver Driver
        {
            get { return _device.Driver; }
        }

        void deviceState_StateChanged()
        {
            var deviceState = FiresecManager.DeviceStates.DeviceStates.FirstOrDefault(x => x.UID == _device.UID);

            if (_deviceControl != null)
            {
                _deviceControl.StateType = deviceState.StateType;
            }

            OnPropertyChanged("DeviceControlContent");
        }

        public object DeviceControlContent
        {
            get
            {
                _deviceControl = new DeviceControls.DeviceControl();
                _deviceControl.DriverId = _device.Driver.UID;

                var deviceState = FiresecManager.DeviceStates.DeviceStates.FirstOrDefault(x => x.UID == _device.UID);
                _deviceControl.StateType = deviceState.StateType;

                _deviceControl.Width = 50;
                _deviceControl.Height = 50;

                return _deviceControl;
            }
        }

        public string ConnectedTo
        {
            get
            {
                if (_device.Parent != null)
                {
                    return _device.Parent.Driver.Name;
                }
                return null;
            }
        }

        public string PresentationZone
        {
            get { return _device.GetPersentationZone(); }
        }

        public List<string> SelfStates
        {
            get
            {
                var selfStates = new List<string>();
                var deviceState = FiresecManager.DeviceStates.DeviceStates.FirstOrDefault(x => x.UID == _device.UID);
                foreach (var state in deviceState.States)
                {
                    selfStates.Add(state.DriverState.Name);
                }
                return selfStates;
            }
        }

        public List<string> ParentStates
        {
            get
            {
                var parentStates = new List<string>();
                var deviceState = FiresecManager.DeviceStates.DeviceStates.FirstOrDefault(x => x.UID == _device.UID);
                if (deviceState.ParentStringStates != null)
                    foreach (var parentState in deviceState.ParentStringStates)
                    {
                        parentStates.Add(parentState);
                    }
                return parentStates;
            }
        }

        public List<string> Parameters
        {
            get
            {
                var parameters = new List<string>();
                var deviceState = FiresecManager.DeviceStates.DeviceStates.FirstOrDefault(x => x.UID == _device.UID);
                if (deviceState.Parameters != null)
                    foreach (var parameter in deviceState.Parameters)
                    {
                        if (parameter.Visible)
                        {
                            if ((string.IsNullOrEmpty(parameter.Value)) || (parameter.Value == "<NULL>"))
                                continue;

                            parameters.Add(parameter.Caption + " - " + parameter.Value);
                        }
                    }
                return parameters;
            }
        }

        public StateType StateType
        {
            get
            {
                var deviceState = FiresecManager.DeviceStates.DeviceStates.FirstOrDefault(x => x.UID == _device.UID);
                return deviceState.StateType;
            }
        }
    }
}