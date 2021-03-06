﻿using System;
using System.Collections.Generic;
using System.Linq;
using ChinaSKDDriver;
using Common;
using FiresecAPI;
using FiresecAPI.GK;
using FiresecAPI.Journal;
using FiresecAPI.SKD;
using SKDDriver;
using SKDDriver.Translators;

namespace FiresecService
{
	public static class SKDProcessor
	{
		static SKDProcessor()
		{
#if DEBUG
			try
			{
				System.IO.File.Copy(@"..\..\..\ChinaController\CPPWrapper\Bin\CPPWrapper.dll", @"CPPWrapper.dll", true);
			}
			catch { }
#endif
			SKDDaylyUpdateProcessor.Start();
		}

		public static void Start()
		{
			try
			{
				if (SKDManager.SKDConfiguration != null)
				{
					SKDManager.CreateDrivers();
					SKDManager.UpdateConfiguration();
					SKDManager.CreateStates();
				}
				ChinaSKDDriver.Processor.Start();
				foreach (var deviceProcessor in ChinaSKDDriver.Processor.DeviceProcessors)
				{
					deviceProcessor.NewJournalItem -= new Action<JournalItem>(OnNewJournalItem);
					deviceProcessor.NewJournalItem += new Action<JournalItem>(OnNewJournalItem);

					deviceProcessor.ConnectionAppeared -= new Action<DeviceProcessor>(OnConnectionAppeared);
					deviceProcessor.ConnectionAppeared += new Action<DeviceProcessor>(OnConnectionAppeared);
				}

				ChinaSKDDriver.Processor.NewJournalItem -= new Action<JournalItem>(OnNewJournalItem);
				ChinaSKDDriver.Processor.NewJournalItem += new Action<JournalItem>(OnNewJournalItem);

				ChinaSKDDriver.Processor.StatesChangedEvent -= new Action<SKDStates>(OnSKDStates);
				ChinaSKDDriver.Processor.StatesChangedEvent += new Action<SKDStates>(OnSKDStates);

				ChinaSKDDriver.Processor.GKProgressCallbackEvent -= new Action<GKProgressCallback>(OnGKProgressCallbackEvent);
				ChinaSKDDriver.Processor.GKProgressCallbackEvent += new Action<GKProgressCallback>(OnGKProgressCallbackEvent);
			}
			catch (Exception e)
			{
				Logger.Error(e, "SKDProcessor.Create");
			}
		}

		public static void Stop()
		{
			ChinaSKDDriver.Processor.Stop();
		}

		static void OnNewJournalItem(JournalItem journalItem)
		{
			var cardNo = 0;
			var journalDetalisationItem = journalItem.JournalDetalisationItems.FirstOrDefault(x => x.Name == "Номер карты");
			if (journalDetalisationItem != null)
			{
				var cardNoString = journalDetalisationItem.Value;
				Int32.TryParse(cardNoString, System.Globalization.NumberStyles.HexNumber, null, out cardNo);
			}

			if (cardNo > 0)
			{
				using (var databaseService = new SKDDatabaseService())
				{
					var operationResult = databaseService.CardTranslator.GetEmployeeByCardNo(cardNo);
					if (!operationResult.HasError)
					{
						var employeeUID = operationResult.Result;
						journalItem.EmployeeUID = employeeUID;
						if (employeeUID != Guid.Empty)
						{
							var employee = databaseService.EmployeeTranslator.GetSingle(employeeUID);
							if (employee != null)
							{
								journalItem.UserName = employee.Result.Name;
							}
						}

						if (journalItem.JournalEventNameType == JournalEventNameType.Проход_разрешен)
						{
							var readerdevice = SKDManager.Devices.FirstOrDefault(x => x.UID == journalItem.ObjectUID);
							if (readerdevice != null && readerdevice.Zone != null)
							{
								var zoneUID = readerdevice.Zone.UID;
								using (var passJournalTranslator = new PassJournalTranslator())
								{
									passJournalTranslator.AddPassJournal(employeeUID, zoneUID);
								}
							}
						}
					}
				}
			}

			journalItem.JournalSubsystemType = EventDescriptionAttributeHelper.ToSubsystem(journalItem.JournalEventNameType);
			FiresecService.Service.FiresecService.AddCommonJournalItem(journalItem);
		}

		static void OnSKDStates(SKDStates skdStates)
		{
			if (skdStates.DeviceStates.Count > 0)
			{
				foreach (var zone in SKDManager.Zones)
				{
					var stateClasses = GetZoneStateClasses(zone);

					var hasChanges = stateClasses.Count != zone.State.StateClasses.Count;
					if (!hasChanges)
					{
						foreach (var stateClass in stateClasses)
						{
							if (!zone.State.StateClasses.Contains(stateClass))
								hasChanges = true;
						}
					}

					if (hasChanges)
					{
						zone.State.StateClasses = stateClasses;
						zone.State.StateClass = zone.State.StateClasses.Min();
						skdStates.ZoneStates.Add(zone.State);
					}
				}
				foreach (var door in SKDManager.Doors)
				{
					var stateClasses = GetDoorStateClasses(door);

					var hasChanges = stateClasses.Count != door.State.StateClasses.Count;
					if (!hasChanges)
					{
						foreach (var stateClass in stateClasses)
						{
							if (!door.State.StateClasses.Contains(stateClass))
								hasChanges = true;
						}
					}

					if (hasChanges)
					{
						door.State.StateClasses = stateClasses;
						door.State.StateClass = door.State.StateClasses.Min();
						skdStates.DoorStates.Add(door.State);
					}
				}
			}

			FiresecService.Service.FiresecService.NotifySKDObjectStateChanged(skdStates);
			ProcedureRunner.RunOnStateChanged();
		}

		static List<XStateClass> GetZoneStateClasses(SKDZone zone)
		{
			var stateClasses = new List<XStateClass>();
			foreach (var readerDevice in zone.Devices)
			{
				var lockAddress = readerDevice.IntAddress;
				if (readerDevice.Parent != null && readerDevice.Parent.DoorType == DoorType.TwoWay)
				{
					lockAddress = readerDevice.IntAddress / 2;
				}
				var lockDevice = readerDevice.Parent.Children.FirstOrDefault(x => x.DriverType == SKDDriverType.Lock && x.IntAddress == lockAddress);
				if (lockDevice != null)
				{
					if (!stateClasses.Contains(lockDevice.State.StateClass))
						stateClasses.Add(lockDevice.State.StateClass);
				}
			}
			stateClasses.Sort();
			if (stateClasses.Count == 0)
				stateClasses.Add(XStateClass.Unknown);
			return stateClasses;
		}

		static List<XStateClass> GetDoorStateClasses(SKDDoor door)
		{
			var stateClasses = new List<XStateClass>();

			if (door.InDevice != null)
			{
				var lockAddress = door.InDevice.IntAddress;
				if (door.DoorType == DoorType.TwoWay)
				{
					lockAddress = door.InDevice.IntAddress / 2;
				}
				var lockDevice = door.InDevice.Parent.Children.FirstOrDefault(x => x.DriverType == SKDDriverType.Lock && x.IntAddress == lockAddress);
				if (lockDevice != null)
				{
					if (!stateClasses.Contains(lockDevice.State.StateClass))
						stateClasses.Add(lockDevice.State.StateClass);
				}
			}

			stateClasses.Sort();
			if (stateClasses.Count == 0)
				stateClasses.Add(XStateClass.Unknown);
			return stateClasses;
		}

		public static SKDStates SKDGetStates()
		{
			var skdStates = new SKDStates();
			foreach (var device in SKDManager.Devices)
			{
				skdStates.DeviceStates.Add(device.State);
			}
			foreach (var zone in SKDManager.Zones)
			{
				zone.State.StateClasses = GetZoneStateClasses(zone);
				zone.State.StateClass = zone.State.StateClasses.Min();
				skdStates.ZoneStates.Add(zone.State);
			}
			foreach (var door in SKDManager.Doors)
			{
				door.State.StateClasses = GetDoorStateClasses(door);
				door.State.StateClass = door.State.StateClasses.Min();
				skdStates.DoorStates.Add(door.State);
			}
			return skdStates;
		}

		static void OnGKProgressCallbackEvent(GKProgressCallback gkProgressCallback)
		{
			FiresecService.Service.FiresecService.NotifyGKProgress(gkProgressCallback);
		}

		static void OnConnectionAppeared(DeviceProcessor deviceProcessor)
		{
			using (var databaseService = new SKDDatabaseService())
			{
				var pendingCards = databaseService.CardTranslator.GetAllPendingCards(deviceProcessor.Device.UID);
				foreach (var pendingCard in pendingCards)
				{
					var operationResult = databaseService.CardTranslator.GetSingle(pendingCard.CardUID);
					if (!operationResult.HasError && operationResult.Result != null)
					{
						var card = operationResult.Result;
						var getAccessTemplateOperationResult = databaseService.AccessTemplateTranslator.GetSingle(card.AccessTemplateUID);
						var cardWriter = new CardWriter();
						if ((PendingCardAction)pendingCard.Action == PendingCardAction.Add)
						{
							cardWriter = ChinaSKDDriver.Processor.AddCard(card, getAccessTemplateOperationResult.Result);
						}
						if ((PendingCardAction)pendingCard.Action == PendingCardAction.Edit)
						{
							cardWriter = ChinaSKDDriver.Processor.DeleteCard(card, getAccessTemplateOperationResult.Result);
							cardWriter = ChinaSKDDriver.Processor.AddCard(card, getAccessTemplateOperationResult.Result);
						}
						if ((PendingCardAction)pendingCard.Action == PendingCardAction.Add)
						{
							cardWriter = ChinaSKDDriver.Processor.DeleteCard(card, getAccessTemplateOperationResult.Result);
						}
						foreach (var controllerCardItem in cardWriter.ControllerCardItems)
						{
							if (!controllerCardItem.HasError)
							{
								databaseService.CardTranslator.DeleteAllPendingCards(pendingCard.CardUID, deviceProcessor.Device.UID);
							}
						}
					}
					else
					{
						databaseService.CardTranslator.DeleteAllPendingCards(pendingCard.CardUID, deviceProcessor.Device.UID);
					}
				}
			}
		}

		public static void SetNewConfig()
		{
			Stop();
			Start();
		}
	}
}