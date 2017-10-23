﻿using DOL.WHD.Section14c.Log.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DOL.WHD.Section14c.Log.Repositories
{
    public class ErrorLogRepository: IErrorLogRepository
    {
        private readonly ApplicationLogContext _dbContext;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private Boolean Disposed;

        public ErrorLogRepository()
        {
            _dbContext = new ApplicationLogContext();
        }

        /// <summary>
        /// Get All Logs
        /// </summary>
        /// <returns></returns>
        public IQueryable<APIErrorLogs> GetAllLogs()
        {
            return _dbContext.ErrorLogs.AsQueryable();
        }

        /// <summary>
        /// Add New Log Message
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public LogDetails AddLog(LogDetails entity)
        {
            
            if (entity != null)
            {
                LogEventInfo eventInfo = new LogEventInfo();
                eventInfo.Properties[Constants.CorrelationId] = Guid.NewGuid().ToString();

                eventInfo.Properties[Constants.EIN] = string.IsNullOrEmpty(entity.EIN) ? string.Empty : entity.EIN;
                eventInfo.LoggerName = "NLog";
                if (string.IsNullOrEmpty(entity.Message))
                {
                    throw new ArgumentException("Message cannot be null or empty string", "Log Message");
                }

                eventInfo.Message = entity.Message;

                if (!string.IsNullOrEmpty(entity.Exception))
                {
                    eventInfo.Exception = new Exception(entity.Exception);
                }
                
                eventInfo.Level = LogLevel.FromString(entity.Level);
                eventInfo.Properties[Constants.UserId] = entity.UserId;
                eventInfo.Properties[Constants.UserName] = entity.User;
                _logger.Log(eventInfo);
            }
            return entity;
        }
        /// <summary>
        /// Dispose Object
        /// </summary>
        public void Dispose()
        {
            if (!Disposed)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    Disposed = true;
                }
            }
        }
    }
}