using Aliyun.Api.LogService;
using Aliyun.Api.LogService.Domain.Log;
using Aliyun.Api.LogService.Infrastructure.Authentication;
using Aliyun.Api.LogService.Infrastructure.Protocol.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Logging.Aliyun
{
    internal sealed class AliyunLogger : ILogger
    {
        private readonly string categoryName;
        private static AliyunLoggerConfiguration _config;
        private readonly static string _ver;
        private readonly static string _appName;
        private static readonly System.Collections.Concurrent.ConcurrentBag<LogInfo> logs;
        private static System.Timers.Timer tmr;
        private static object _tmrLock = new object();
        private static ILogServiceClient _client;
        ILoggerProvider Provider;
        internal IExternalScopeProvider ScopeProvider { get; set; }

        static AliyunLogger()
        {
            if (null != System.Reflection.Assembly.GetEntryAssembly())
            {
                _appName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
                _ver = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
            }
            else
            {
                _appName = "";
                _ver = "0.0.0.0";
            }
            logs = new System.Collections.Concurrent.ConcurrentBag<LogInfo>();
        }

        public AliyunLogger(string categoryName, AliyunLoggerConfiguration config, ILoggerProvider Provider)
        {
            this.Provider = Provider;
            if (categoryName == null)
                throw new NotImplementedException("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
            lock (_tmrLock)
            {
                if (null == _client)
                {
                    var client = LogServiceClientBuilders.HttpBuilder
              // 服务入口<endpoint>及项目名<projectName>。
              .Endpoint($"{config.Project_name}.{config.Region_endpoint}", config.Project_name)
              // 访问密钥信息。
              .Credential(config.AccessKeyId, config.AccessKeySecret)
              // 设置每次请求超时时间。
              .RequestTimeout(1000)
              .Build();

                    this.categoryName = categoryName.Clone() as string;
                    _config = config;
                    _client = client;

                    tmr = new System.Timers.Timer(config.MaxDelaySecends * 1000);
                    tmr.Elapsed += Tmr_Elapsed;
                    tmr.Start();
                }
            }

        }




        private static void Tmr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DoPutAsync();
        }

        public static void DoPutAsync()
        {
            if (logs.Count == 0)
                return;

            //这里需要是异步
            LogInfo[] arr = null;
            lock (logs)
            {
                arr = logs.ToArray();
                logs.Clear();
            }
            var logGroupInfo = new LogGroupInfo
            {
                Topic = _appName,
                Logs = arr,
                Source = System.Environment.MachineName,
                LogTags =
                {
                    {"ver",_ver},
                },
            };
            try
            {
                var response = _client.PostLogStoreLogsAsync(_config.LogstoreName, logGroupInfo).Result;
                // 此接口没有返回结果，确保返回结果成功即可。
                if (!response.IsSuccess)
                {
                    System.Diagnostics.Debug.WriteLine($"上传日志错误，RequestId：{  response.RequestId}，code:{response.Error.ErrorCode} msg:{response.Error.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"上传日志错误，RequestId：{ex}");
            }
        }

        public IDisposable BeginScope<TState>(TState state) => ScopeProvider?.Push(state) ?? default;

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (_config.EventId == 0 || _config.EventId == eventId.Id)
            {
                var logInfo = new LogInfo
                {
                    Contents =
                        {
                            {"level", logLevel+""},
                            {"time",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")},
                             {"name",categoryName},
                            {"content",formatter(state, exception)},
                        },
                    Time = DateTimeOffset.Now
                };
                ScopeProvider?.ForEachScope((o, t) =>
                {
                    if (o is IEnumerable<KeyValuePair<string, object>>)
                    {
                        var obj = o as IEnumerable<KeyValuePair<string, object>>;
                        if (null == obj) return;
                        foreach (var item in obj)
                        {
                            logInfo.Contents.TryAdd(item.Key, item.Value + "");
                        }
                    }
                    else
                    {

                    }
                }, state);



                logs.Add(logInfo);
                //if (logs.Count >= _config.LogsCountPerPut)
                // DoPutAsync();
            }
        }
    }
}
