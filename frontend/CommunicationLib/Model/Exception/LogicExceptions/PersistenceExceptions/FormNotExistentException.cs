﻿using CommunicationLib.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationLib.Exception
{
    class FormNotExistentException : NotExistentException
    {
         /// <summary>
        /// This Exception es a child of DoesntExistsException. 
        /// Each Exception has an number, here it is 11254. 
        /// </summary>
        private int _number = 11255;
        new public int number { get { return _number; } }

        public FormNotExistentException()
            : base("Das Formular existiert nicht.")
        {

        }

        /// <summary>
        /// This constructor allows to add a spezial message.
        /// </summary>
        /// <param name="message">the message</param>
        public FormNotExistentException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// This constructor allows to add a spezial message and an other exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public FormNotExistentException(string message, System.Exception inner)
            : base(message, inner)
        {
        }
    }
}
