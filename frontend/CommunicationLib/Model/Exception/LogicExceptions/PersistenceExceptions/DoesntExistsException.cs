﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationLib.Exception
{
    public class DoesntExistsException : PersistenceException
    {
        private int _number = 11250;
        new public int number { get { return _number; } }

        public DoesntExistsException()
            : base("Das Element existiert nicht.")
        {

        }

        public DoesntExistsException(string message)
         : base(message)
        {
        }

        public DoesntExistsException(string message, System.Exception inner)
            : base(message, inner)
        {
        }
    }
}
