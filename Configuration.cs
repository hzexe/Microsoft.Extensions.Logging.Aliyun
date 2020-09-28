using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Logging.Aliyun
{
    /// <summary>
    /// 配置
    /// </summary>
    public class Configuration
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;
        public int EventId { get; set; } = 0;

        public string AccessKeyId { get; set; }

        public string AccessKeySecret { get; set; }

        /// <summary>
        /// 阿里云日志项目名
        /// </summary>
        public string Project_name { get; set; }

        /// <summary>
        /// 服务入口(公网入口，内网入口或全球加速)
        /// </summary>
        /// <remarks>见：https://help.aliyun.com/document_detail/29008.html?spm=a2c4g.11186623.6.1150.199b13faloVrJu#title-h85-wzi-3da</remarks>
        public string Region_endpoint { get; set; }


        /// <summary>
        /// 最大延时时间
        /// </summary>
        /// <remarks>会影响每次提交日志数量</remarks>
        public int MaxDelaySecends { get; set; }

        /// <summary>
        /// 每次提交日志数量
        /// </summary>
        /// <remarks>近似值。受最大延时时间限制</remarks>
        public int LogsCountPerPut { get; set; }


        public System.Net.Http.HttpMessageHandler messageHandler { get; set; }


        /// <summary>
        /// 已重写,对应访问地址
        /// </summary>
        /// <returns></returns>
        //public override string ToString() => $"{Project_name}.{Region_endpoint}";
    }
}
