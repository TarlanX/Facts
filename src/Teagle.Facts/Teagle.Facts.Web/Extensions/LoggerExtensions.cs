﻿using System;
using Microsoft.Extensions.Logging;

namespace Teagle.Facts.Web.Extensions
{
    static class EventIdentifiers
    {
        public static readonly EventId DatabaseSavingErrorId = new EventId(70040001, "DatabaseSavingError");
        public static readonly EventId NotificationAddedId = new EventId(70040002, "NotificationAdded");
    }

    public static class LoggerExtensions
    {
        public static void NotificationAdded(this ILogger source, string subject, Exception? exception = null)
        {
            NotificationAddedExecute(source, subject, exception);
        }

        private static readonly Action<ILogger, string, Exception?> NotificationAddedExecute =
            LoggerMessage.Define<string>(LogLevel.Information, EventIdentifiers.NotificationAddedId,
                "New Notification created: {subject}");



        public static void DatabaseSavingError(this ILogger source, string entityName, Exception? exception = null)
        {
            DatabaseSavingErrorExecute(source, entityName, exception);
        }

        private static readonly Action<ILogger, string, Exception?> DatabaseSavingErrorExecute =
            LoggerMessage.Define<string>(LogLevel.Error, EventIdentifiers.DatabaseSavingErrorId,
                "{entityName}");

    }
}