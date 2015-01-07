﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunicationLib.Model;
using CommunicationLib.Exception;
using System.Windows;
using CommunicationLib;

namespace Admin.ViewModel
{
    /// <summary>
    /// The UserViewModel contains properties and commands to send new users and roles to the server.
    /// Furthermore, the properties and commands are used as DataBindings to show current users and roles in the graphical user interface. 
    /// </summary>
    public class UserViewModel: ViewModelBase
    {
        private MainViewModel _mainViewModel;
        private IRestRequester _restRequester;

        public UserViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _restRequester = _mainViewModel.restRequester;

            InitModel();
        }

        /// <summary>
        /// Init the model via rest requests at first startup.
        /// </summary>
        private void InitModel()
        {
            // update userlist
            IList<User> allUsers = _restRequester.GetAllElements<User>();
            foreach (User user in allUsers)
            {
                _mainViewModel.userCollection.Add(user);
            }

            // update rolelist
            IList<Role> allRoles =  _restRequester.GetAllElements<Role>();
            foreach(Role role in allRoles)
            {
                RoleCheckboxRows.Add(new RoleCheckboxRow(role, false));
                _mainViewModel.roleCollection.Add(role);
            }
        }

        # region USER PROPERTIES

        /// <summary>
        /// User collection with all (synchronized) users.
        /// </summary>
        public ObservableCollection<User> UserCollection { get { return _mainViewModel.userCollection; } }

        /// <summary>
        /// Property _roleCheckboxRoles to fill list view with selectable roles.
        /// </summary>
        private ObservableCollection<RoleCheckboxRow> _roleCheckboxRows = new ObservableCollection<RoleCheckboxRow>();
        public ObservableCollection<RoleCheckboxRow> RoleCheckboxRows { get { return _roleCheckboxRows; } }

        /// <summary>
        /// Currently selected user which can be changed afterwards.
        /// If a user is selected, update user information in the view.
        /// </summary>
        private User _selectedUser;
        public User SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            { 
                _selectedUser = value;

                if (_selectedUser != null)
                {
                    PostUserButtonText = "Nutzer ändern";
                    DetailedUser = _selectedUser.Clone<User>();
                    foreach (RoleCheckboxRow roleCheckboxRow in _roleCheckboxRows)
                    {
                        roleCheckboxRow.IsSelected = DetailedUser.roles.Any(i => i.id == roleCheckboxRow.Role.id);
                    }
                }

                EnableUserTextBox = _selectedUser == null;
                OnChanged("SelectedUser");
            }
        }

        /// <summary>
        /// Property to fill detail information with this user.
        /// This user can either have the data of the SelectedUser, or new inputted data.
        /// </summary>
        private User _detailedUser = new User();
        public User DetailedUser
        {
            get
            {
                return _detailedUser;
            }
            set
            {
                _detailedUser = value;
                OnChanged("DetailedUser");
            }
        }

        /// <summary>
        /// Property for input from username text box.
        /// </summary>
        private string _enteredUsername = "";
        public string EnteredUsername
        {
            get
            {
                return _enteredUsername;
            }
            set
            {
                _enteredUsername = value;
                OnChanged("EnteredUsername");
            }
        }

        /// <summary>
        /// Property to enable the textbox to enter a username.
        /// </summary>
        private bool _enableUserTextBox = true;
        public bool EnableUserTextBox
        {
            get
            {
                return _enableUserTextBox;
            }
            set
            {
                _enableUserTextBox = value;
                OnChanged("EnableUserTextBox");
            }
        }

        /// <summary>
        /// Property which sets the text of the post user button.
        /// The text depends on whether the user is new or updated.
        /// </summary>
        private string _postUserButtonText = "Nutzer hinzufügen";
        public string PostUserButtonText
        {
            get
            {
                return _postUserButtonText;
            }
            set
            {
                _postUserButtonText = value;
                OnChanged("PostUserButtonText");
            }
        }

        /// <summary>
        /// Property to the change the users activity.
        /// </summary>
        private bool _selectedUserActivity = false;
        public bool SelectedUserActivity
        {
            get
            {
                return _selectedUserActivity;
            }
            set
            {
                _selectedUserActivity = value;
                OnChanged("SelectedUserActivity");
            }
        }

        # endregion

        # region USER COMMANDS

        /// <summary>
        /// Command to add a new user.
        /// </summary>
        private ICommand _addUserCommand;
        public ICommand AddUserCommand
        {
            get
            {
                if (_addUserCommand == null)
                {
                    _addUserCommand = new ActionCommand(execute =>
                    {
                        try
                        {
                            foreach (RoleCheckboxRow actRow in RoleCheckboxRows)
                            {
                                if (actRow.IsSelected)
                                {
                                    DetailedUser.roles.Add(actRow.Role);
                                }
                            }
                            PostUser(DetailedUser);
                        }
                        catch (BasicException be)
                        {
                            MessageBox.Show(be.Message);
                        }
                    }, canExecute =>
                    {
                        return DetailedUser.id.Length > 0;
                    });
                }
                return _addUserCommand;
            }
        }

        /// <summary>
        /// Command to deselect a currently selected user.
        /// </summary>
        private ICommand _deselectCommand;
        public ICommand DeselectCommand
        {
            get
            {
                if (_deselectCommand == null)
                {
                    _deselectCommand = new ActionCommand(execute =>
                    {
                        DeselectUser();
                    }, canExecute => _selectedUser != null);
                }

                return _deselectCommand;
            }
        }

        # endregion

        # region ROLE PROPERTIES

        /// <summary>
        /// Role collection with all (synchronized) roles.
        /// </summary>
        public ObservableCollection<Role> RoleCollection { get { return _mainViewModel.roleCollection; } }

        /// <summary>
        /// Property for input from rolename text box.
        /// </summary>
        private string _enteredRolename = "";
        public string EnteredRolename
        {
            get
            {
                return _enteredRolename;
            }
            set
            {
                _enteredRolename = value;
                OnChanged("EnteredRolename");
            }
        }

        /// <summary>
        /// Property for currently selected Role.
        /// </summary>
        private Role _selectedRole;
        public Role SelectedRole
        {
            get
            {
                return _selectedRole;
            }
            set
            {
                _selectedRole = value;
                UpdateUserInSelectedRoles();
                OnChanged("SelectedRole");
            }
        }

        /// <summary>
        /// Property to show users who have the currently selected Role.
        /// </summary>
        private ObservableCollection<User> _userInSelectedRole = new ObservableCollection<User>();
        public ObservableCollection<User> UserInSelectedRole
        {
            get
            {
                return _userInSelectedRole;
            }
            set
            {
                _userInSelectedRole = value;
                OnChanged("UserInSelectedRole");
            }
        }

        # endregion

        # region ROLE COMMANDS

        /// <summary>
        /// Command to add a new role.
        /// </summary>
        private ICommand _addRoleCommand;
        public ICommand AddRoleCommand
        {
            get
            {
                if (_addRoleCommand == null)
                {
                    _addRoleCommand = new ActionCommand(execute =>
                    {
                        try
                        {
                            Role newRole = new Role();
                            newRole.rolename = EnteredRolename;

                            _restRequester.PostObject(newRole);
                            EnteredRolename = "";
                        }
                        catch (BasicException be)
                        {
                            MessageBox.Show(be.Message);
                        }
                    }, canExecute =>
                    {
                        if (EnteredRolename.Length == 0)
                        {
                            return false;
                        }

                        return true;
                    });
                }

                return _addRoleCommand;
            }
        }

        /// <summary>
        /// Command to delete a role from a user.
        /// </summary>
        private ICommand _deleteRoleFromUserCommand;
        public ICommand DeleteRoleFromUserCommand
        {
            get
            {
                if (_deleteRoleFromUserCommand == null)
                {
                    _deleteRoleFromUserCommand = new ActionCommand(execute =>
                    {
                        if (SelectedRole != null)
                        {
                            User user = ((User) execute).Clone<User>();
                            user.roles.Remove(SelectedRole);
                            PostUser(user);
                        }
                    }, canExecute => true);
                }

                return _deleteRoleFromUserCommand;
            }
        }

        # endregion

        # region METHODS

        /// <summary>
        /// Post a given user to the server.
        /// </summary>
        /// <param name="user"></param>
        private void PostUser(User user)
        {
            _restRequester.PostObject(user);
            DeselectUser();
        }

        /// <summary>
        /// Deselect a currently selected user.
        /// </summary>
        private void DeselectUser()
        {
            SelectedUser = null;
            DetailedUser = new User();

            PostUserButtonText = "Nutzer hinzufügen";
            foreach (RoleCheckboxRow roleCheckboxRow in RoleCheckboxRows)
            {
                roleCheckboxRow.IsSelected = false;
            }
        }

        /// <summary>
        /// Update or add a new user to the ViewModel.
        /// </summary>
        /// <param name="newUser">The user to be added or updated.</param>
        public void UserUpdate(User newUser)
        {
   
            User toUpdate = _mainViewModel.userCollection.FirstOrDefault(u => newUser.id == u.id);
            if (toUpdate != null)
            {
                int indexToUpdate = _mainViewModel.userCollection.IndexOf(toUpdate);
                _mainViewModel.userCollection[indexToUpdate] = newUser;
            }
            else
            {
                _mainViewModel.userCollection.Add(newUser);
            }
            UpdateUserInSelectedRoles();
        }

        /// <summary>
        /// Add a new role to the ViewModel and CheckboxList.
        /// </summary>
        /// <param name="updatedRole">The role to be added.</param>
        public void RoleUpdate(Role updatedRole)
        {
            _mainViewModel.roleCollection.Add(updatedRole);
            RoleCheckboxRows.Add(new RoleCheckboxRow(updatedRole, false));
            UpdateUserInSelectedRoles();
        }

        /// <summary>
        /// Update the list view which shows all users who have the selected role.
        /// </summary>
        private void UpdateUserInSelectedRoles()
        {
            if (SelectedRole != null)
            {
                UserInSelectedRole = new ObservableCollection<User>(_mainViewModel.userCollection.Where(u => u.roles.Contains(_selectedRole)));
            }
            else
            {
                UserInSelectedRole.Clear();
            }
        }

        # endregion
    }
}
