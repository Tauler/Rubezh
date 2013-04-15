﻿using System;
using System.Collections.Generic;
using Common;
using DevicesModule.Validation;
using DevicesModule.ViewModels;
using FiresecAPI;
using FiresecAPI.Models;
using FiresecClient;
using Infrastructure;
using Infrastructure.Client;
using Infrastructure.Common;
using Infrastructure.Common.Navigation;
using Infrastructure.Common.Validation;
using Infrastructure.Common.Windows;
using Infrastructure.Events;
using Infrustructure.Plans.Events;
using DevicesModule.Plans;
using DevicesModule.Plans.Designer;

namespace DevicesModule
{
	public class DevicesModule : ModuleBase, IValidationModule
	{
		private NavigationItem _guardNavigationItem;
		private DevicesViewModel DevicesViewModel;
		private ZonesViewModel ZonesViewModel;
		private DirectionsViewModel DirectionsViewModel;
		private GuardViewModel GuardViewModel;
		private PlanExtension _planExtension;

		public override void CreateViewModels()
		{
			ServiceFactory.Events.GetEvent<CreateZoneEvent>().Subscribe(OnCreateZone);
			ServiceFactory.Events.GetEvent<EditZoneEvent>().Subscribe(OnEditZone);
			ServiceFactory.Events.GetEvent<ShowDeviceEvent>().Subscribe(OnShowDevice);

			DevicesViewModel = new DevicesViewModel();
			ZonesViewModel = new ZonesViewModel();
			DirectionsViewModel = new DirectionsViewModel();
			GuardViewModel = new GuardViewModel();
			_planExtension = new PlanExtension(DevicesViewModel, ZonesViewModel);
		}

		void OnCreateZone(CreateZoneEventArg createZoneEventArg)
		{
			ZonesViewModel.CreateZone(createZoneEventArg);
		}
		void OnEditZone(Guid zoneUID)
		{
			ZonesViewModel.EditZone(zoneUID);
		}
		void OnShowDevice(Guid deviceUID)
		{
			DevicesViewModel.Select(deviceUID);
		}

		public override void RegisterResource()
		{
			ServiceFactory.ResourceService.AddResource(new ResourceDescription(GetType().Assembly, "Devices/DataTemplates/Dictionary.xaml"));
			ServiceFactory.ResourceService.AddResource(new ResourceDescription(GetType().Assembly, "Zones/DataTemplates/Dictionary.xaml"));
			ServiceFactory.ResourceService.AddResource(new ResourceDescription(GetType().Assembly, "Directions/DataTemplates/Dictionary.xaml"));
			ServiceFactory.ResourceService.AddResource(new ResourceDescription(GetType().Assembly, "Guard/DataTemplates/Dictionary.xaml"));
			ServiceFactory.ResourceService.AddResource(new ResourceDescription(GetType().Assembly, "Plans/DataTemplates/Dictionary.xaml"));
		}
		public override void Initialize()
		{
			DevicesViewModel.Initialize();
			ZonesViewModel.Initialize();
			DirectionsViewModel.Initialize();
			GuardViewModel.Initialize();
			_planExtension.Initialize();

			ServiceFactory.Events.GetEvent<RegisterPlanExtensionEvent<Plan>>().Publish(_planExtension);
			Helper.BuildMap();
		}
		public override IEnumerable<NavigationItem> CreateNavigation()
		{
			_guardNavigationItem = new NavigationItem<ShowGuardEvent>(GuardViewModel, "Охрана", "/Controls;component/Images/user.png") { IsVisible = false };
			ServiceFactory.Events.GetEvent<GuardVisibilityChangedEvent>().Subscribe(x => { _guardNavigationItem.IsVisible = x; });

			return new List<NavigationItem>()
			{
				new NavigationItem<ShowDeviceEvent, Guid>(DevicesViewModel, "Устройства","/Controls;component/Images/tree.png", null, null, Guid.Empty),
				new NavigationItem<ShowZoneEvent, Guid>(ZonesViewModel, "Зоны","/Controls;component/Images/zones.png", null, null, Guid.Empty),
				new NavigationItem<ShowDirectionsEvent, Guid>(DirectionsViewModel, "Направления","/Controls;component/Images/direction.png", null, null, Guid.Empty),
				_guardNavigationItem
			};
		}
		public override string Name
		{
			get { return "Устройства, Зоны, Направления"; }
		}

		#region IValidationModule Members

		public IEnumerable<IValidationError> Validate()
		{
			var devicesValidator = new Validator(FiresecManager.FiresecConfiguration);
			return devicesValidator.Validate();
		}

		#endregion

		public override bool BeforeInitialize(bool firstTime)
		{
			try
			{
				LoadingService.DoStep("Загрузка драйвера устройств");
				var connectionResult = FiresecManager.InitializeFiresecDriver(false);
				if (connectionResult.HasError)
				{
					MessageBoxService.ShowError(connectionResult.Error);
					return false;
				}
				LoadingService.DoStep("Синхронизация конфигурации");
				FiresecManager.FiresecDriver.Synchronyze(false);
				LoadingService.DoStep("Старт мониторинга");
				FiresecManager.FiresecDriver.StartWatcher(false, false);
				FiresecManager.FSAgent.Start();
			}
			catch (FiresecException e)
			{
				Logger.Error(e, "DevicesModule.BeforeInitialize");
				MessageBoxService.ShowError(e.Message);
				return false;
            }
#if RELEASE
					if (LoadingErrorManager.HasError)
						MessageBoxService.ShowWarning(LoadingErrorManager.ToString(), "Ошибки при загрузке драйвера FireSec");
#endif
            return true;
		}
	}
}