﻿namespace FSAgentServer
{
	internal class FiresecExceptionHelper
	{
		public static bool IsWellKnownNormalException(string name)
		{
			switch (name)
			{
				case "Операция прервана":
				case "Прибор не отвечает":
				case "Обновление завершилось неудачно. Повторите обновление":
				case "Операция записи БД окончилась неудачей. Повторите запись":
				case "Управление устройством невозможно. Нет связи с прибором":
				case "USB устройство отсутствует":
				case "Версия 5 базы в блоке индикации не поддерживается. Обновите FireSec":
				case "Устройство относится к нескольким направлениям тушения":
				case "Версия 3 базы в ПДУ не поддерживается. Обновите FireSec":
					return true;
			}
			if (name.StartsWith("Предотвращена возможная взаимная блокировка"))
				return true;
			return false;
		}

		public static bool IsWellKnownComException(string name)
		{
			switch (name)
			{
				case "Разрушительный сбой":
				case "Не могу преобразовать вариант типа (Array Variant) в тип (OleStr)":
				case "Член группы не найден":
				case "Ошибка":
				case "Ошибка записи потока":
				case "Неверный аргумент":
                case "Catastrophic failure":
				case "ERR: Адрес (номер параметра, подфункция) является недопустимым":
				case "Указанный прибор не имеет модели базы данных":
				case "Ошибка на сервере":
				case "Ошибка при системном вызове":
					return true;
			}
			if (name.StartsWith("Предотвращена возможная взаимная блокировка"))
				return true;
			return false;
		}

		public static bool IsWellKnownInvalidComObjectException(string name)
		{
			switch (name)
			{
				case "Объект COM, который был отделен от своего базового RCW, использоваться не может.":
					return true;
			}
			return false;
		}

		public static bool IsWellKnownTargetParameterCountException(string name)
		{
			switch (name)
			{
				case "Недопустимое число параметров":
					return true;
			}
			return false;
		}
	}
}