﻿using System;
using System.ComponentModel;
using System.Windows.Controls;
using Infrastructure;
using Infrastructure.Events;

namespace FireAdministrator.Views
{
    public partial class NavigationAreaView : UserControl, INotifyPropertyChanged
    {
        public NavigationAreaView()
        {
            InitializeComponent();
            DataContext = this;

            ServiceFactory.Events.GetEvent<ShowDeviceEvent>().Subscribe(x => { _isDevicesSelected = true; OnPropertyChanged("IsDevicesSelected"); });
            ServiceFactory.Events.GetEvent<ShowZoneEvent>().Subscribe(x => { _isZonesSelected = true; OnPropertyChanged("IsZonesSelected"); });
            ServiceFactory.Events.GetEvent<ShowDirectionsEvent>().Subscribe(x => { _isDerectonsSelected = true; OnPropertyChanged("IsDerectonsSelected"); });

            ServiceFactory.Events.GetEvent<GuardVisibilityChangedEvent>().Subscribe(x => { IsGuardVisible = x; });
            ServiceFactory.Events.GetEvent<ShowGuardEvent>().Subscribe(x => { _isGuardSelected = true; OnPropertyChanged("IsGuardSelected"); });

            ServiceFactory.Events.GetEvent<ShowLibraryEvent>().Subscribe(x => { _isLibrarySelected = true; OnPropertyChanged("IsLibrarySelected"); });
            ServiceFactory.Events.GetEvent<ShowPlansEvent>().Subscribe(x => { _isPlanSelected = true; OnPropertyChanged("IsPlanSelected"); });

            ServiceFactory.Events.GetEvent<ShowUsersEvent>().Subscribe(x => { _isUsersSelected = true; OnPropertyChanged("IsUsersSelected"); });
            ServiceFactory.Events.GetEvent<ShowUserGroupsEvent>().Subscribe(x => { _isUserGroupsSelected = true; OnPropertyChanged("IsUserGroupsSelected"); });

            ServiceFactory.Events.GetEvent<ShowJournalEvent>().Subscribe(x => { _isJournalSelected = true; OnPropertyChanged("IsJournalSelected"); });
            ServiceFactory.Events.GetEvent<ShowSoundsEvent>().Subscribe(x => { _isSoundsSelected = true; OnPropertyChanged("IsSoundsSelected"); });
            ServiceFactory.Events.GetEvent<ShowInstructionsEvent>().Subscribe(x => { _isInstructionsSelected = true; OnPropertyChanged("IsInstructionsSelected"); });
            ServiceFactory.Events.GetEvent<ShowSettingsEvent>().Subscribe(x => { _isSettingsSelected = true; OnPropertyChanged("IsSettingsSelected"); });

            DevicesModule.ViewModels.DevicesViewModel.UpdateGuardVisibility();
        }

        private void On_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                IsDevicesSelected = true;
            }
            catch { return; }
        }

        bool _isDevicesSelected;
        public bool IsDevicesSelected
        {
            get { return _isDevicesSelected; }
            set
            {
                _isDevicesSelected = value;
                if (value)
                    ServiceFactory.Events.GetEvent<ShowDeviceEvent>().Publish(Guid.Empty);
                OnPropertyChanged("IsDevicesSelected");
            }
        }

        bool _isZonesSelected;
        public bool IsZonesSelected
        {
            get { return _isZonesSelected; }
            set
            {
                _isZonesSelected = value;
                if (value)
                    ServiceFactory.Events.GetEvent<ShowZoneEvent>().Publish(0);
                OnPropertyChanged("IsZonesSelected");
            }
        }

        bool _isDerectonsSelected;
        public bool IsDerectonsSelected
        {
            get { return _isDerectonsSelected; }
            set
            {
                _isDerectonsSelected = value;
                if (value)
                    ServiceFactory.Events.GetEvent<ShowDirectionsEvent>().Publish(null);
                OnPropertyChanged("IsDerectonsSelected");
            }
        }

        bool _isGuardVisible;
        public bool IsGuardVisible
        {
            get { return _isGuardVisible; }
            set
            {
                _isGuardVisible = value;
                OnPropertyChanged("IsGuardVisible");
            }
        }

        bool _isGuardSelected;
        public bool IsGuardSelected
        {
            get { return _isGuardSelected; }
            set
            {
                _isGuardSelected = value;
                if (value)
                    ServiceFactory.Events.GetEvent<ShowGuardEvent>().Publish(null);
                OnPropertyChanged("IsGuardSelected");
            }
        }

        bool _isLibrarySelected;
        public bool IsLibrarySelected
        {
            get { return _isLibrarySelected; }
            set
            {
                _isLibrarySelected = value;
                if (value)
                    ServiceFactory.Events.GetEvent<ShowLibraryEvent>().Publish(null);
                OnPropertyChanged("IsLibrarySelected");
            }
        }

        bool _isPlanSelected;
        public bool IsPlanSelected
        {
            get { return _isPlanSelected; }
            set
            {
                _isPlanSelected = value;
                if (value)
                    ServiceFactory.Events.GetEvent<ShowPlansEvent>().Publish(null);
                OnPropertyChanged("IsPlanSelected");
            }
        }

        bool _isSecuritySelected;
        public bool IsSecuritySelected
        {
            get { return _isSecuritySelected; }
            set
            {
                _isSecuritySelected = value;
                OnPropertyChanged("IsSecuritySelected");
            }
        }

        bool _isUsersSelected;
        public bool IsUsersSelected
        {
            get { return _isUsersSelected; }
            set
            {
                _isUsersSelected = value;
                if (value)
                    ServiceFactory.Events.GetEvent<ShowUsersEvent>().Publish(null);
                OnPropertyChanged("IsUsersSelected");
            }
        }

        bool _isUserGroupsSelected;
        public bool IsUserGroupsSelected
        {
            get { return _isUserGroupsSelected; }
            set
            {
                _isUserGroupsSelected = value;
                if (value)
                    ServiceFactory.Events.GetEvent<ShowUserGroupsEvent>().Publish(null);
                OnPropertyChanged("IsUserGroupsSelected");
            }
        }

        bool _isJournalSelected;
        public bool IsJournalSelected
        {
            get { return _isJournalSelected; }
            set
            {
                _isJournalSelected = value;
                if (value)
                    ServiceFactory.Events.GetEvent<ShowJournalEvent>().Publish(null);
                OnPropertyChanged("IsJournalSelected");
            }
        }

        bool _isSoundsSelected;
        public bool IsSoundsSelected
        {
            get { return _isSoundsSelected; }
            set
            {
                _isSoundsSelected = value;
                if (value)
                    ServiceFactory.Events.GetEvent<ShowSoundsEvent>().Publish(null);
                OnPropertyChanged("IsSoundsSelected");
            }
        }

        bool _isInstructionsSelected;
        public bool IsInstructionsSelected
        {
            get { return _isInstructionsSelected; }
            set
            {
                _isInstructionsSelected = value;
                if (value)
                    ServiceFactory.Events.GetEvent<ShowInstructionsEvent>().Publish(null);
                OnPropertyChanged("IsInstructionsSelected");
            }
        }

        bool _isSettingsSelected;
        public bool IsSettingsSelected
        {
            get { return _isSettingsSelected; }
            set
            {
                _isSettingsSelected = value;
                if (value)
                    ServiceFactory.Events.GetEvent<ShowSettingsEvent>().Publish(null);
                OnPropertyChanged("IsSettingsSelected");
            }
        }

        bool _isGCSelected;
        public bool IsGCrSelected
        {
            get { return _isGCSelected; }
            set
            {
                _isGCSelected = value;
                OnPropertyChanged("IsGCrSelected");
            }
        }

        bool _isXDevicesSelected;
        public bool IsXDevicesSelected
        {
            get { return _isXDevicesSelected; }
            set
            {
                _isXDevicesSelected = value;
                if (value)
                    ServiceFactory.Events.GetEvent<ShowXDevicesEvent>().Publish(Guid.Empty);
                OnPropertyChanged("IsXDevicesSelected");
            }
        }

        bool _isXZonesSelected;
        public bool IsXZonesSelected
        {
            get { return _isXZonesSelected; }
            set
            {
                _isXZonesSelected = value;
                if (value)
                    ServiceFactory.Events.GetEvent<ShowXZonesEvent>().Publish(0);
                OnPropertyChanged("IsXZonesSelected");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        bool _isAccesRightsExpanded;
        public bool IsAccesRightsExpanded
        {
            get { return _isAccesRightsExpanded; }
            set
            {
                _isAccesRightsExpanded = value;
                OnPropertyChanged("IsAccesRightsExpanded");
            }
        }

        bool _isGuardExpanded;
        public bool IsGuardExpanded
        {
            get { return _isGuardExpanded; }
            set
            {
                _isGuardExpanded = value;
                OnPropertyChanged("IsGuardExpanded");
            }
        }

        bool _isGKExpanded;
        public bool IsGKExpanded
        {
            get { return _isGKExpanded; }
            set
            {
                _isGKExpanded = value;
                OnPropertyChanged("IsGKExpanded");
            }
        }

        void AccesRightsMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IsAccesRightsExpanded = IsAccesRightsExpanded != true;
        }

        void GuardMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IsGuardExpanded = IsGuardExpanded != true;
        }

        void GKMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IsGKExpanded = IsGKExpanded != true;
        }
    }
}