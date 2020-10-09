using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Logging.Aliyun
{
    [ProviderAlias("Aliyun")]
    public class AliyunLoggerProvider : ILoggerProvider
    {
        private readonly AliyunLoggerConfiguration _config;
        private readonly ConcurrentDictionary<string,AliyunLogger> _loggers = new ConcurrentDictionary<string, AliyunLogger>();

        public AliyunLoggerProvider(AliyunLoggerConfiguration config)
        {
            _config = config;
        }


        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new AliyunLogger(name, _config));
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
