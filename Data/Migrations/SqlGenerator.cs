using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Sql;

namespace Data.Migrations
{
    public class SqlGenerator : MySql.Data.Entity.MySqlMigrationSqlGenerator
    {
        /// <summary>
        /// 在添加数据迁移的时候, 过滤掉dbo.这些奇怪的操作符, MySQL会报错
        /// </summary>
        /// <param name="migrationOperations"></param>
        /// <param name="providerManifestToken"></param>
        /// <returns></returns>
        public override IEnumerable<MigrationStatement> Generate(IEnumerable<MigrationOperation> migrationOperations, string providerManifestToken)
        {
            IEnumerable<MigrationStatement> res = base.Generate(migrationOperations, providerManifestToken);
            foreach (MigrationStatement ms in res)
            {
                ms.Sql = ms.Sql.Replace("dbo.", "");
            }
            return res;
        }
    }
}
