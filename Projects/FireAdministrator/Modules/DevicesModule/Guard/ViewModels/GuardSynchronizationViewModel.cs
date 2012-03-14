﻿using System.Collections.ObjectModel;
using DevicesModule.Guard;
using FiresecAPI.Models;
using FiresecClient;
using Infrastructure.Common;

namespace DevicesModule.ViewModels
{
    public class GuardSynchronizationViewModel : SaveCancelDialogContent
    {
        public GuardSynchronizationViewModel(Device device)
        {
            Title = "Синхронизация охранного прибора";
            CancelSystemCommand = new RelayCommand(OnCancelSystem, CanCancelSystem);
            ReadDeviceCommand = new RelayCommand(OnReadDevice, CanReadDevice);
            WriteDeviceCommand = new RelayCommand(OnWriteDevice, CanWriteDevice);
            SystemUsers = new ObservableCollection<GuardUser>();
            DeviceUsers = new ObservableCollection<GuardUser>();

            Device = device;

            foreach (var guardUser in FiresecManager.DeviceConfiguration.GuardUsers)
            {
                SystemUsers.Add(guardUser);
            }
        }

        Device Device;

        public ObservableCollection<GuardUser> SystemUsers { get; private set; }

        GuardUser _selectedSystemUser;
        public GuardUser SelectedSystemUser
        {
            get { return _selectedSystemUser; }
            set
            {
                _selectedSystemUser = value;
                OnPropertyChanged("SelectedSystemUser");
            }
        }

        public ObservableCollection<GuardUser> DeviceUsers { get; private set; }

        GuardUser _selectedDeviceUser;
        public GuardUser SelectedDeviceUser
        {
            get { return _selectedDeviceUser; }
            set
            {
                _selectedDeviceUser = value;
                OnPropertyChanged("SelectedDeviceUser");
            }
        }

        bool CanCancelSystem()
        {
            return true;
        }

        public RelayCommand CancelSystemCommand { get; private set; }
        void OnCancelSystem()
        {

        }

        bool CanReadDevice()
        {
            return true;
        }

        public RelayCommand ReadDeviceCommand { get; private set; }
        void OnReadDevice()
        {
            DeviceGetGuardUserListHelper.Run(Device);
        }

        bool CanWriteDevice()
        {
            return true;
        }

        public RelayCommand WriteDeviceCommand { get; private set; }
        void OnWriteDevice()
        {
            DeviceSetGuardUsersListHelper.Run(Device, "");
        }

        protected override void Save(ref bool cancel)
        {
        }
    }
}
