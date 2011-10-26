﻿using System.Windows.Controls;
using System.Windows.Input;
using PlansModule.ViewModels;

namespace PlansModule.Views
{
    public partial class PlansTree : UserControl
    {
        public PlansTree()
        {
            InitializeComponent();
        }

        private void TreeView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var element = Mouse.DirectlyOver;
            var treeView = sender as TreeView;
            if (!(element is TextBlock))
            {
                (DataContext as OldPlansViewModel).SelectedPlan = null;
            }
            else
            {
                (DataContext as OldPlansViewModel).SelectedPlan =    treeView.SelectedItem as PlanViewModel;
                
                
            }
        }

        private void TreeView_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            var treeView = sender as TreeView;
            if (treeView.SelectedItem is PlanViewModel)
            {
                (DataContext as OldPlansViewModel).SelectedPlan = treeView.SelectedItem as PlanViewModel;
            }

        }
    }
}