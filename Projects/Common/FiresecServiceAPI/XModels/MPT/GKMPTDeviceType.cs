﻿using System.ComponentModel;

namespace FiresecAPI.GK
{
	/// <summary>
	/// Тип устройства МПТ
	/// </summary>
	public enum GKMPTDeviceType
	{
		[DescriptionAttribute("Табличка Не входи")]
		DoNotEnterBoard,

		[DescriptionAttribute("Табличка Уходи")]
		ExitBoard,

		[DescriptionAttribute("Табличка Автоматика отключена")]
		AutomaticOffBoard,

		[DescriptionAttribute("Сирена")]
		Speaker,

		[DescriptionAttribute("Двери-окна")]
		Door,

		[DescriptionAttribute("Ручной запуск")]
		HandStart,

		[DescriptionAttribute("Ручной останов")]
		HandStop,

		[DescriptionAttribute("Ручное изменение режима автоматики")]
		HandAutomatic,

		[DescriptionAttribute("Пуск")]
		Bomb,

		[DescriptionAttribute("Неизвестно")]
		Unknown
	}
}