using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgLife.Classes
{
    static class ValidationRegex
    {
        static public string PhoneRegex = @"^(\+375)(29|25|44|33)(\d{6})";
        static public string NameRegex = @"^[А-ЯA-Z]{1}[а-яa-z]*";
        static public string EmailRegex = @"^[A-Za-z0-9][A-Za-z0-9._-]*@([A-Za-z0-9]*)+\.+[A-Za-z]*$";
        static public string Login = @"^[A-Za-z0-9_-]*$";
        static public string Password = @"^[A-Za-z0-9_-]{4,18}$";
    }
}
