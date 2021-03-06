﻿using FiresecAPI.Models;
using FiresecAPI.SKD;
using Infrastructure.Common.Windows;
using Infrustructure.Plans.Designer;
using Infrustructure.Plans.InstrumentAdorners;
using AutomationModule.ViewModels;
using FiresecAPI.Automation;
using Infrustructure.Plans.Elements;
using AutomationModule.Plans.ViewModels;

namespace AutomationModule.Plans.InstrumentAdorners
{
	public class ProcedureRectangleAdorner : BaseRectangleAdorner
	{
		private ProceduresViewModel _proceduresViewModel;
		public ProcedureRectangleAdorner(CommonDesignerCanvas designerCanvas, ProceduresViewModel proceduresViewModel)
			: base(designerCanvas)
		{
			_proceduresViewModel = proceduresViewModel;
		}

		protected override ElementBaseRectangle CreateElement()
		{
			var element = new ElementProcedure();
			var propertiesViewModel = new ProcedurePropertiesViewModel(element, _proceduresViewModel);
			if (!DialogService.ShowModalWindow(propertiesViewModel))
				return null;
			AutomationPlanExtension.Instance.SetItem<Procedure>(element);
			return element;
		}
	}
}