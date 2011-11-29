﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using FiresecAPI.Models;
using Infrastructure;
using Infrastructure.Common;
using PlansModule.Designer;
using PlansModule.Events;
using PlansModule.Views;
using System.Diagnostics;

namespace PlansModule.ViewModels
{
    public partial class PlansViewModel : RegionViewModel
    {
        List<ElementBase> Buffer;

        void InitializeCopyPaste()
        {
            CopyCommand = new RelayCommand(OnCopy, CanCopyCut);
            CutCommand = new RelayCommand(OnCut, CanCopyCut);
            PasteCommand = new RelayCommand(OnPaste, CanPaste);
            Buffer = new List<ElementBase>();
        }

        bool CanCopyCut(object obj)
        {
            return DesignerCanvas.SelectedItems.Count() > 0;
        }

        public RelayCommand CopyCommand { get; private set; }
        void OnCopy()
        {
            Trace.WriteLine("Copy");

            PlanDesignerViewModel.Save();
            Buffer = new List<ElementBase>();
            foreach (var designerItem in DesignerCanvas.SelectedItems)
            {
                designerItem.SavePropertiesToElementBase();
                Buffer.Add(designerItem.ElementBase.Clone());
            }
        }

        public RelayCommand CutCommand { get; private set; }
        void OnCut()
        {
            OnCopy();
            DesignerCanvas.RemoveAllSelected();
            PlansModule.HasChanges = true;
        }

        bool CanPaste(object obj)
        {
            return Buffer.Count > 0;
        }

        public RelayCommand PasteCommand { get; private set; }
        void OnPaste()
        {
            NormalizeBuffer();

            DesignerCanvas.DeselectAll();
            foreach (var elementBase in Buffer)
            {
                elementBase.UID = Guid.NewGuid();
                var designerItem = DesignerCanvas.AddElement(elementBase.Clone());
                designerItem.IsSelected = true;
            }
            ServiceFactory.Events.GetEvent<ElementAddedEvent>().Publish(DesignerCanvas.SelectedElements);
            PlansModule.HasChanges = true;
        }

        void NormalizeBuffer()
        {
            double minLeft = double.MaxValue;
            double minTop = double.MaxValue;
            double maxRight = 0;
            double maxBottom = 0;
            foreach (var elementBase in Buffer)
            {
                minLeft = Math.Min(elementBase.Left, minLeft);
                minTop = Math.Min(elementBase.Top, minTop);
                maxRight = Math.Max(elementBase.Left + elementBase.Width, maxRight);
                maxBottom = Math.Max(elementBase.Top + elementBase.Height, maxBottom);
            }
            foreach (var elementBase in Buffer)
            {
                elementBase.Left = elementBase.Left - minLeft + PlanDesignerView.Current._scrollViewer.HorizontalOffset / PlanDesignerViewModel.ZoomFactor;
                elementBase.Top = elementBase.Top - minTop + PlanDesignerView.Current._scrollViewer.VerticalOffset / PlanDesignerViewModel.ZoomFactor;
            }
            maxRight -= minLeft;
            maxBottom -= minTop;

            if ((maxRight > PlanDesignerViewModel.Plan.Width) || (maxBottom > PlanDesignerViewModel.Plan.Height))
            {
                MessageBox.Show("Размер вставляемого содержимого больше размеров плана");
            }
        }
    }
}