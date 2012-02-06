﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
using FiresecAPI;

namespace FiresecService
{
    [ServiceBehavior(MaxItemsInObjectGraph = 2147483647, UseSynchronizationContext = true,
    InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SafeFiresecService : IFiresecService
    {
        public FiresecService FiresecService { get; set; }

        public SafeFiresecService()
        {
            FiresecService = new FiresecService();
        }

        public string Connect(string userName, string password)
        {
            try
            {
                return FiresecService.Connect(userName, password);
            }
            catch
            {
                return null;
            }
        }

        public string Reconnect(string userName, string password)
        {
            try
            {
                return FiresecService.Reconnect(userName, password);
            }
            catch
            {
                return null;
            }
        }

        public void Disconnect()
        {
            try
            {
                FiresecService.Disconnect();
            }
            catch
            {
            }
        }

        public void Subscribe()
        {
            try
            {
                FiresecService.Subscribe();
            }
            catch
            {
            }
        }

        public List<FiresecAPI.Models.Driver> GetDrivers()
        {
            try
            {
                return FiresecService.GetDrivers();
            }
            catch
            {
                return null;
            }
        }

        public FiresecAPI.Models.DeviceConfiguration GetDeviceConfiguration()
        {
            try
            {
                return FiresecService.GetDeviceConfiguration();
            }
            catch
            {
                return null;
            }
        }

        public void SetDeviceConfiguration(FiresecAPI.Models.DeviceConfiguration deviceConfiguration)
        {
            try
            {
                FiresecService.SetDeviceConfiguration(deviceConfiguration);
            }
            catch
            {
            }
        }

        public void DeviceWriteConfiguration(FiresecAPI.Models.DeviceConfiguration deviceConfiguration, Guid deviceUID)
        {
            try
            {
                FiresecService.DeviceWriteConfiguration(deviceConfiguration, deviceUID);
            }
            catch
            {
            }
        }

        public void DeviceWriteAllConfiguration(FiresecAPI.Models.DeviceConfiguration deviceConfiguration)
        {
            try
            {
                FiresecService.DeviceWriteAllConfiguration(deviceConfiguration);
            }
            catch
            {
            }
        }

        public void DeviceSetPassword(FiresecAPI.Models.DeviceConfiguration deviceConfiguration, Guid deviceUID, FiresecAPI.Models.DevicePasswordType devicePasswordType, string password)
        {
            try
            {
                FiresecService.DeviceSetPassword(deviceConfiguration, deviceUID, devicePasswordType, password);
            }
            catch
            {
            }
        }

        public void DeviceDatetimeSync(FiresecAPI.Models.DeviceConfiguration deviceConfiguration, Guid deviceUID)
        {
            try
            {
                FiresecService.DeviceDatetimeSync(deviceConfiguration, deviceUID);
            }
            catch
            {
            }
        }

        public void DeviceRestart(FiresecAPI.Models.DeviceConfiguration deviceConfiguration, Guid deviceUID)
        {
            try
            {
                FiresecService.DeviceRestart(deviceConfiguration, deviceUID);
            }
            catch
            {
            }
        }

        public string DeviceGetInformation(FiresecAPI.Models.DeviceConfiguration deviceConfiguration, Guid deviceUID)
        {
            try
            {
                return FiresecService.DeviceGetInformation(deviceConfiguration, deviceUID);
            }
            catch
            {
                return null;
            }
        }

        public List<string> DeviceGetSerialList(FiresecAPI.Models.DeviceConfiguration deviceConfiguration, Guid deviceUID)
        {
            try
            {
                return FiresecService.DeviceGetSerialList(deviceConfiguration, deviceUID);
            }
            catch
            {
                return null;
            }
        }

        public string DeviceUpdateFirmware(FiresecAPI.Models.DeviceConfiguration deviceConfiguration, Guid deviceUID, byte[] bytes, string fileName)
        {
            try
            {
                return FiresecService.DeviceUpdateFirmware(deviceConfiguration, deviceUID, bytes, fileName);
            }
            catch
            {
                return null;
            }
        }

        public string DeviceVerifyFirmwareVersion(FiresecAPI.Models.DeviceConfiguration deviceConfiguration, Guid deviceUID, byte[] bytes, string fileName)
        {
            try
            {
                return FiresecService.DeviceVerifyFirmwareVersion(deviceConfiguration, deviceUID, bytes, fileName);
            }
            catch
            {
                return null;
            }
        }

        public string DeviceReadEventLog(FiresecAPI.Models.DeviceConfiguration deviceConfiguration, Guid deviceUID)
        {
            try
            {
                return FiresecService.DeviceReadEventLog(deviceConfiguration, deviceUID);
            }
            catch
            {
                return null;
            }
        }

        public FiresecAPI.Models.DeviceConfiguration DeviceAutoDetectChildren(FiresecAPI.Models.DeviceConfiguration deviceConfiguration, Guid deviceUID, bool fastSearch)
        {
            try
            {
                return FiresecService.DeviceAutoDetectChildren(deviceConfiguration, deviceUID, fastSearch);
            }
            catch
            {
                return null;
            }
        }

        public FiresecAPI.Models.DeviceConfiguration DeviceReadConfiguration(FiresecAPI.Models.DeviceConfiguration deviceConfiguration, Guid deviceUID)
        {
            try
            {
                return FiresecService.DeviceReadConfiguration(deviceConfiguration, deviceUID);
            }
            catch
            {
                return null;
            }
        }

        public List<FiresecAPI.Models.DeviceCustomFunction> DeviceCustomFunctionList(Guid driverUID)
        {
            try
            {
                return FiresecService.DeviceCustomFunctionList(driverUID);
            }
            catch
            {
                return null;
            }
        }

        public string DeviceCustomFunctionExecute(FiresecAPI.Models.DeviceConfiguration deviceConfiguration, Guid deviceUID, string functionName)
        {
            try
            {
                return FiresecService.DeviceCustomFunctionExecute(deviceConfiguration, deviceUID, functionName);
            }
            catch
            {
                return null;
            }
        }

        public FiresecAPI.Models.SystemConfiguration GetSystemConfiguration()
        {
            try
            {
                return FiresecService.GetSystemConfiguration();
            }
            catch
            {
                return null;
            }
        }

        public void SetSystemConfiguration(FiresecAPI.Models.SystemConfiguration systemConfiguration)
        {
            try
            {
                FiresecService.SetSystemConfiguration(systemConfiguration);
            }
            catch
            {
            }
        }

        public FiresecAPI.Models.LibraryConfiguration GetLibraryConfiguration()
        {
            try
            {
                return FiresecService.GetLibraryConfiguration();
            }
            catch
            {
                return null;
            }
        }

        public void SetLibraryConfiguration(FiresecAPI.Models.LibraryConfiguration libraryConfiguration)
        {
            try
            {
                FiresecService.SetLibraryConfiguration(libraryConfiguration);
            }
            catch
            {
            }
        }

        public FiresecAPI.Models.PlansConfiguration GetPlansConfiguration()
        {
            try
            {
                return FiresecService.GetPlansConfiguration();
            }
            catch
            {
                return null;
            }
        }

        public void SetPlansConfiguration(FiresecAPI.Models.PlansConfiguration plansConfiguration)
        {
            try
            {
                FiresecService.SetPlansConfiguration(plansConfiguration);
            }
            catch
            {
            }
        }

        public FiresecAPI.Models.SecurityConfiguration GetSecurityConfiguration()
        {
            try
            {
                return FiresecService.GetSecurityConfiguration();
            }
            catch
            {
                return null;
            }
        }

        public void SetSecurityConfiguration(FiresecAPI.Models.SecurityConfiguration securityConfiguration)
        {
            try
            {
                FiresecService.SetSecurityConfiguration(securityConfiguration);
            }
            catch
            {
            }
        }

        public FiresecAPI.Models.DeviceConfigurationStates GetStates()
        {
            try
            {
                return FiresecService.GetStates();
            }
            catch
            {
                return null;
            }
        }

        public List<FiresecAPI.Models.JournalRecord> ReadJournal(int startIndex, int count)
        {
            try
            {
                return FiresecService.ReadJournal(startIndex, count);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<FiresecAPI.Models.JournalRecord> GetFilteredJournal(FiresecAPI.Models.JournalFilter journalFilter)
        {
            try
            {
                return FiresecService.GetFilteredJournal(journalFilter);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<FiresecAPI.Models.JournalRecord> GetFilteredArchive(FiresecAPI.Models.ArchiveFilter archiveFilter)
        {
            try
            {
                return FiresecService.GetFilteredArchive(archiveFilter);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<FiresecAPI.Models.JournalRecord> GetDistinctRecords()
        {
            try
            {
                return FiresecService.GetDistinctRecords();
            }
            catch
            {
                return null;
            }
        }

        public DateTime GetArchiveStartDate()
        {
            try
            {
                return FiresecService.GetArchiveStartDate();
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public void AddToIgnoreList(List<Guid> deviceUIDs)
        {
            try
            {
                FiresecService.AddToIgnoreList(deviceUIDs);
            }
            catch
            {
            }
        }

        public void RemoveFromIgnoreList(List<Guid> deviceUIDs)
        {
            try
            {
                FiresecService.RemoveFromIgnoreList(deviceUIDs);
            }
            catch
            {
            }
        }

        public void ResetStates(List<FiresecAPI.Models.ResetItem> resetItems)
        {
            try
            {
                FiresecService.ResetStates(resetItems);
            }
            catch
            {
            }
        }

        public void AddUserMessage(string message)
        {
            try
            {
                FiresecService.AddUserMessage(message);
            }
            catch
            {
            };
        }

        public void AddJournalRecord(FiresecAPI.Models.JournalRecord journalRecord)
        {
            try
            {
                FiresecService.AddJournalRecord(journalRecord);
            }
            catch
            {
            };
        }

        public void ExecuteCommand(Guid deviceUID, string methodName)
        {
            try
            {
                FiresecService.ExecuteCommand(deviceUID, methodName);
            }
            catch
            {
            };
        }

        public List<string> GetFileNamesList(string directory)
        {
            try
            {
                return FiresecService.GetFileNamesList(directory);
            }
            catch
            {
                return null;
            }
        }

        public Dictionary<string, string> GetDirectoryHash(string directory)
        {
            try
            {
                return FiresecService.GetDirectoryHash(directory);
            }
            catch
            {
                return null;
            }
        }

        public System.IO.Stream GetFile(string dirAndFileName)
        {
            try
            {
                return FiresecService.GetFile(dirAndFileName);
            }
            catch
            {
                return null;
            }
        }

        public string Ping()
        {
            try
            {
                return FiresecService.Ping();
            }
            catch
            {
                return null;
            }
        }

        public string Test()
        {
            try
            {
                return FiresecService.Test();
            }
            catch
            {
                return null;
            }
        }

        public void StopProgress()
        {
            try
            {
                FiresecService.StopProgress();
            }
            catch
            {
            };
        }
    }
}