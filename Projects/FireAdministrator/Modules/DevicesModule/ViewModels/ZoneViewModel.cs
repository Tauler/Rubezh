﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Common;
using FiresecClient;

namespace DevicesModule.ViewModels
{
    public class ZoneViewModel : BaseViewModel
    {
        public readonly Zone _zone;

        public ZoneViewModel(Zone zone)
        {
            _zone = zone;
            Update();
        }

        public void Update()
        {
            Name = _zone.Name;
            No = _zone.No;
            Description = _zone.Description;
            DetectorCount = _zone.DetectorCount;
            EvacuationTime = _zone.EvacuationTime;
        }

        string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        string _no;
        public string No
        {
            get { return _no; }
            set
            {
                _no = value;
                OnPropertyChanged("No");
            }
        }

        string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        string _detectorCount;
        public string DetectorCount
        {
            get { return _detectorCount; }
            set
            {
                _detectorCount = value;
                OnPropertyChanged("DetectorCount");
            }
        }

        string _evacuationTime;
        public string EvacuationTime
        {
            get { return _evacuationTime; }
            set
            {
                _evacuationTime = value;
                OnPropertyChanged("EvacuationTime");
            }
        }

        public string PresentationName
        {
            get { return No + "." + Name; }
        }
    }
}
