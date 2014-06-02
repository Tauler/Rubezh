﻿using System.Collections.Generic;
using FiresecAPI.SKD;
using Infrastructure.Common.Windows.ViewModels;

namespace SKDModule.ViewModels
{
	public class OrganisationZonesViewModel : BaseViewModel
	{
		public Organisation Organisation { get; private set; }

		public OrganisationZonesViewModel(Organisation organisation)
		{
			Organisation = organisation;
			AllZones = new List<OrganisationZoneViewModel>();
			RootZone = AddZoneInternal(SKDManager.SKDConfiguration.RootZone, null);
			SelectedZone = RootZone;

			foreach (var zone in AllZones)
			{
				zone.ExpandToThis();
				if (organisation.ZoneUIDs.Contains(zone.Zone.UID))
					zone._isChecked = true;
			}
		}

		#region Zones
		public List<OrganisationZoneViewModel> AllZones;

		OrganisationZoneViewModel _selectedZone;
		public OrganisationZoneViewModel SelectedZone
		{
			get { return _selectedZone; }
			set
			{
				_selectedZone = value;
				if (value != null)
					value.ExpandToThis();
				OnPropertyChanged("SelectedZone");
			}
		}

		OrganisationZoneViewModel _rootZone;
		public OrganisationZoneViewModel RootZone
		{
			get { return _rootZone; }
			private set
			{
				_rootZone = value;
				OnPropertyChanged("RootZone");
			}
		}

		public OrganisationZoneViewModel[] RootZones
		{
			get { return new OrganisationZoneViewModel[] { RootZone }; }
		}

		OrganisationZoneViewModel AddZoneInternal(SKDZone zone, OrganisationZoneViewModel parentZoneViewModel)
		{
			var zoneViewModel = new OrganisationZoneViewModel(Organisation, zone);
			AllZones.Add(zoneViewModel);
			if (parentZoneViewModel != null)
				parentZoneViewModel.AddChild(zoneViewModel);

			foreach (var childZone in zone.Children)
			{
				AddZoneInternal(childZone, zoneViewModel);
			}
			return zoneViewModel;
		}
		#endregion
	}
}