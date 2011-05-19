﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DeviceLibrary;

namespace DeviceControls
{
    public partial class DeviceControl : INotifyPropertyChanged
    {
        public DeviceControl()
        {
            InitializeComponent();
            DataContext = this;
            StateCanvases = new ObservableCollection<Canvas>();
        }
        
        public string DriverId { get; set; }

        private string _state;
        public string State
        {
            get { return _state; }
            set
            {
                _state = value;
                Update();
            }
        }

        private List<string> _additionalStates;
        public List<string> AdditionalStates
        {
            get { return _additionalStates; }
            set
            {
                _additionalStates = value;
                Update();
            }
        }

        private ObservableCollection<Canvas> _stateCanvases;
        public ObservableCollection<Canvas> StateCanvases
        {
            get { return _stateCanvases; }
            set
            {
                _stateCanvases = value;
                OnPropertyChanged("StateCanvases");
            }
        }

        private List<StateViewModel> _stateViewModelList;

        private void Update()
        {
            if (_stateViewModelList != null)
                _stateViewModelList.ForEach(x => x.Dispose());
            _stateViewModelList = new List<StateViewModel>();

            var device = LibraryManager.Devices.FirstOrDefault(x => x.Id == DriverId);
            StateCanvases = new ObservableCollection<Canvas>();
            var state = device.States.FirstOrDefault(x => (x.Id == State) && (Convert.ToInt32(x.Id) >= 0) && (!x.IsAdditional));
            if (state != null)
            {
                _stateViewModelList.Add(new StateViewModel(state, StateCanvases));
                if (AdditionalStates == null) return;
                foreach (var additionalStateId in AdditionalStates)
                {
                    var aState = device.States.FirstOrDefault(x => (x.Id == additionalStateId) && (x.IsAdditional));
                    _stateViewModelList.Add(new StateViewModel(aState, StateCanvases));
                }
            }
            else
                foreach (var additionalStateId in AdditionalStates)
                {
                    var aState = device.States.FirstOrDefault(x => (x.Id == additionalStateId) && (x.IsAdditional));
                    _stateViewModelList.Add(new StateViewModel(aState, StateCanvases));
                }
        }

        private void UserControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _itemsControl.LayoutTransform = new ScaleTransform(ActualWidth / 500, ActualHeight / 500);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}