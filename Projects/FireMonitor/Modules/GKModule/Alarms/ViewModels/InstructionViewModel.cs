﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Common.Windows.ViewModels;
using XFiresecAPI;
using FiresecClient;
using Infrastructure.Common;

namespace GKModule.ViewModels
{
	public class InstructionViewModel : DialogViewModel
	{
		public InstructionViewModel(XDevice device, XZone zone, XAlarmType alarmType)
		{
			
			AlarmType = alarmType;

			Instruction = FindInstruction(device, zone);
			Title = Instruction != null ? Instruction.Name : "";
			HasContent = Instruction != null;
			if (Instruction != null)
			{
				Title += Instruction.Name;
			}
			CloseCommand = new RelayCommand(OnClose);
		}

		public bool HasContent { get; private set; }
		public XAlarmType AlarmType { get; private set; }
		public XStateBit StateType { get; private set; }
		public XInstruction Instruction { get; private set; }

		public RelayCommand CloseCommand { get; private set; }
		void OnClose()
		{
			Close();
		}

		XInstruction FindInstruction(XDevice device, XZone zone)
		{
			var availableStateTypeInstructions = XManager.DeviceConfiguration.Instructions.FindAll(x => x.AlarmType == AlarmType);

			if (device != null)
			{
				foreach (var instruction in availableStateTypeInstructions)
				{
					if (instruction.Devices.Contains(device.UID))
					{
						return instruction;
					}
				}
			}

			if (zone != null)
			{
				foreach (var instruction in availableStateTypeInstructions)
				{
					if (instruction.ZoneUIDs.Contains(zone.UID))
					{
						return instruction;
					}
				}
			}

			foreach (var instruction in availableStateTypeInstructions)
			{
				if (instruction.InstructionType == XInstructionType.General)
				{
					return instruction;
				}
			}

			return null;
		}
	}
}