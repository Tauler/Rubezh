﻿using Controls.MessageBox;
using FiresecAPI.Models;
using FiresecClient;
using Infrastructure;
using FiresecAPI;

namespace DevicesModule.ViewModels
{
    public static class DeviceGetMDS5DataHelper
    {
        static Device _device;
        static OperationResult<string> _operationResult;

        public static void Run(Device device)
        {
            _device = device;
            ServiceFactory.ProgressService.Run(OnPropgress, OnCompleted, _device.PresentationAddressDriver + ". Получение данных с модуля доставки сообщений");
        }

        static void OnPropgress()
        {
            _operationResult = FiresecManager.DeviceGetMDS5Data(_device.UID);
        }

        static void OnCompleted()
        {
            if (_operationResult.HasError)
            {
                MessageBoxService.ShowDeviceError("Ошибка при выполнении операции", _operationResult.Error);
                return;
            }
            MessageBoxService.Show(_operationResult.Result);
        }
    }
}
