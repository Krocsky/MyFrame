using Common;
using Common.Utilities;
using Data.Migrations;
using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;

namespace Data
{
    /// <summary>
    /// EF数据库上下文
    /// </summary>
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class EFDbContext : DbContext
    {
        public EFDbContext()
            : base("MySqlServer")
        { }

        public EFDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<EFDbContext, Configuration>());
        }

        #region Utilities

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ITypeFinder typeFinder = new WebAppTypeFinder();
            var typesToRegister = typeFinder.GetAssemblies().SelectMany(n => n.GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>)));

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
        }

        public static EFDbContext Create()
        {
            return new EFDbContext();
        }

        #endregion

        /// <summary>
        /// 数据集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public new DbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        /// <summary>
        /// 分离实体
        /// </summary>
        /// <param name="entity"></param>
        public void Detach(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            this.Detach(entity);
        }

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string</param>
        public void ExecuteSqlCommand(string sql)
        {
            this.Database.ExecuteSqlCommand(sql);
        }
    }
}
