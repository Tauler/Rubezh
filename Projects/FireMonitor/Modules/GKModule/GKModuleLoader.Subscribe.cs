﻿using System;
using System.Collections.Generic;
using System.Linq;
using FiresecAPI;
using FiresecAPI.GK;
using FiresecClient;
using GKProcessor;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.Services;
using Infrastructure.Common.Windows;
using Infrastructure.Events;

namespace GKModule
{
	public partial class GKModuleLoader
	{
		void SubscribeGK()
		{
			if (!GlobalSettingsHelper.GlobalSettings.IsGKAsAService)
			{
				GKProcessorManager.Suspend();
			}
			XManager.UpdateConfiguration();
			XManager.CreateStates();
			DescriptorsManager.Create();
			DescriptorsManager.CreateDynamicObjectsInXManager();
			InitializeStates();
			if (!GlobalSettingsHelper.GlobalSettings.IsGKAsAService)
			{
				GKProcessorManager.MustMonitor = true;
				GKProcessorManager.Start();
				GKLicenseProcessor.Start();
			}
		}

		void GKAfterInitialize()
		{
			SafeFiresecService.GKProgressCallbackEvent -= new Action<GKProgressCallback>(OnGKProgressCallbackEvent);
			SafeFiresecService.GKProgressCallbackEvent += new Action<GKProgressCallback>(OnGKProgressCallbackEvent);

			SafeFiresecService.GKCallbackResultEvent -= new Action<GKCallbackResult>(OnGKCallbackResult);
			SafeFiresecService.GKCallbackResultEvent += new Action<GKCallbackResult>(OnGKCallbackResult);

			GKProcessorManager.GKProgressCallbackEvent -= new Action<GKProgressCallback>(OnGKProgressCallbackEvent);
			GKProcessorManager.GKProgressCallbackEvent += new Action<GKProgressCallback>(OnGKProgressCallbackEvent);

			GKProcessorManager.GKCallbackResultEvent -= new Action<GKCallbackResult>(OnGKCallbackResult);
			GKProcessorManager.GKCallbackResultEvent += new Action<GKCallbackResult>(OnGKCallbackResult);

			SafeFiresecService.GetFilteredGKArchiveCompletedEvent -= new Action<IEnumerable<XJournalItem>, Guid>(OnGetFilteredGKArchiveCompletedEvent);
			SafeFiresecService.GetFilteredGKArchiveCompletedEvent += new Action<IEnumerable<XJournalItem>, Guid>(OnGetFilteredGKArchiveCompletedEvent);

			ServiceFactoryBase.Events.GetEvent<GKObjectsStateChangedEvent>().Publish(null);
		}

		void OnGetFilteredGKArchiveCompletedEvent(IEnumerable<XJournalItem> journalItems, Guid archivePortionUID)
		{
			ApplicationService.Invoke(() =>
			{
				var archiveResult = new GKArchiveResult()
				{
					ArchivePortionUID = archivePortionUID,
					JournalItems = journalItems
				};
				ServiceFactory.Events.GetEvent<GetFilteredGKArchiveCompletedEvent>().Publish(archiveResult);
			});
		}

		void InitializeStates()
		{
			var gkStates = FiresecManager.FiresecService.GKGetStates();
			CopyGKStates(gkStates);
		}

		void OnGKProgressCallbackEvent(GKProgressCallback gkProgressCallback)
		{
			ApplicationService.Invoke(() =>
			{
				switch (gkProgressCallback.GKProgressCallbackType)
				{
					case GKProgressCallbackType.Start:
						if (gkProgressCallback.GKProgressClientType == GKProgressClientType.Monitor)
						{
							LoadingService.Show(gkProgressCallback.Title, gkProgressCallback.Text, gkProgressCallback.StepCount, gkProgressCallback.CanCancel);
						}
						return;

					case GKProgressCallbackType.Progress:
						if (gkProgressCallback.GKProgressClientType == GKProgressClientType.Monitor)
						{
							LoadingService.DoStep(gkProgressCallback.Text, gkProgressCallback.Title, gkProgressCallback.StepCount, gkProgressCallback.CanCancel);
							if (LoadingService.IsCanceled)
								FiresecManager.FiresecService.CancelGKProgress(gkProgressCallback.UID, FiresecManager.CurrentUser.Name);
						}
						return;

					case GKProgressCallbackType.Stop:
						LoadingService.Close();
						return;
				}
			});
		}

		void OnGKCallbackResult(GKCallbackResult gkCallbackResult)
		{
			ApplicationService.Invoke(() =>
			{
				if (gkCallbackResult.JournalItems.Count > 0)
				{
					ServiceFactory.Events.GetEvent<NewXJournalEvent>().Publish(gkCallbackResult.JournalItems);
				}
				CopyGKStates(gkCallbackResult.GKStates);
				ServiceFactoryBase.Events.GetEvent<GKObjectsStateChangedEvent>().Publish(null);
			});
		}

		void CopyGKStates(GKStates gkStates)
		{
			foreach (var remoteDeviceState in gkStates.DeviceStates)
			{
				var device = XManager.Devices.FirstOrDefault(x => x.UID == remoteDeviceState.UID);
				if (device != null)
				{
					remoteDeviceState.CopyTo(device.State);
					device.State.OnStateChanged();
				}
			}
			foreach (var remoteZoneState in gkStates.ZoneStates)
			{
				var zone = XManager.Zones.FirstOrDefault(x => x.UID == remoteZoneState.UID);
				if (zone != null)
				{
					remoteZoneState.CopyTo(zone.State);
					zone.State.OnStateChanged();
				}
			}
			foreach (var remoteDirectionState in gkStates.DirectionStates)
			{
				var direction = XManager.Directions.FirstOrDefault(x => x.UID == remoteDirectionState.UID);
				if (direction != null)
				{
					remoteDirectionState.CopyTo(direction.State);
					direction.State.OnStateChanged();
				}
			}
			foreach (var remotePumpStationState in gkStates.PumpStationStates)
			{
				var pumpStation = XManager.PumpStations.FirstOrDefault(x => x.UID == remotePumpStationState.UID);
				if (pumpStation != null)
				{
					remotePumpStationState.CopyTo(pumpStation.State);
					pumpStation.State.OnStateChanged();
				}
			}
			foreach (var remoteMPTState in gkStates.MPTStates)
			{
				var mpt = XManager.MPTs.FirstOrDefault(x => x.UID == remoteMPTState.UID);
				if (mpt != null)
				{
					remoteMPTState.CopyTo(mpt.State);
					mpt.State.OnStateChanged();
				}
			}
			foreach (var delayState in gkStates.DelayStates)
			{
				var delay = XManager.Delays.FirstOrDefault(x => x.UID == delayState.UID);
				if (delay != null)
				{
					delayState.CopyTo(delay.State);
					delay.State.OnStateChanged();
				}
				else
				{
					delay = XManager.AutoGeneratedDelays.FirstOrDefault(x => x.UID == delayState.UID);
					if (delay == null)
						delay = XManager.AutoGeneratedDelays.FirstOrDefault(x => x.PresentationName == delayState.PresentationName);
					if (delay != null)
					{
						delayState.CopyTo(delay.State);
						delay.State.OnStateChanged();
					}
				}
			}
			foreach (var remotePimState in gkStates.PimStates)
			{
				var pim = XManager.AutoGeneratedPims.FirstOrDefault(x => x.UID == remotePimState.UID);
				if (pim == null)
					pim = XManager.AutoGeneratedPims.FirstOrDefault(x => x.PresentationName == remotePimState.PresentationName);
				if (pim != null)
				{
					remotePimState.CopyTo(pim.State);
					pim.State.OnStateChanged();
				}
			}
			foreach (var remoteGuardZoneState in gkStates.GuardZoneStates)
			{
				var guardZone = XManager.GuardZones.FirstOrDefault(x => x.UID == remoteGuardZoneState.UID);
				if (guardZone != null)
				{
					remoteGuardZoneState.CopyTo(guardZone.State);
					guardZone.State.OnStateChanged();
				}
			}
			foreach (var deviceMeasureParameter in gkStates.DeviceMeasureParameters)
			{
				var device = XManager.Devices.FirstOrDefault(x => x.UID == deviceMeasureParameter.DeviceUID);
				if (device != null)
				{
					device.State.XMeasureParameterValues = deviceMeasureParameter.MeasureParameterValues;
					device.State.OnMeasureParametersChanged();
				}
			}
		}
	}
}