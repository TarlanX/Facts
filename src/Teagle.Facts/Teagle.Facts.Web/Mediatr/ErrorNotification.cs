using System;
using Calabonga.UnitOfWork;
using Microsoft.Extensions.Logging;
using Teagle.Facts.Web.Mediatr.Base;

namespace Teagle.Facts.Web.Mediatr
{
    class ErrorNotification : NotificationBase
    {
        public ErrorNotification(string content, Exception? exception = null) 
            : base("ERROR on jacts.ru", content, "dev@teagle.net", "noreply@jfacts.ru", exception)
        {
        }
    }

    class ErrorNotificationHandler : NotificationHandlerBase<ErrorNotification>
    {
        public ErrorNotificationHandler(IUnitOfWork unitOfWork, ILogger<ErrorNotification> logger)
            : base(unitOfWork, logger)
        {
        }
    }
}