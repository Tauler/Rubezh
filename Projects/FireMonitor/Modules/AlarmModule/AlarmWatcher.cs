﻿using System;
using System.Linq;
using AlarmModule.Events;
using FiresecAPI.Models;
using FiresecClient;
using Infrastructure;
using Infrastructure.Events;

namespace AlarmModule
{
    public class AlarmWatcher
    {
        public AlarmWatcher()
        {
            FiresecEventSubscriber.DeviceStateChangedEvent += new Action<string>(OnDeviceStateChangedEvent);
            DeviceState.AlarmAdded += new Action<AlarmType, string>(DeviceState_AlarmAdded);
            DeviceState.AlarmRemoved += new Action<AlarmType, string>(DeviceState_AlarmRemoved);
            Update();
        }

        static void Update()
        {
            //Microsoft.Maintainability : 'AlarmWatcher.Update()' имеет сложность организации циклов 30.
            //Напишите заново или реструктурируйте метод, чтобы уменьшить сложность до 25.

            foreach (var deviceState in FiresecManager.DeviceStates.DeviceStates)
            {
                deviceState.IsFire = deviceState.States.Any(x => x.IsActive && x.DriverState.CanResetOnPanel && x.DriverState.StateType == StateType.Fire);
                deviceState.IsAttention = deviceState.States.Any(x => x.IsActive && x.DriverState.CanResetOnPanel && x.DriverState.StateType == StateType.Attention);
                deviceState.IsInfo = deviceState.States.Any(x => x.IsActive && x.DriverState.CanResetOnPanel && x.DriverState.StateType == StateType.Info);
                deviceState.IsOff = deviceState.States.Any(x => x.IsActive && x.DriverState.StateType == StateType.Off);

                deviceState.IsFailure = deviceState.States.Any(x => x.IsActive && x.DriverState.IsManualReset && x.DriverState.StateType == StateType.Failure);
                deviceState.IsService = deviceState.States.Any(x => x.IsActive && x.DriverState.IsManualReset && x.DriverState.StateType == StateType.Service && x.DriverState.IsAutomatic == false);
                deviceState.IsAutomaticOff = deviceState.States.Any(x => x.IsActive && x.DriverState.IsManualReset && x.DriverState.IsAutomatic);
            }
        }

        void DeviceState_AlarmAdded(AlarmType alarmType, string id)
        {
            //Microsoft.Performance : 'AlarmWatcher.DeviceState_AlarmAdded(AlarmType, string)' объявляет переменную 'deviceState' типа 'DeviceState',
            //которая никогда не используется или которой только присваивается значение. Используйте эту переменную, или удалите ее.

            var device = FiresecManager.DeviceConfiguration.Devices.FirstOrDefault(x => x.Id == id);
            var deviceState = FiresecManager.DeviceStates.DeviceStates.FirstOrDefault(x => x.Id == id);
            var alarm = new Alarm()
            {
                AlarmType = alarmType,
                DeviceId = id,
                Name = EnumsConverter.AlarmToString(alarmType) + ". Устройство " + device.Driver.Name + " - " + device.PresentationAddress,
                Time = DateTime.Now.ToString()
            };
            ServiceFactory.Events.GetEvent<AlarmAddedEvent>().Publish(alarm);

            if (alarmType == AlarmType.Fire)
            {
                ServiceFactory.Events.GetEvent<ShowDeviceOnPlanEvent>().Publish(id);
            }
        }

        void DeviceState_AlarmRemoved(AlarmType alarmType, string id)
        {
            Alarm alarm = new Alarm()
            {
                AlarmType = alarmType,
                DeviceId = id
            };
            ServiceFactory.Events.GetEvent<ResetAlarmEvent>().Publish(alarm);
        }

        void OnDeviceStateChangedEvent(string obj)
        {
            Update();
        }
    }
}