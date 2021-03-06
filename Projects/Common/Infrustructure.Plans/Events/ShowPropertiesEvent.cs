﻿using Infrustructure.Plans.Elements;
using Microsoft.Practices.Prism.Events;

namespace Infrustructure.Plans.Events
{
	public class ShowPropertiesEvent : CompositePresentationEvent<ShowPropertiesEventArgs>
	{
	}
	public class ShowPropertiesEventArgs
	{
		public ShowPropertiesEventArgs(ElementBase element)
		{
			Element = element;
		}

		public ElementBase Element { get; private set; }
		public object PropertyViewModel { get; set; }
	}
}
