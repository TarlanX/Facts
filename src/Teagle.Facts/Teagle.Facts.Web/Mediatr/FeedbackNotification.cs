using System;
using Calabonga.UnitOfWork;
using Microsoft.Extensions.Logging;
using Teagle.Facts.Web.Mediatr.Base;

namespace Teagle.Facts.Web.Mediatr
{
    class FeedbackNotification : NotificationBase
    {
        public FeedbackNotification(string content,  Exception? exception = null)
            : base("FEEDBACK from jfacts.ru", content, "dev@teagle.net", "noreply@jfacts.ru", exception)
        {
        }
    }

    class FeedbackNotificationHandler : NotificationHandlerBase<FeedbackNotification>
    {
        public FeedbackNotificationHandler(IUnitOfWork unitOfWork, ILogger<FeedbackNotification> logger)
            : base(unitOfWork, logger)
        {
        }
    }

}