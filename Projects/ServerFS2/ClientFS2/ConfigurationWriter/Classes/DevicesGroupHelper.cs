﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FiresecAPI.Models;
using System.Diagnostics;
using FiresecAPI.Models.Binary;

namespace ClientFS2.ConfigurationWriter.Classes
{
	public class DevicesGroupHelper
	{
		static int totalMiliseconds = 0;

		BinaryPanel BinaryPanel;
		Device ParentPanel;
		public List<DevicesGroup> DevicesGroups { get; set; }

		public DevicesGroupHelper(BinaryPanel binaryPanel)
		{
			var startDateTime = DateTime.Now;
			BinaryPanel = binaryPanel;
			ParentPanel = binaryPanel.ParentPanel;
			DevicesGroups = new List<DevicesGroup>();

			CreateDevicesGroup("Указатель на таблицу РМ", 0, DriverType.RM_1);
			CreateDevicesGroup("Указатель на таблицу МПТ", 0, DriverType.MPT);
			CreateDevicesGroup("Указатель на таблицу Дымовых", 12, DriverType.SmokeDetector);
			CreateDevicesGroup("Указатель на таблицу Тепловых", 0, DriverType.HeatDetector);
			CreateDevicesGroup("Указатель на таблицу Комбинированных", 0, DriverType.CombinedDetector);
			CreateDevicesGroup("Указатель на таблицу АМ-1", 0, DriverType.AM_1,
				DriverType.ShuzOffButton, DriverType.ShuzOnButton, DriverType.ShuzUnblockButton, DriverType.StartButton, DriverType.StopButton);
			CreateDevicesGroup("Указатель на таблицу ИПР", 0, DriverType.HandDetector);
			CreateDevicesGroup("Указатель на таблицу Охранных извещателей", 0, DriverType.AM1_O);
			var OuterDevices_Group = CreateDevicesGroup("Указатель на таблицу Внешних ИУ", 0, DriverType.Computer);
			OuterDevices_Group.IsRemoteDevicesPointer = true;
			CreateDevicesGroup("Указатель на таблицу МДУ", 0, DriverType.MDU);
			CreateDevicesGroup("Указатель на таблицу БУНС", 0, DriverType.PumpStation);
			CreateDevicesGroup("Указатель на таблицу АМП-4", 0, DriverType.AMP_4);
			CreateDevicesGroup("Указатель на таблицу ОЗ", 0, DriverType.Computer);
			CreateDevicesGroup("Указатель на таблицу Задвижек", 0, DriverType.Valve);
			CreateDevicesGroup("Указатель на таблицу АМ-Т", 0, DriverType.AM1_T);
			CreateDevicesGroup("Указатель на таблицу АМТ-4", 0, DriverType.AMT_4);
			CreateDevicesGroup("Указатель на таблицу ППУ", 0, DriverType.Computer);
			CreateDevicesGroup("Указатель на таблицу АСПТ", 0, DriverType.ASPT);
			CreateDevicesGroup("Указатель на таблицу МУК-1Э", 0, DriverType.Computer);
			CreateDevicesGroup("Указатель на таблицу Выход реле", 0, DriverType.Exit);
			CreateDevicesGroup("Указатель на таблицу радиоканальный ручной", 0, DriverType.RadioHandDetector);
			CreateDevicesGroup("Указатель на таблицу радиоканальный дымовой", 0, DriverType.RadioSmokeDetector);

			var deltaMiliseconds = (DateTime.Now - startDateTime).Milliseconds;
			totalMiliseconds += deltaMiliseconds;
			Trace.WriteLine("TotalMiliseconds=" + totalMiliseconds.ToString());
		}

		DevicesGroup CreateDevicesGroup(string name, int length, params DriverType[] driverTypes)
		{
			var devicesGroup = new DevicesGroup(name, length);
			foreach (var binaryDevice in BinaryPanel.BinaryLocalDevices)
			{
				if (driverTypes.Contains(binaryDevice.Device.Driver.DriverType))
				{
					devicesGroup.BinaryDevices.Add(binaryDevice);
				}
			}
			DevicesGroups.Add(devicesGroup);
			return devicesGroup;
		}
	}
}