using System.ComponentModel.DataAnnotations;

namespace Common
{
    /// <summary>
    /// 所有枚举类
    /// </summary>
    /// 


    ///信息状态
    public enum MessageCode
    {
        [Display(Name = "订单不存在")]
        Not_Exist = 0,
        [Display(Name = "余额不足")]
        Not_Sufficient_Funds = 1,
        [Display(Name = "预约订单超过3次")]
        Over_Times = 2,
        [Display(Name = "缺少信息")]
        Lack_Info = 3,

    }

    /// <summary>
    /// 活动状态
    /// </summary>
    public enum PushState
    {
        [Display(Name = "已发送")]
        Send = 1,
        [Display(Name = "未发送")]
        NotSend = 2,
        [Display(Name = "待定")]
        Undetermined = 3
    }

    /// <summary>
    /// 活动状态
    /// </summary>
    public enum ActivityState
    {
        [Display(Name = "未开始")]
        NotStart = 1,
        [Display(Name = "进行中")]
        UnderWay = 2,
        [Display(Name = "已结束")]
        Over = 3
    }

    /// <summary>
    /// 性别
    /// </summary>
    public enum Gender
    {
        [Display(Name = "女")]
        Female = 1,
        [Display(Name = "男")]
        Male = 2,
        [Display(Name = "待定")]
        Undetermined = 3
    }

    /// <summary>
    /// 订单支付类型
    /// </summary>
    public enum PayState
    {
        [Display(Name = "未支付")]
        Not_Paid = 1,
        [Display(Name = "已支付")]
        Paid = 2,
        [Display(Name = "待定")]
        Undetermined = 3
    }

    /// <summary>
    /// 支付类型
    /// </summary>
    public enum PayType
    {
        [Display(Name = "押金")]
        Deposite = 1,
        [Display(Name = "充值")]
        Charge = 2,
        [Display(Name = "立即支付")]
        QUICKPay = 3,
        [Display(Name = "待定")]
        Undetermined = 4
    }

    /// <summary>
    /// 支付方式
    /// </summary>
    public enum CostWay
    {
        [Display(Name = "支付宝")]
        ZhiFuBao = 1,
        [Display(Name = "微信")]
        WeiXin = 2,
        [Display(Name = "其它")]
        Other = 3
    }

    /// <summary>
    /// 冻结不可以登录
    /// </summary>
    public enum UserStatus
    {
        [Display(Name = "正常")]
        Normal = 1,
        [Display(Name = "冻结")]
        Freeze = 2
    }

    /// <summary>
    /// 设备状态枚举类型
    /// </summary>
    public enum DeviceStatus
    {
        [Display(Name = "正常")]
        Normal = 1,
        [Display(Name = "使用中")]
        Appointed = 3,
        [Display(Name = "损坏")]
        Ruined = 2
    }

    /// <summary>
    /// 设备类型枚举
    /// </summary>
    public enum DeviceType
    {
        [Display(Name = "跑步机")]
        Treadmill = 1,
        [Display(Name = "哑铃")]
        Dumbbell = 2
    }

    /// <summary>
    /// 反馈状态
    /// </summary>
    public enum FeedbackType
    {
        [Display(Name = "解决中")]
        ToDo = 1,
        [Display(Name = "已解决")]
        Handled = 2,
        [Display(Name = "未解决")]
        Unsolved = 3
    }

    /// <summary>
    /// 附件所属
    /// </summary>
    public enum TenantType
    {
        /// <summary>
        /// 学校
        /// </summary>
        School = 0
    }

    /// <summary>
    /// 附件媒体类型
    /// </summary>
    public enum MediaType
    {
        /// <summary>
        /// 图片
        /// </summary>
        Image = 1,
        /// <summary>
        /// 视频
        /// </summary>
        Video = 2,
        /// <summary>
        /// Flash
        /// </summary>
        Flash = 3,
        /// <summary>
        /// 音乐
        /// </summary>
        Audio = 4,
        /// <summary>
        /// 文档
        /// </summary>
        Document = 5,
        /// <summary>
        /// 压缩包
        /// </summary>
        Compressed = 6,
        /// <summary>
        /// 其他类型
        /// </summary>
        Other = 99
    }

    /// <summary>
    /// 订单类型
    /// </summary>
    public enum OrderType
    {
        [Display(Name = "预约")]
        Appointment = 1,
        [Display(Name = "扫码")]
        Scan = 2,
        [Display(Name = "待定")]
        Undetermined = 3
    }

    /// <summary>
    /// 消息推送状态
    /// </summary>
    public enum NoticeStatus
    {
        [Display(Name = "未发送")]
        ToDo = 1,
        [Display(Name = "已发送")]
        Sended = 2
    }

    /// <summary>
    /// 地区类型
    /// </summary>
    public enum AreaType
    {
        [Display(Name = "省")]
        Province = 1,
        [Display(Name = "市")]
        City = 2,
        [Display(Name = "区")]
        Areas = 3
    }

    /// <summary>
    /// 性别
    /// </summary>
    public enum GenderType
    {
        [Display(Name = "女")]
        Female = 1,
        [Display(Name = "男")]
        Male = 2,
        [Display(Name = "待定")]
        Undetermined = 3
    }

    /// <summary>
    /// 冻结不可以登录
    /// </summary>
    public enum UserStatusType
    {
        [Display(Name = "正常")]
        Normal = 1,
        [Display(Name = "冻结")]
        Freeze = 2
    }

    /// <summary>
    /// 用户类型
    /// </summary>
    public enum UserType
    {
        [Display(Name = "客户")]
        Customer = 1,
        [Display(Name = "维修人员")]
        Repair = 2,
        [Display(Name = "公司人员")]
        Company = 3,
        [Display(Name = "系统后台用户")]
        Admin = 4
    }

    /// <summary>
    /// 应用类型
    /// </summary>
    public enum ApplicationTypes
    {
        JavaScript = 0,
        NativeConfidential = 1
    }
}
