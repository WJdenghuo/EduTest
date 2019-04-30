﻿using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Edu.Tools
{
 
    public class LogHelper
    {
        private static ILog logger;
        static LogHelper()
        {
            if (logger == null)
            {
                var repository = LogManager.CreateRepository("NETCoreRepository");
                //log4net从log4net.config文件中读取配置信息
                XmlConfigurator.Configure(repository, new FileInfo("Logs/log4net.config"));
                logger = LogManager.GetLogger(repository.Name, "InfoLogger");
            }
        }

        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Info(string message, Exception exception = null)
        {
            if (exception == null)
                logger.Info(message);
            else
                logger.Info(message, exception);
        }
        public static void Info(string msg, Exception exception = null, params object[] args)
        {
            string message= string.Format(msg, args);
            if (exception == null)
                logger.Info(message);
            else
                logger.Info(message, exception);
        }
        /// <summary>
        /// 告警日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Warn(string message, Exception exception = null)
        {
            if (exception == null)
                logger.Warn(message);
            else
                logger.Warn(message, exception);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Error(string message, Exception exception = null)
        {
            if (exception == null)
                logger.Error(message);
            else
                logger.Error(message, exception);
        }
        public static void Debug(string message, Exception exception = null)
        {
            if (exception == null)
                logger.Debug(message);
            else
                logger.Debug(message, exception);
        }

        public static void Fatal(string message, Exception exception = null)
        {
            if (exception == null)
                logger.Fatal(message);
            else
                logger.Fatal(message, exception);
        }
    }
}
