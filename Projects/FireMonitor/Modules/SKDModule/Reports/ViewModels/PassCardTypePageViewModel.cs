﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Common.SKDReports;
using FiresecAPI.SKD.ReportFilters;

namespace SKDModule.Reports.ViewModels
{
	public class PassCardTypePageViewModel : FilterContainerViewModel
	{
		public PassCardTypePageViewModel()
		{
			Title = "Типы пропусков";
		}

		private bool _passCardActive;
		public bool PassCardActive
		{
			get { return _passCardActive; }
			set
			{
				_passCardActive = value;
				OnPropertyChanged(() => PassCardActive);
				if (PassCardActive)
				{
					PassCardPermanent = true;
					PassCardTemprorary = true;
					PassCardOnceOnly = true;
					PassCardForcing = true;
					PassCardLocked = true;
				}
			}
		}
		private bool _passCardInactive;
		public bool PassCardInactive
		{
			get { return _passCardInactive; }
			set
			{
				_passCardInactive = value;
				OnPropertyChanged(() => PassCardInactive);
			}
		}
		private bool _passCardPermanent;
		public bool PassCardPermanent
		{
			get { return _passCardPermanent; }
			set
			{
				_passCardPermanent = value;
				OnPropertyChanged(() => PassCardPermanent);
			}
		}
		private bool _passCardTemprorary;
		public bool PassCardTemprorary
		{
			get { return _passCardTemprorary; }
			set
			{
				_passCardTemprorary = value;
				OnPropertyChanged(() => PassCardTemprorary);
			}
		}
		private bool _passCardOnceOnly;
		public bool PassCardOnceOnly
		{
			get { return _passCardOnceOnly; }
			set
			{
				_passCardOnceOnly = value;
				OnPropertyChanged(() => PassCardOnceOnly);
			}
		}
		private bool _passCardForcing;
		public bool PassCardForcing
		{
			get { return _passCardForcing; }
			set
			{
				_passCardForcing = value;
				OnPropertyChanged(() => PassCardForcing);
			}
		}
		private bool _passCardLocked;
		public bool PassCardLocked
		{
			get { return _passCardLocked; }
			set
			{
				_passCardLocked = value;
				OnPropertyChanged(() => PassCardLocked);
			}
		}

		private bool _allowInactive;
		public bool AllowInactive
		{
			get { return _allowInactive; }
			set
			{
				_allowInactive = value;
				OnPropertyChanged(() => AllowInactive);
			}
		}

		public override void LoadFilter(SKDReportFilter filter)
		{
			var passCardTypeFilter = filter as IReportFilterPassCardType;
			if (passCardTypeFilter == null)
				return;
			PassCardActive = passCardTypeFilter.PassCardActive;
			PassCardPermanent = passCardTypeFilter.PassCardPermanent;
			PassCardTemprorary = passCardTypeFilter.PassCardTemprorary;
			PassCardOnceOnly = passCardTypeFilter.PassCardOnceOnly;
			PassCardForcing = passCardTypeFilter.PassCardForcing;
			PassCardLocked = passCardTypeFilter.PassCardLocked;
			var fullPassCardTypeFilter = passCardTypeFilter as IReportFilterPassCardTypeFull;
			AllowInactive = fullPassCardTypeFilter != null;
			if (AllowInactive)
				PassCardInactive = fullPassCardTypeFilter.PassCardInactive;
		}
		public override void UpdateFilter(SKDReportFilter filter)
		{
			var passCardTypeFilter = filter as IReportFilterPassCardType;
			if (passCardTypeFilter == null)
				return;
			passCardTypeFilter.PassCardActive = PassCardActive;
			passCardTypeFilter.PassCardPermanent = PassCardPermanent;
			passCardTypeFilter.PassCardTemprorary = PassCardTemprorary;
			passCardTypeFilter.PassCardOnceOnly = PassCardOnceOnly;
			passCardTypeFilter.PassCardForcing = PassCardForcing;
			passCardTypeFilter.PassCardLocked = PassCardLocked;
			var fullPassCardTypeFilter = passCardTypeFilter as IReportFilterPassCardTypeFull;
			if (fullPassCardTypeFilter != null)
				fullPassCardTypeFilter.PassCardInactive = PassCardInactive;
		}
	}
}
