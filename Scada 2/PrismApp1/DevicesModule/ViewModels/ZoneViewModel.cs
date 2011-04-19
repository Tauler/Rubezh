﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;
using DevicesModule.Events;
using ClientApi;

namespace DevicesModule.ViewModels
{
    public class ZoneViewModel : BaseViewModel
    {
        public ZoneViewModel()
        {
            SelectCommand = new RelayCommand(OnSelect);
        }

        Zone zone;

        public void Initialize(Zone zone)
        {
            this.zone = zone;
            ZoneState zoneState = ServiceClient.CurrentStates.ZoneStates.FirstOrDefault(x => x.No == zone.No);
            State = zoneState.State;
        }

        public RelayCommand SelectCommand { get; private set; }
        void OnSelect()
        {
            ServiceFactory.Events.GetEvent<ZoneSelectedEvent>().Publish(zone.No);
        }

        public string Name
        {
            get { return zone.Name; }
        }

        public string No
        {
            get { return zone.No; }
        }

        public string Description
        {
            get { return zone.Description; }
        }

        public string DetectorCount
        {
            get { return zone.DetectorCount; }
        }

        public string EvacuationTime
        {
            get { return zone.EvacuationTime; }
        }

        public string PresentationName
        {
            get { return zone.No + "." + zone.Name; }
        }

        string state;
        public string State
        {
            get { return state; }
            set
            {
                state = value;
                OnPropertyChanged("State");
            }
        }
    }
}
