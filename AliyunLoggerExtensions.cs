using Microsoft.Extensions.Logging.Aliyun;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Logging
{
    public static class AliyunLoggerExtensions
    {
        public static ILoggerFactory AddAliyunLogger(
                                      this ILoggerFactory loggerFactory,
                                      Microsoft.Extensions.Logging.Aliyun.Configuration config)
        {
            loggerFactory.AddProvider(new LoggerProvider(config));
            return loggerFactory;
        }
        public static ILoggerFactory AddAliyunLogger(
                                        this ILoggerFactory loggerFactory,
                                        Action<Microsoft.Extensions.Logging.Aliyun.Configuration> configure)
        {
            var config = new Microsoft.Extensions.Logging.Aliyun.Configuration();
            configure(config);
            return loggerFactory.AddAliyunLogger(config);
        }

        public static ILoggingBuilder AddAliyunLogger(
                                     this Microsoft.Extensions.Logging.ILoggingBuilder logingBuilder,
                                     Action<Microsoft.Extensions.Logging.Aliyun.Configuration> configure)
        {
            var config = new Microsoft.Extensions.Logging.Aliyun.Configuration();
            configure(config);
            logingBuilder.AddProvider(new LoggerProvider(config));
            return logingBuilder;
        }

        public static ILoggingBuilder AddAliyunLogger(
                                    this Microsoft.Extensions.Logging.ILoggingBuilder loggerFactory,
                                    Microsoft.Extensions.Logging.Aliyun.Configuration configure)
        {
            loggerFactory.AddProvider(new LoggerProvider(configure));
            return loggerFactory;
        }
    }
}
