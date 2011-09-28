﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using FiresecClient;
using System.Windows;

namespace ItvIntegration
{
    public class DevicesViewModel : INotifyPropertyChanged
    {
        public void Initialize()
        {
            var result = FiresecManager.Connect("adm", "");
            if (result != null)
            {
                MessageBox.Show(result);
                return;
            }
            FiresecManager.SelectiveFetch();

            Devices = new ObservableCollection<DeviceViewModel>();
            foreach (var deviceState in FiresecManager.DeviceStates.DeviceStates)
            {
                var deviceViewModel = new DeviceViewModel(deviceState);
                Devices.Add(deviceViewModel);
            }
        }

        public ObservableCollection<DeviceViewModel> Devices { get; set; }

        DeviceViewModel _selectedDevice;
        public DeviceViewModel SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value;
                OnPropertyChanged("StateType");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
