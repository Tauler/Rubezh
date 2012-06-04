﻿using System.Collections.Generic;
using System.Linq;
using FiresecAPI.Models;
using FiresecClient;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Events;
using Infrastructure.Common.Windows.ViewModels;

namespace PlansModule.ViewModels
{
	public class ZonePropertiesViewModel : SaveCancelDialogViewModel
    {
        IElementZone IElementZone;

        public ZonePropertiesViewModel(IElementZone iElementZone)
        {
            IElementZone = iElementZone;
            CreateCommand = new RelayCommand(OnCreate);
            Title = "Свойства фигуры: Зона";
            Zones = new List<Zone>(FiresecManager.DeviceConfiguration.Zones);
            if (iElementZone.ZoneNo.HasValue)
                SelectedZone = Zones.FirstOrDefault(x => x.No == iElementZone.ZoneNo.Value);
        }

        public List<Zone> Zones { get; private set; }

        Zone _selectedZone;
        public Zone SelectedZone
        {
            get { return _selectedZone; }
            set
            {
                _selectedZone = value;
                OnPropertyChanged("SelectedZone");
            }
        }

        public RelayCommand CreateCommand { get; private set; }
        void OnCreate()
        {
            var createZoneEventArg = new CreateZoneEventArg();
            ServiceFactory.Events.GetEvent<CreateZoneEvent>().Publish(createZoneEventArg);
            IElementZone.ZoneNo = createZoneEventArg.ZoneNo;
            if (createZoneEventArg.Cancel == false)
                Close(true);
        }

		protected override bool Save()
		{
            if (SelectedZone != null)
            {
                IElementZone.ZoneNo = SelectedZone.No;
                IElementZone.Zone = SelectedZone;
            }
			return base.Save();
		}
    }
}
