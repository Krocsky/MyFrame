using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class AutoMapperExtension
    {
        /// <summary>
        ///  类型映射
        /// </summary>
        public static T MapTo<T>(this object obj)
        {
            if (obj == null) return default(T);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap(obj.GetType(), typeof(T));
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<T>(obj);
        }

        /// <summary>
        /// 集合列表类型映射
        /// </summary>
        public static List<TDestination> MapToList<TDestination>(this IEnumerable source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                foreach (var first in source)
                {
                    var type = first.GetType();
                    cfg.CreateMap(type, typeof(TDestination));
                    break;
                }
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<List<TDestination>>(source);
        }

        /// <summary>
        /// 集合列表类型映射
        /// </summary>
        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<List<TDestination>>(source);
        }
        /// <summary>
        /// 类型映射
        /// </summary>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
            where TSource : class
            where TDestination : class
        {
            if (source == null) return destination;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map(source, destination);
        }
    }
}
