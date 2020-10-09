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
                                      Microsoft.Extensions.Logging.Aliyun.AliyunLoggerConfiguration config)
        {
            loggerFactory.AddProvider(new AliyunLoggerProvider(config));
            return loggerFactory;
        }
        public static ILoggerFactory AddAliyunLogger(
                                        this ILoggerFactory loggerFactory,
                                        Action<Microsoft.Extensions.Logging.Aliyun.AliyunLoggerConfiguration> configure)
        {
            var config = new Microsoft.Extensions.Logging.Aliyun.AliyunLoggerConfiguration();
            configure(config);
            return loggerFactory.AddAliyunLogger(config);
        }

        public static ILoggingBuilder AddAliyunLogger(
                                     this Microsoft.Extensions.Logging.ILoggingBuilder logingBuilder,
                                     Action<Microsoft.Extensions.Logging.Aliyun.AliyunLoggerConfiguration> configure)
        {
            var config = new Microsoft.Extensions.Logging.Aliyun.AliyunLoggerConfiguration();
            configure(config);
            logingBuilder.AddProvider(new AliyunLoggerProvider(config));
            return logingBuilder;
        }

        public static ILoggingBuilder AddAliyunLogger(
                                    this Microsoft.Extensions.Logging.ILoggingBuilder loggerFactory,
                                    Microsoft.Extensions.Logging.Aliyun.AliyunLoggerConfiguration configure)
        {
            loggerFactory.AddProvider(new AliyunLoggerProvider(configure));
            return loggerFactory;
        }
    }
}
