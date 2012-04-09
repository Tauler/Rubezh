﻿using System;
using System.Linq;
using Controls.MessageBox;
using FiresecClient;
using Infrastructure;
using FiresecAPI;

namespace DevicesModule.ViewModels
{
    public static class SynchronizeDeviceHelper
    {
        static Guid _deviceUID;
        static bool _isUsb;
        static OperationResult<bool> _operationResult;

        public static void Run(Guid deviceUID, bool isUsb)
        {
            _deviceUID = deviceUID;
            _isUsb = isUsb;

            var device = FiresecManager.DeviceConfiguration.Devices.FirstOrDefault(x => x.UID == _deviceUID);
            ServiceFactory.ProgressService.Run(OnPropgress, OnCompleted, device.PresentationAddressDriver + ". Установка времени");
        }

        static void OnPropgress()
        {
            _operationResult = FiresecManager.SynchronizeDevice(_deviceUID, _isUsb);
        }

        static void OnCompleted()
        {
            if (_operationResult.HasError)
            {
                MessageBoxService.ShowDeviceError("Ошибка при выполнении операции", _operationResult.Error);
                return;
            }
            MessageBoxService.Show("Операция завершилась успешно");
        }
    }
}
