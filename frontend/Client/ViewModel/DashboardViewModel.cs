﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using CommunicationLib.Model;
using CommunicationLib;
using RestAPI;
using NLog;
using System.Windows.Threading;
using System.Diagnostics;
using CommunicationLib.Exception;
using CommunicationLib.Model.DataModel;

namespace Client.ViewModel
{
    /// <summary>
    /// The WorkflowViewModel contains properties and commands to create a new workflow and to send it to the server.
    /// Furthermore, the properties and commands are used as DataBindings in the graphical user interface.
    /// </summary>
    public class DashboardViewModel : ViewModelBase
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        
        /// <summary>
        /// Constructor for DashboardViewModel with its parent MainViewModel.
        /// </summary>
        /// <param name="mainViewModelInstanz"></param>
        public DashboardViewModel(MainViewModel mainViewModelInstanz)
        {
            _mainViewModel = mainViewModelInstanz;
        }

        /// <summary>
        /// Init update-Methode used while the login.
        /// </summary>
        private void InitModel()
        {
            _restRequester = _mainViewModel.restRequester;
            logger.Debug("Init Model");
            _workflows.Clear();
            IList<Workflow> workflowList = null;
            try
            {
                workflowList = _restRequester.GetAllElements<Workflow>();
            } catch (BasicException exc){ MessageBox.Show(exc.Message);}
            
            if (workflowList == null)
            {
                workflowList = new List<Workflow>();
            }
            workflowList.ToList().ForEach(_workflows.Add);

            _startableWorkflows.Clear();
            IList<string> startableList = null;
            try
            {
                startableList = _restRequester.GetStartablesByUser();
            }
            catch (BasicException exc) { MessageBox.Show(exc.Message); }
            

            if (startableList == null)
            {
                startableList = new List<string>();
            }
            startableList.ToList().ForEach(_startableWorkflows.Add);
            
            foreach (Workflow workflow in _workflows)
            {
                AddWorkflowToModel(workflow);
                _mainViewModel.myComLib.Listener.RegisterItemSource(workflow);
            }

            if (_workflows.Count == 0 && startableList.Count == 0)
            {
                workflowMessage = "Keine Workflows vorhanden.";
                workflowMessageVisibility = "Visible";

            }
            else
            {
                workflowMessageVisibility = "Collapsed";
            }
        }

        /// <summary>
        /// Update a single workflow
        /// </summary>
        /// <param name="updatedWorkflow">workflow which has to be updated</param>
        public void AddWorkflowToModel(Workflow updatedWorkflow)
        {
            logger.Debug("addWorkflowtoModel");
            DashboardWorkflow toUpdate = new DashboardWorkflow(updatedWorkflow);

            IList<string> startableList = null;
            _startableWorkflows.Clear();
            try
            {
                startableList = _restRequester.GetStartablesByUser();
            }
            catch (BasicException exc) { MessageBox.Show(exc.Message); }
            startableList.ToList().ForEach(_startableWorkflows.Add);

            toUpdate.startPermission = _startableWorkflows.Contains(updatedWorkflow.id);

            _relevantItems.Clear();
            _restRequester.GetRelevantItemsByUser(updatedWorkflow.id).ToList().ForEach(_relevantItems.Add);
            Step activeStep;
            DashboardRow row;
            foreach (Item item in _relevantItems)
            {
                activeStep = GetStepById(item.GetActiveStepId(), updatedWorkflow);
                logger.Debug("active Step" + activeStep.ToString());
                row = new DashboardRow(item, activeStep, _userName, updatedWorkflow.form);
                toUpdate.AddDashboardRow(row);
            }

            if (_workflows.Count == 0 && startableList.Count == 0)
            {
                workflowMessage = "Keine Workflows vorhanden.";
                workflowMessageVisibility = "Visible";

            }
            else
            {
                workflowMessageVisibility = "Collapsed";
            }

            Application.Current.Dispatcher.Invoke(new System.Action(() => _dashboardWorkflows.Add(toUpdate)));
            logger.Debug("Workflow Update ID="+toUpdate.actWorkflow.id + " ItemCount="+toUpdate.dashboardRows.Count); 
        }

        /// <summary>
        /// Updates a single item.
        /// </summary>
        /// <param name="item">the item to update</param>
        public void UpdateItem(Item item)
        {
            String workflowId = item.workflowId;
            _relevantItems.Clear();
            try
            {
                _restRequester.GetRelevantItemsByUser(workflowId).ToList().ForEach(_relevantItems.Add);
            }
            catch (BasicException exc) { MessageBox.Show(exc.Message); }
            
            if(IsItemInList(item.id, _relevantItems)) {
                DashboardRow fittingRow = GetWorkflowRowForItem(item);
                fittingRow.actItem = item;
                fittingRow.actStep = GetStepById(item.GetActiveStepId(), workflowId);
                selectedRow = fittingRow;
                OnChanged("selectedRow");
            }
        }

        /// <summary>
        /// deletes the item with the fitting id from the model
        /// </summary>
        /// <param name="id">id of the item</param>
        public void DeleteItem(String id)
        {
            foreach (DashboardWorkflow dashboardWorkflow in _dashboardWorkflows)
            {
                foreach (DashboardRow dashboardRow in dashboardWorkflow.dashboardRows)
                {
                    if (dashboardRow.actItem.id.Equals(id))
                    {
                        dashboardWorkflow.DeleteDashboardRow(dashboardRow);
                        logger.Debug("Item mit der id " + id + " wurde gelöscht.");     
                        return;
                    }
                }
            }
            logger.Debug("Item mit der id "+ id +" konnte nicht gelöscht werden.");     
        }
        /// <summary>
        /// Checks whether the new item is relevant for the user.
        /// </summary>
        /// <param name="id">id of new or updated item</param>
        /// <param name="itemsList">List of the relevant items for the user</param>
        /// <returns>true if item is in itemsList, else false</returns>
        private Boolean IsItemInList(String id, List<Item> itemsList)
        {
            foreach (Item checkItem in itemsList)
            {
                if (id.Equals(checkItem.id))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// The method searches the dashboardRow for the given item.
        /// </summary>
        /// <param name="item">the given item</param>
        /// <returns>the fitting dashboardrow or null</returns>
        private DashboardRow GetWorkflowRowForItem(Item item)
        {
            String workflowId = item.workflowId;
            bool changed = true;
            

            // find workflowModel form item
            foreach (DashboardWorkflow workflow in _dashboardWorkflows)
            {
                if (workflowId.Equals(workflow.actWorkflow.id))
                {
                    DashboardRow fittingRow = workflow.dashboardRows.FirstOrDefault(dr => dr.actItem.id.Equals(item.id));
                    if(fittingRow == null)
                    {
                        // create DashboardRow for item
                        Step actStep = GetStepById(item.GetActiveStepId(), workflow.actWorkflow);
                        fittingRow = new DashboardRow(item, actStep, userName, workflow.actWorkflow.form);
                        workflow.AddDashboardRow(fittingRow);
                        changed = false;
                    }

                    logger.Debug("Item ID=" + item.id + (changed ? " changed" : " added"));
                    
                    return fittingRow;
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the step for the given id in a single workflow.
        /// </summary>
        /// <param name="id">id of the step</param>
        /// <param name="workflow">the associated workflow</param>
        /// <returns>the fitting step or null</returns>
        private Step GetStepById(int id, Workflow workflow)
        {
            foreach (Step step in workflow.steps)
            {
                if (id.ToString().Equals(step.id))
                {
                    return step;
                }
            }
            return null;
        }
        private Step GetStepById(int id, String workflowId)
        {
            foreach (DashboardWorkflow workflow in _dashboardWorkflows)
            {
                if (workflowId.Equals(workflow.actWorkflow.id))
                {
                foreach (Step step in workflow.actWorkflow.steps)
                {
                    if (id.ToString().Equals(step.id))
                    {
                        return step;
                    }
                }
                }
            }
            return null;
        }

        /// <summary>
        /// ´Clears all data in the model.
        /// </summary>
        private void DeleteModel()
        {
            logger.Info("Clear Model");
            _workflows.Clear();
            _startableWorkflows.Clear();
            _relevantItems.Clear();
            _dashboardWorkflows.Clear();
            workflowMessage = "";
            workflowMessageVisibility = "Collapsed";
        }

        /// <summary>
        /// Starts a workflow with the given id.
        /// </summary>
        /// <param name="id"></param>
        public void CreateWorkflow(string id)
        {
            try
            {
                _restRequester.StartWorkflow(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Command to step forward, called from the gui.
        /// </summary>
        public ICommand stepForwardCommand
        {
            get
            {
                if (_stepForwardCommand == null)
                {
                    _stepForwardCommand = new ActionCommand(execute => 
                    {
                        try
                        {
                            DashboardRow param = (DashboardRow)execute;
                            _restRequester.PostObject(param.actItem);
                            _restRequester.StepForward(param.actStep.id, param.actItem.id);
                        }
                        catch (BasicException exc)
                        {
                            if (exc is StorageFailedException)
                            {
                                MessageBox.Show("Ungültige Benutzereingaben.");
                            }
                            
                        }
                }, canExecute => true);
                }
                return _stepForwardCommand;
            }
        }
        private ICommand _stepForwardCommand;
        
        /// <summary>
        /// Command to save an item. Called from the gui.
        /// </summary>
        public ICommand saveItemCommand
        {
            get
            {
                if (_saveItemCommand == null)
                {
                    _saveItemCommand = new ActionCommand(execute =>
                    {
                        DashboardRow param = (DashboardRow)execute;
                        try
                        {
                            _restRequester.PostObject(param.actItem);
                        }
                        catch (BasicException exc)
                        {
                            if (exc is StorageFailedException)
                            {
                                MessageBox.Show("Ungültige Benutzereingaben.");
                            }
                        }
                    }, canExecute => true);
                }
                return _saveItemCommand;
            }
        }
        private ICommand _saveItemCommand;
        
        /// <summary>
        /// Logout command for the user;
        /// the data is deleted an view changes to to login view.
        /// </summary>
        public ICommand logoutCommand
        {
            get
            {
                if (_logoutCommand == null)
                {
                    _logoutCommand = new ActionCommand(excute =>
                        {
                            userName = "";
                            DeleteModel();
                            _mainViewModel.CurrentPageViewModel = _mainViewModel.loginViewModel;

                            // unregister mainViewModel from CommunicationLib (if logout worked)
                            _mainViewModel.myComLib.Logout();
                        }, canExecute =>
                            {
                                return true;
                            });
                }
                return _logoutCommand;
            }
        }
        private ICommand _logoutCommand;

        /// <summary>
        /// Command to start a workflow.
        /// </summary>
        public ICommand startWorkflowCommand
        {
            get
            {
                if (_startWorkflowCommand == null)
                {
                    _startWorkflowCommand = new ActionCommand(execute =>
                    {
                        DashboardWorkflow param = (DashboardWorkflow)execute;
                        CreateWorkflow(param.actWorkflow.id);
                    }, canExecute =>
                    {
                        return true;
                    });
                }
                return _startWorkflowCommand;
            }
        }
        private ICommand _startWorkflowCommand;
        

        #region properties

        /// <summary>
        /// The actual selected row in table of workflowViewModels.
        /// </summary>
        public DashboardRow selectedRow
        {
            get
            {
                return _selectedRow;
            }
            set
            {
                _selectedRow = value;
                OnChanged("selectedRow");
            }
        }
        private DashboardRow _selectedRow = null;
        
        private IRestRequester _restRequester;
        private MainViewModel _mainViewModel;

        /// <summary>
        /// The username of the logged in user.
        /// </summary>
        public String userName
        {
            get { return _userName; }
            set
            {
                _userName = value;

                if (_userName.Equals(""))
                {
                    DeleteModel();
                }
                else
                {
                        InitModel();            
                }
            }
        }
        private String _userName = "";

        /// <summary>
        /// List of all workflows from server, displayed in this ViewModel.
        /// </summary>
        public ObservableCollection<Workflow> workflows { get { return _workflows; } }
        private ObservableCollection<Workflow> _workflows = new ObservableCollection<Workflow>();
        
        /// <summary>
        /// List of all ids from workflows displayed in this ViewModel.
        /// </summary>
        public ObservableCollection<string> startableWorkflows { get { return _startableWorkflows; } }
        private ObservableCollection<string> _startableWorkflows = new ObservableCollection<string>();
        
        /// <summary>
        /// List of all ViewModels of the workflows in the workflow list.
        /// </summary>
        public ObservableCollection<DashboardWorkflow> dashboardWorkflows { get { return _dashboardWorkflows; } }
        private ObservableCollection<DashboardWorkflow> _dashboardWorkflows = new ObservableCollection<DashboardWorkflow>();
        
        private List<Item> _relevantItems = new List<Item>();

        public String workflowMessage
        {
            get
            {
                return _workflowMessage;
            }
            set
            {
                _workflowMessage = value;
                OnChanged("workflowMessage");
            }
        }
        private String _workflowMessage = "";

        public String workflowMessageVisibility
        {
            get
            {
                return _workflowMessageVisibility;
            }
            set
            {
                _workflowMessageVisibility = value;
                OnChanged("workflowMessageVisibility");
            }
        }
        private String _workflowMessageVisibility;

        #endregion
    }
}