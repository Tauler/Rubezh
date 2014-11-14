﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using FiresecAPI.GK;
using FiresecAPI.Models;
using FiresecAPI.Models.Layouts;
using FiresecClient;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.Windows;
using Infrastructure.Common.Windows.ViewModels;
using Infrastructure.Events;
using Infrustructure.Plans;
using Infrustructure.Plans.Events;
using FiresecAPI.AutomationCallback;
using System.Collections.ObjectModel;
using Infrustructure.Plans.Elements;
using FiresecAPI.Automation;
using System.Windows.Media;

namespace PlansModule.ViewModels
{
	public class PlansViewModel : ViewPartViewModel
	{
		private bool _initialized;
		private LayoutPartPlansProperties _properties;
		public List<IPlanPresenter<Plan, XStateClass>> PlanPresenters { get; private set; }
		public PlanTreeViewModel PlanTreeViewModel { get; private set; }
		public PlanDesignerViewModel PlanDesignerViewModel { get; private set; }

		public PlansViewModel(List<IPlanPresenter<Plan, XStateClass>> planPresenters)
			: this(planPresenters, new LayoutPartPlansProperties() { Type = LayoutPartPlansType.All })
		{
		}
		public PlansViewModel(List<IPlanPresenter<Plan, XStateClass>> planPresenters, LayoutPartPlansProperties properties)
		{
			_properties = properties ?? new LayoutPartPlansProperties() { Type = LayoutPartPlansType.All };
			PlanPresenters = planPresenters;
			ServiceFactory.Events.GetEvent<NavigateToPlanElementEvent>().Subscribe(OnNavigate);
			ServiceFactory.Events.GetEvent<ShowElementEvent>().Subscribe(OnShowElement);
			ServiceFactory.Events.GetEvent<FindElementEvent>().Subscribe(OnFindElementEvent);
			ServiceFactory.Events.GetEvent<SelectPlanEvent>().Subscribe(OnSelectPlan);
			_initialized = false;
			if (_properties.Type != LayoutPartPlansType.Single)
			{
				PlanTreeViewModel = new PlanTreeViewModel(this, _properties.Type == LayoutPartPlansType.Selected ? _properties.Plans : null);
				PlanTreeViewModel.SelectedPlanChanged += SelectedPlanChanged;
				var planNavigationWidth = RegistrySettingsHelper.GetDouble("Monitor.Plans.SplitterDistance");
				if (planNavigationWidth == 0)
					planNavigationWidth = 100;
				PlanNavigationWidth = new GridLength(planNavigationWidth, GridUnitType.Pixel);
				ApplicationService.ShuttingDown += () =>
				{
					RegistrySettingsHelper.SetDouble("Monitor.Plans.SplitterDistance", PlanNavigationWidth.Value);
				};
			}
			else
				PlanNavigationWidth = GridLength.Auto;
			PlanDesignerViewModel = new PlanDesignerViewModel(this);
		}

		public void Initialize()
		{
			_initialized = false;
			if (PlanTreeViewModel != null)
				PlanTreeViewModel.Initialize();
			_initialized = true;
			OnSelectedPlanChanged();
			SafeFiresecService.AutomationEvent -= OnAutomationCallback;
			SafeFiresecService.AutomationEvent += OnAutomationCallback;
		}

		private void OnSelectPlan(Guid planUID)
		{
			if (PlanTreeViewModel != null)
			{
				var newPlan = PlanTreeViewModel.FindPlan(planUID);
				if (PlanTreeViewModel.SelectedPlan == newPlan)
					PlanDesignerViewModel.Update();
				else
					PlanTreeViewModel.SelectedPlan = newPlan;
			}
		}
		private void SelectedPlanChanged(object sender, EventArgs e)
		{
			OnSelectedPlanChanged();
		}
		private void OnSelectedPlanChanged()
		{
			if (_initialized)
			{
				if (PlanTreeViewModel != null)
					PlanDesignerViewModel.SelectPlan(PlanTreeViewModel.SelectedPlan);
				else if (_properties != null && _properties.Plans.Count > 0)
				{
					var plan = FiresecManager.PlansConfiguration.AllPlans.FirstOrDefault(item => item.UID == _properties.Plans[0]);
					if (plan != null)
					{
						var planViewModel = new PlanViewModel(this, plan);
						PlanPresenters.ForEach(planPresenter => planViewModel.RegisterPresenter(planPresenter));
						PlanDesignerViewModel.SelectPlan(planViewModel);
					}
				}
			}
		}

		private void OnShowElement(Guid elementUID)
		{
			foreach (var presenterItem in PlanDesignerViewModel.PresenterItems)
				if (presenterItem.Element.UID == elementUID)
				{
					presenterItem.Navigate();
					PlanDesignerViewModel.Navigate(presenterItem);
				}
		}
		private void OnFindElementEvent(List<Guid> deviceUIDs)
		{
			if (PlanTreeViewModel != null)
			{
				foreach (var plan in PlanTreeViewModel.AllPlans)
					if (plan.PlanFolder == null && FindElementOnPlan(plan, deviceUIDs))
						return;
			}
			else
				FindElementOnPlan(PlanDesignerViewModel.PlanViewModel, deviceUIDs);
		}
		private bool FindElementOnPlan(PlanViewModel plan, List<Guid> deviceUIDs)
		{
			foreach (var element in plan.Plan.ElementUnion)
				if (deviceUIDs.Contains(element.UID))
				{
					PlanTreeViewModel.SelectedPlan = plan;
					OnShowElement(element.UID);
					return true;
				}
			return false;
		}
		private void OnNavigate(NavigateToPlanElementEventArgs args)
		{
			//Debug.WriteLine("[{0}]Navigation: PlanUID={1}\t\tElementUID={2}", DateTime.Now, args.PlanUID, args.ElementUID);
			ServiceFactory.Events.GetEvent<ShowPlansEvent>().Publish(null);
			OnSelectPlan(args.PlanUID);
			OnShowElement(args.ElementUID);
		}

		public bool IsPlanTreeVisible
		{
			get { return !GlobalSettingsHelper.GlobalSettings.Monitor_HidePlansTree && PlanTreeViewModel != null; }
		}

		public override void OnShow()
		{
			base.OnShow();
			foreach (var planPresenter in PlanPresenters)
				planPresenter.ExtensionAttached();
			if (PlanTreeViewModel != null)
				PlanTreeViewModel.Select();
		}

		GridLength _planNavigationWidth;
		public GridLength PlanNavigationWidth
		{
			get { return _planNavigationWidth; }
			set
			{
				_planNavigationWidth = value;
				OnPropertyChanged(() => PlanNavigationWidth);
			}
		}

		private void OnAutomationCallback(AutomationCallbackResult automationCallbackResult)
		{
			ApplicationService.Invoke(() =>
			{
				switch (automationCallbackResult.AutomationCallbackType)
				{
					case AutomationCallbackType.SetPlanProperty:
						var planArguments = (PlanCallbackData)automationCallbackResult.Data;
						var plan = PlanTreeViewModel == null ? (PlanDesignerViewModel.Plan.UID == planArguments.PlanUid ? PlanDesignerViewModel.PlanViewModel : null) : PlanTreeViewModel.Plans.FirstOrDefault(x => x.Plan.UID == planArguments.PlanUid);
						if (plan != null)
						{
							var elementBase = plan.Plan.SimpleElements.FirstOrDefault(x => x.UID == planArguments.ElementUid);
							switch (planArguments.ElementPropertyType)
							{
								case ElementPropertyType.Color:
									elementBase.BorderColor = (Color)planArguments.Value;
									break;
								case ElementPropertyType.BackColor:
									elementBase.BackgroundColor = (Color)planArguments.Value;
									break;
								case ElementPropertyType.BorderThickness:
									elementBase.BorderThickness = Convert.ToDouble(planArguments.Value);
									break;
							}
							var elementRectangle = elementBase as ElementBaseRectangle;
							if (elementRectangle != null)
								switch (planArguments.ElementPropertyType)
								{
									case ElementPropertyType.Height:
										elementRectangle.Height = Convert.ToDouble(planArguments.Value);
										break;
									case ElementPropertyType.Width:
										elementRectangle.Width = Convert.ToDouble(planArguments.Value);
										break;
									case ElementPropertyType.Left:
										elementRectangle.Left = Convert.ToDouble(planArguments.Value);
										break;
									case ElementPropertyType.Top:
										elementRectangle.Top = Convert.ToDouble(planArguments.Value);
										break;
								}
							var elementText = elementBase as IElementTextBlock;
							if (elementText != null)
								switch (planArguments.ElementPropertyType)
								{
									case ElementPropertyType.FontBold:
										elementText.FontBold = Convert.ToBoolean(planArguments.Value);
										break;
									case ElementPropertyType.FontItalic:
										elementText.FontItalic = Convert.ToBoolean(planArguments.Value);
										break;
									case ElementPropertyType.FontSize:
										elementText.FontSize = Convert.ToDouble(planArguments.Value);
										break;
									case ElementPropertyType.ForegroundColor:
										elementText.ForegroundColor = (Color)planArguments.Value;
										break;
									case ElementPropertyType.Stretch:
										elementText.Stretch = Convert.ToBoolean(planArguments.Value);
										break;
									case ElementPropertyType.Text:
										elementText.Text = Convert.ToString(planArguments.Value);
										break;
									case ElementPropertyType.WordWrap:
										elementText.WordWrap = Convert.ToBoolean(planArguments.Value);
										break;
								}
							var presenterItem = PlanDesignerViewModel.PresenterItems.FirstOrDefault(item => item.Element == elementBase);
							if (presenterItem != null)
								presenterItem.InvalidatePainter();
						}
						break;
				}
			});
		}
	}
}