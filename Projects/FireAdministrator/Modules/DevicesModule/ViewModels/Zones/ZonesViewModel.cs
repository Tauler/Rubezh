﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Common;
using System.Collections.ObjectModel;
using FiresecClient;
using Infrastructure;
using System.Windows;
using FiresecClient.Models;
using DevicesModule.Events;

namespace DevicesModule.ViewModels
{
    public class ZonesViewModel : RegionViewModel
    {
        public ZoneDevicesViewModel ZoneDevices { get; set; }

        public ZonesViewModel()
        {
            AddCommand = new RelayCommand(OnAdd);
            DeleteCommand = new RelayCommand(OnDelete, CanDelete);
            EditCommand = new RelayCommand(OnEdit, CanEdit);
            ZoneDevices = new ZoneDevicesViewModel();
        }

        public void Initialize()
        {
            Zones = new ObservableCollection<ZoneViewModel>(
                from zone in FiresecManager.Configuration.Zones
                orderby (Convert.ToInt32(zone.No))
                select new ZoneViewModel(zone));

            if (Zones.Count > 0)
                SelectedZone = Zones[0];
        }

        ObservableCollection<ZoneViewModel> _zones;
        public ObservableCollection<ZoneViewModel> Zones
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

                if (value != null)
                {
                    ZoneDevices.Initialize(value.No);
                }

                OnPropertyChanged("SelectedZone");
            }
        }

        public RelayCommand AddCommand { get; private set; }
        void OnAdd()
        {
            Zone newZone = new Zone();
            newZone.Name = "Новая зона";
            var maxNo = (from zone in FiresecManager.Configuration.Zones select Convert.ToInt32(zone.No)).Max();
            newZone.No = (maxNo + 1).ToString();

            ZoneDetailsViewModel zoneDetailsViewModel = new ZoneDetailsViewModel(newZone);
            var result = ServiceFactory.UserDialogs.ShowModalWindow(zoneDetailsViewModel);
            if (result)
            {
                FiresecManager.Configuration.Zones.Add(newZone);
                ZoneViewModel zoneViewModel = new ZoneViewModel(newZone);
                Zones.Add(zoneViewModel);
            }
        }

        bool CanDelete(object obj)
        {
            return SelectedZone != null;
        }

        public RelayCommand DeleteCommand { get; private set; }
        void OnDelete()
        {
            if (CanDelete(null))
            {
                var dialogResult = MessageBox.Show("Вы уверены, что хотите удалить зону " + SelectedZone.PresentationName, "Подтверждение", MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    FiresecManager.Configuration.Zones.Remove(SelectedZone.Zone);
                    Zones.Remove(SelectedZone);
                }
            }
        }

        bool CanEdit(object obj)
        {
            return SelectedZone != null;
        }

        public RelayCommand EditCommand { get; private set; }
        void OnEdit()
        {
            if (CanEdit(null))
            {
                ZoneDetailsViewModel zoneDetailsViewModel = new ZoneDetailsViewModel(SelectedZone.Zone);
                bool result = ServiceFactory.UserDialogs.ShowModalWindow(zoneDetailsViewModel);
                SelectedZone.Update();
            }
        }

        public override void OnShow()
        {
            ZonesMenuViewModel zonesMenuViewModel = new ZonesMenuViewModel(AddCommand, DeleteCommand, EditCommand);
            ServiceFactory.Layout.ShowMenu(zonesMenuViewModel);
        }

        public override void OnHide()
        {
            ServiceFactory.Layout.ShowMenu(null);
        }
    }
}
