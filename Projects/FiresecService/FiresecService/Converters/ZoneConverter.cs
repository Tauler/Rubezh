﻿using System.Collections.Generic;
using System.Linq;
using FiresecAPI.Models;
using System.Diagnostics;

namespace FiresecService.Converters
{
    public static class ZoneConverter
    {
        public static void Convert(Firesec.CoreConfig.config firesecConfig)
        {
            FiresecManager.DeviceConfiguration.Zones = new List<Zone>();
            FiresecManager.DeviceConfigurationStates.ZoneStates = new List<ZoneState>();

            if (firesecConfig.zone != null)
            {
                foreach (var innerZone in firesecConfig.zone)
                {
                    var zone = new Zone()
                    {
                        Name = innerZone.name,
                        No = innerZone.no,
                        Description = innerZone.desc
                    };

                    if (innerZone.shape != null)
                    {
                        zone.ShapeId = innerZone.shape[0].id;
                        Trace.WriteLine(zone.ShapeId);
                    }

                    if (innerZone.param != null)
                    {
                        var zoneTypeParam = innerZone.param.FirstOrDefault(x => x.name == "ZoneType");
                        if (zoneTypeParam != null)
                        {
                            zone.ZoneType = (zoneTypeParam.value == "0") ? ZoneType.Fire : ZoneType.Guard;
                        }

                        var exitTimeParam = innerZone.param.FirstOrDefault(x => x.name == "ExitTime");
                        if (exitTimeParam != null)
                            zone.EvacuationTime = exitTimeParam.value;

                        var fireDeviceCountParam = innerZone.param.FirstOrDefault(x => x.name == "FireDeviceCount");
                        if (fireDeviceCountParam != null)
                            zone.DetectorCount = fireDeviceCountParam.value;

                        var autoSetParam = innerZone.param.FirstOrDefault(x => x.name == "AutoSet");
                        if (autoSetParam != null)
                            zone.AutoSet = autoSetParam.value;

                        var delayParam = innerZone.param.FirstOrDefault(x => x.name == "Delay");
                        if (delayParam != null)
                            zone.Delay = delayParam.value;

                        var skippedParam = innerZone.param.FirstOrDefault(x => x.name == "Skipped");
                        if (skippedParam != null)
                            zone.Skipped = skippedParam.value == "1" ? true : false;

                        var guardZoneTypeParam = innerZone.param.FirstOrDefault(x => x.name == "GuardZoneType");
                        if (guardZoneTypeParam != null)
                        {
                            zone.GuardZoneType = (GuardZoneType)int.Parse(guardZoneTypeParam.value);
                        }
                    }
                    FiresecManager.DeviceConfiguration.Zones.Add(zone);
                    FiresecManager.DeviceConfigurationStates.ZoneStates.Add(new ZoneState() { No = zone.No });
                }
            }
        }

        public static void ConvertBack(DeviceConfiguration deviceConfiguration)
        {
            var innerZones = new List<Firesec.CoreConfig.zoneType>();
            foreach (var zone in deviceConfiguration.Zones)
            {
                var innerZone = new Firesec.CoreConfig.zoneType()
                {
                    name = zone.Name,
                    idx = zone.No,
                    no = zone.No
                };

                if (string.IsNullOrEmpty(zone.Description) == false)
                    innerZone.desc = zone.Description;

                var zoneParams = new List<Firesec.CoreConfig.paramType>();

                zoneParams.Add(new Firesec.CoreConfig.paramType()
                {
                    name = "ZoneType",
                    type = "Int",
                    value = (zone.ZoneType == ZoneType.Fire) ? "0" : "1"
                });

                if (string.IsNullOrEmpty(zone.DetectorCount) == false)
                {
                    zoneParams.Add(new Firesec.CoreConfig.paramType()
                    {
                        name = "FireDeviceCount",
                        type = "Int",
                        value = zone.DetectorCount
                    });
                }

                if (string.IsNullOrEmpty(zone.EvacuationTime) == false)
                {
                    zoneParams.Add(new Firesec.CoreConfig.paramType()
                    {
                        name = "ExitTime",
                        type = "SmallInt",
                        value = zone.EvacuationTime
                    });
                }

                if (string.IsNullOrEmpty(zone.AutoSet) == false)
                {
                    zoneParams.Add(new Firesec.CoreConfig.paramType()
                    {
                        name = "AutoSet",
                        type = "Int",
                        value = zone.AutoSet
                    });
                }

                if (string.IsNullOrEmpty(zone.Delay) == false)
                {
                    zoneParams.Add(new Firesec.CoreConfig.paramType()
                    {
                        name = "Delay",
                        type = "Int",
                        value = zone.Delay
                    });
                }

                if (true)
                {
                    zoneParams.Add(new Firesec.CoreConfig.paramType()
                    {
                        name = "Skipped",
                        type = "Int",
                        value = zone.Skipped ? "1" : "0"
                    });
                }

                string GuardZoneTypeParamString = ((int)zone.GuardZoneType).ToString();

                zoneParams.Add(new Firesec.CoreConfig.paramType()
                {
                    name = "GuardZoneType",
                    value = GuardZoneTypeParamString
                });

                if (zoneParams.Count > 0)
                    innerZone.param = zoneParams.ToArray();

                innerZones.Add(innerZone);
            }

            if (innerZones.Count > 0)
                FiresecManager.CoreConfig.zone = innerZones.ToArray();
        }
    }
}