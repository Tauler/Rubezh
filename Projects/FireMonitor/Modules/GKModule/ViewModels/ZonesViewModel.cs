﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FiresecClient;
using Infrastructure.Common.Windows.ViewModels;
using XFiresecAPI;

namespace GKModule.ViewModels
{
    public class ZonesViewModel : ViewPartViewModel, ISelectable<Guid>
    {
        public static ZonesViewModel Current { get; private set; }
        public ZonesViewModel()
        {
            Current = this;
        }

        public void Initialize()
        {
			Zones = new List<ZoneViewModel>();
			foreach (var zone in XManager.Zones)
			{
				if (zone.Devices.Count > 0)
				{
					var zoneViewModel = new ZoneViewModel(zone.ZoneState);
					Zones.Add(zoneViewModel);
				}
			}
            SelectedZone = Zones.FirstOrDefault();
        }

        List<ZoneViewModel> _zones;
        public List<ZoneViewModel> Zones
        {
            get { return _zones; }
            set
            {
                _zones = value;
                OnPropertyChanged("Zones");
            }
        }

        ZoneViewModel _selectedZone;
        public ZoneViewModel SelectedZone
        {
            get { return _selectedZone; }
            set
            {
                _selectedZone = value;
                InitializeDevices();
                OnPropertyChanged("SelectedZone");
            }
        }

        public void Select(Guid zoneUID)
        {
            if (zoneUID != Guid.Empty)
            {
                SelectedZone = Zones.FirstOrDefault(x => x.Zone.UID == zoneUID);
            }
        }

        public DeviceViewModel RootDevice { get; private set; }
        public DeviceViewModel[] RootDevices
        {
            get { return RootDevice == null ? null : new DeviceViewModel[] { RootDevice }; }
        }

        void InitializeDevices()
        {
            if (SelectedZone == null)
                return;

            var devices = new HashSet<XDevice>();

            foreach (var device in XManager.Devices)
            {
                if (device.ZoneUIDs.Contains(SelectedZone.Zone.UID))
                {
                    device.AllParents.ForEach(x => { devices.Add(x); });
                    devices.Add(device);
                }
            }

            var deviceViewModels = new ObservableCollection<DeviceViewModel>();
            foreach (var device in devices)
            {
                deviceViewModels.Add(new DeviceViewModel(device)
                {
                    IsExpanded = true,
                });
            }

            foreach (var device in deviceViewModels)
            {
                if (device.Device.Parent != null)
                {
                    var parent = deviceViewModels.FirstOrDefault(x => x.Device.UID == device.Device.Parent.UID);
                    parent.AddChild(device);
                }
            }

            RootDevice = deviceViewModels.FirstOrDefault(x => x.Parent == null);
            OnPropertyChanged("RootDevices");
        }
    }
}