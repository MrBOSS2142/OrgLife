using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using Tulpep.NotificationWindow;

namespace OrgLife.Classes
{
    static public class CheckEvent
    {
        static public void Check()
        {
            Models.OrganizerDB context = new Models.OrganizerDB();
            DateTime thisDay = DateTime.Today;
            foreach (var ev in context.Event.Where(p => p.User == Classes.SelectUser.SelectUserID))
            {
                if (ev.DateEvent == thisDay.ToString().Remove(10))
                {
                    Notification(ev.Text_Event);
                }
            }
            thisDay.ToString();
        }

        static public void Notification(string textEvent)
        {
            PopupNotifier not = new PopupNotifier();
            not.TitleText = "OrgLife";
            not.ContentText = textEvent;
            not.Popup();
        }
    }
}
