﻿using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using DeviceControls;
using FiresecAPI;
using FiresecAPI.Models;
using FiresecClient;
using Infrastructure;
using Infrustructure.Plans;
using Infrustructure.Plans.Elements;
using Infrustructure.Plans.Events;
using Infrustructure.Plans.Presenter;
using SKDModule.Events;
using SKDModule.Plans.Designer;
using XFiresecAPI;

namespace SKDModule.Plans
{
	class PlanPresenter : IPlanPresenter<Plan, XStateClass>
	{
		private Dictionary<Plan, PlanMonitor> _monitors;
		public PlanPresenter()
		{
			ServiceFactory.Events.GetEvent<ShowSKDDeviceOnPlanEvent>().Subscribe(OnShowSKDDeviceOnPlan);
			ServiceFactory.Events.GetEvent<ShowSKDZoneOnPlanEvent>().Subscribe(OnShowSKDZoneOnPlan);
			ServiceFactory.Events.GetEvent<PainterFactoryEvent>().Unsubscribe(OnPainterFactoryEvent);
			ServiceFactory.Events.GetEvent<PainterFactoryEvent>().Subscribe(OnPainterFactoryEvent);
			_monitors = new Dictionary<Plan, PlanMonitor>();
		}

		#region IPlanPresenter<Plan> Members

		public void SubscribeStateChanged(Plan plan, Action callBack)
		{
			Helper.BuildMap();
			if (_monitors.ContainsKey(plan))
				_monitors[plan].AddCallBack(callBack);
			else
				_monitors.Add(plan, new PlanMonitor(plan, callBack));
		}

		public XStateClass GetState(Plan plan)
		{
			return _monitors.ContainsKey(plan) ? _monitors[plan].GetState() : XStateClass.No;
		}

		public IEnumerable<ElementBase> LoadPlan(Plan plan)
		{
			foreach (var element in plan.ElementSKDDevices.Where(x => x.DeviceUID != Guid.Empty))
				yield return element;
			foreach (var element in plan.ElementRectangleSKDZones.Where(x => x.ZoneUID != Guid.Empty && !x.IsHidden))
				yield return element;
			foreach (var element in plan.ElementPolygonSKDZones.Where(x => x.ZoneUID != Guid.Empty && !x.IsHidden))
				yield return element;
		}

		public void RegisterPresenterItem(PresenterItem presenterItem)
		{
			if (presenterItem.Element is ElementSKDDevice)
				presenterItem.OverridePainter(new SKDDevicePainter(presenterItem));
			else if (presenterItem.Element is ElementPolygonSKDZone || presenterItem.Element is ElementRectangleSKDZone)
				presenterItem.OverridePainter(new SKDZonePainter(presenterItem));
		}
		public void ExtensionAttached()
		{
			using (new TimeCounter("SKDDevice.ExtensionAttached.BuildMap: {0}"))
				Helper.BuildMap();
		}

		#endregion

		public void Initialize()
		{
			_monitors.Clear();
			using (new TimeCounter("DevicePictureCache.LoadSKDCache: {0}"))
				PictureCacheSource.SKDDevicePicture.LoadCache();
			using (new TimeCounter("DevicePictureCache.LoadSKDDynamicCache: {0}"))
				PictureCacheSource.SKDDevicePicture.LoadDynamicCache();
		}

		private void OnPainterFactoryEvent(PainterFactoryEventArgs args)
		{
		}

		private void OnShowSKDDeviceOnPlan(SKDDevice device)
		{
			foreach (var plan in FiresecManager.PlansConfiguration.AllPlans)
				foreach (var element in plan.ElementSKDDevices)
					if (element.DeviceUID == device.UID)
					{
						ServiceFactory.Events.GetEvent<NavigateToPlanElementEvent>().Publish(new NavigateToPlanElementEventArgs(plan.UID, element.UID));
						return;
					}
		}
		private void OnShowSKDZoneOnPlan(SKDZone zone)
		{
			foreach (var plan in FiresecManager.PlansConfiguration.AllPlans)
			{
				foreach (var element in plan.ElementRectangleSKDZones)
					if (element.ZoneUID == zone.UID)
					{
						ServiceFactory.Events.GetEvent<NavigateToPlanElementEvent>().Publish(new NavigateToPlanElementEventArgs(plan.UID, element.UID));
						return;
					}
				foreach (var element in plan.ElementPolygonSKDZones)
					if (element.ZoneUID == zone.UID)
					{
						ServiceFactory.Events.GetEvent<NavigateToPlanElementEvent>().Publish(new NavigateToPlanElementEventArgs(plan.UID, element.UID));
						return;
					}
			}
		}
	}
}