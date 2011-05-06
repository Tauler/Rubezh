﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeviveModelManager
{
    public static class ZoneHelper
    {
        public static TreeItem CreateZone()
        {
            TreeItem zoneTreeItem = new TreeItem();
            zoneTreeItem.ModelInfo = new Assad.modelInfoType();
            zoneTreeItem.ModelInfo.type1 = "rubezh." + ViewModel.StaticVersion + "." + "zone";
            zoneTreeItem.ModelInfo.model = "1.0";
            zoneTreeItem.ModelInfo.name = "Зона";

            List<Assad.modelInfoTypeEvent> events = new List<Assad.modelInfoTypeEvent>();
            events.Add(new Assad.modelInfoTypeEvent() { @event = "Тревога" });
            events.Add(new Assad.modelInfoTypeEvent() { @event = "Внимание (предтревожное)" });
            events.Add(new Assad.modelInfoTypeEvent() { @event = "Неисправность" });
            events.Add(new Assad.modelInfoTypeEvent() { @event = "Требуется обслуживание" });
            events.Add(new Assad.modelInfoTypeEvent() { @event = "Обход устройств" });
            events.Add(new Assad.modelInfoTypeEvent() { @event = "Неопределено" });
            events.Add(new Assad.modelInfoTypeEvent() { @event = "Норма(*)" });
            events.Add(new Assad.modelInfoTypeEvent() { @event = "Норма" });
            events.Add(new Assad.modelInfoTypeEvent() { @event = "Отсутствует в конфигурации сервера оборудования" });
            events.Add(new Assad.modelInfoTypeEvent() { @event = "Нет связи с сервером оборудования" });
            zoneTreeItem.ModelInfo.@event = events.ToArray();

            List<Assad.modelInfoTypeCommand> commands = new List<Assad.modelInfoTypeCommand>();
            commands.Add(new Assad.modelInfoTypeCommand() { command = "Функция с зоной" });
            zoneTreeItem.ModelInfo.command = commands.ToArray();

            List<Assad.modelInfoTypeParam> parameters = new List<Assad.modelInfoTypeParam>();
            parameters.Add(new Assad.modelInfoTypeParam() { param = "Номер зоны", type = "edit" });
            parameters.Add(new Assad.modelInfoTypeParam() { param = "Наименование", type = "edit" });
            parameters.Add(new Assad.modelInfoTypeParam() { param = "Число датчиков для формирования сигнала Пожар", type = "edit" });
            parameters.Add(new Assad.modelInfoTypeParam() { param = "Время эвакуации", type = "edit" });
            parameters.Add(new Assad.modelInfoTypeParam() { param = "Примечание", type = "edit" });

            Assad.modelInfoTypeParam zoneTypeParam = new Assad.modelInfoTypeParam() { param = "Назначение зоны", type = "single" };
            List<Assad.modelInfoTypeParamValue> zoneTypeValues = new List<Assad.modelInfoTypeParamValue>();
            zoneTypeValues.Add(new Assad.modelInfoTypeParamValue() { value = "Пожарная" });
            zoneTypeValues.Add(new Assad.modelInfoTypeParamValue() { value = "Охранная" });
            zoneTypeParam.value = zoneTypeValues.ToArray();
            parameters.Add(zoneTypeParam);

            zoneTreeItem.ModelInfo.param = parameters.ToArray();

            zoneTreeItem.ModelInfo.state = new Assad.modelInfoTypeState[1];
            zoneTreeItem.ModelInfo.state[0] = new Assad.modelInfoTypeState();
            zoneTreeItem.ModelInfo.state[0].state = "Состояние";
            List<Assad.modelInfoTypeStateValue> StateValues = new List<Assad.modelInfoTypeStateValue>();
            StateValues.Add(new Assad.modelInfoTypeStateValue() { value = "Тревога" });
            StateValues.Add(new Assad.modelInfoTypeStateValue() { value = "Внимание (предтревожное)" });
            StateValues.Add(new Assad.modelInfoTypeStateValue() { value = "Неисправность" });
            StateValues.Add(new Assad.modelInfoTypeStateValue() { value = "Требуется обслуживание" });
            StateValues.Add(new Assad.modelInfoTypeStateValue() { value = "Обход устройств" });
            StateValues.Add(new Assad.modelInfoTypeStateValue() { value = "Неопределено" });
            StateValues.Add(new Assad.modelInfoTypeStateValue() { value = "Норма(*)" });
            StateValues.Add(new Assad.modelInfoTypeStateValue() { value = "Норма" });
            StateValues.Add(new Assad.modelInfoTypeStateValue() { value = "Отсутствует в конфигурации сервера оборудования" });
            StateValues.Add(new Assad.modelInfoTypeStateValue() { value = "Нет связи с сервером оборудования" });
            zoneTreeItem.ModelInfo.state[0].value = StateValues.ToArray();

            return zoneTreeItem;
        }
    }
}