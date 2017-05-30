using System;

namespace Common
{
    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity : IEntity<int>
    {

    }

    /// <summary>
    /// 实体接口
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}
