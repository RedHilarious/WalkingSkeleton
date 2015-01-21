﻿using Client.ViewModel;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Client.Util
{
    public class FormValidator : ValidationRule
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        public override ValidationResult Validate
          (object value, System.Globalization.CultureInfo cultureInfo)
        {
            var bindingGroup = value as BindingGroup;
            if (bindingGroup != null)
            {
                StringConverter stringConverter = new StringConverter();
                FormRow entry = bindingGroup.Items[0] as FormRow;
                String datatype = entry.datatype;
                if (!entry.value.Equals(""))
                {
                    logger.Debug("validation for" + entry.value);
                    logger.Debug("datatye: " + datatype);
                    Regex regex = null;
                    switch (datatype)
                    {
                        case "String":
                            regex = new Regex("[a-zA-Z]+");
                            break;
                        case "int":
                            regex = new Regex("[0-9]+");
                            break;
                        case "double":
                            regex = new Regex("-?\\d+(\\.\\d+)?");
                            break;
                    }

                    Match match = regex.Match(entry.value);
                    if (match.Success)
                    {
                        logger.Debug("match successfull");
                        logger.Debug(match.Value);
                    }
                    else
                    {
                        String message = entry.value + " entspricht nicht dem richtigen Datentyp!";
                        return new ValidationResult(false,message);
                    }
                }
            }
            return ValidationResult.ValidResult;
        }
    }
}