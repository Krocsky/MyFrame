using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Json消息类
    /// </summary>
    public class MessageData
    {
        public MessageData(StatusType status, string message, object resObj = null)
        {
            this.Status = status;
            this.Message = message;
            this.ResObj = resObj;
        }

        public MessageData(bool success, string message, object resObj = null)
        {
            if (success)
                this.Status = StatusType.Success;
            else
                this.Status = StatusType.Error;
            this.Message = message;
            this.ResObj = resObj;
        }

        /// <summary>
        /// 状态 成功1,失败-1,警告0
        /// </summary>
        public StatusType Status { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// 返回obj
        /// </summary>
        public object ResObj { get; set; }
    }

    public enum StatusType
    {
        Warn = 0,
        Success = 1,
        Error = 2
    }
}
