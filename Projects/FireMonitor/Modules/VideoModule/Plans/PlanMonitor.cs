﻿using System;
using System.Collections.Generic;
using System.Linq;
using FiresecAPI.GK;
using FiresecAPI.Models;
using Infrustructure.Plans.Presenter;
using VideoModule.Plans.Designer;

namespace VideoModule.Plans
{
	internal class PlanMonitor : BaseMonitor<Plan>
	{
		private List<XStateClass> _cameraStates;

		public PlanMonitor(Plan plan, Action callBack)
			: base(plan, callBack)
		{
			_cameraStates = new List<XStateClass>();
			Initialize();
		}

		private void Initialize()
		{
			foreach (var elementCamera in Plan.ElementExtensions.OfType<ElementCamera>())
			{
				var camera = Helper.GetCamera(elementCamera);
				if (camera != null)
				{
					_cameraStates.Add(camera.CameraStateStateClass);
					//camera.StateClass.StateChanged += _callBack;
				}
			}
		}
		public XStateClass GetState()
		{
			var result = XStateClass.No;
			foreach (var cameraState in _cameraStates)
				if (cameraState < result)
					result = cameraState;
			return result;
		}
	}
}