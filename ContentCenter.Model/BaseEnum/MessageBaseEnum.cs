using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model.BaseEnum
{
   
    public enum SystemNoteTarget
    {
        Single = 1,
        Group = 2,
    }
    public enum NotificationType
    {
        comment = 2,
        reply = 3,
        praize = 1,
        system = 200,
        message = 100,
    }

    public enum NotificationStatus
    {
        created = 0,
        sent = 1,
        read = 2,
    }

}
