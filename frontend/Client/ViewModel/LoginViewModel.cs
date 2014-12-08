﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationLib.Model;
using System.Windows.Input;
using CommunicationLib.Exception;

namespace Client.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private MainViewModel _mainViewModel;
        public LoginViewModel(MainViewModel mainViewModelInstanz)
            : base()
        {
            _mainViewModel = mainViewModelInstanz;
        }
        public string Name
        {
            get
            {
                return "Login Model";
            }
        }
        /// <summary>
        /// Property for input from username text box.
        /// </summary>
        private System.Security.SecureString _securePwd = new System.Security.SecureString();
        public System.Security.SecureString securePwd
        {
            get
            {
                return _securePwd;
            }
            set
            {
                _securePwd = value;
                OnChanged("securePwd");
            }
        }
        /// <summary>
        /// Property for input from username text box.
        /// </summary>
        private string _username = "";
        public string username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                OnChanged("username");
            }
        }
        private ICommand _authenticate;
        public ICommand authenticate
        {
            get
            {
                if (_authenticate == null)
                {

                    _authenticate = new ActionCommand(execute =>
                    {
                        try{
                            RestAPI.RestRequester.checkUser(username, securePwd);
                            Console.WriteLine("userName: " + username);
                            _mainViewModel.CurrentPageViewModel = _mainViewModel.workflowViewModel;
                            _mainViewModel.username = _username;
                        }
                        catch (BasicException exc)
                        {
                            Console.WriteLine("Login fehlgeschlagen:");
                            Console.WriteLine(exc.ToString());
                        }
                        finally
                        {

                        }
                    }, canExecute =>
                    {
                        return true;
                    });
                }
                return _authenticate;
            }
        }
    }
}
