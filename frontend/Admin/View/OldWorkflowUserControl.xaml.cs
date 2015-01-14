﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using CommunicationLib.Model;
using Admin.ViewModel;

namespace Admin.View
{
    /// <summary>
    /// Interaktionslogik für WorkflowUserControl.xaml
    /// </summary>
    public partial class OldWorkflowUserControl : UserControl
    {
        
        public OldWorkflowUserControl()
        {
            InitializeComponent();
            

        }
        protected void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
           
            var lvi = sender as ListViewItem;
            if (lvi != null)
            {
                Workflow wv = lvi.DataContext as Workflow;
                OLD_WorkflowViewModel wmv = (OLD_WorkflowViewModel)this.DataContext;
                
                wmv.actWorkflow = wv;
                wmv.workflowActivity = "";
             }
            
        }
            
        
    }



}