namespace Data
{
    /// <summary>
    /// EF仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EfRepository<TEntity> : EFRepositoryBase<TEntity, int>, IRepository<TEntity> where TEntity : class
    {
        public EfRepository(EFDbContext context) : base(context) { }
    }

    /// <summary>
    /// EF仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public class EfRepository<TEntity, TPrimaryKey> : EFRepositoryBase<TEntity, TPrimaryKey> where TEntity : class
    {
        public EfRepository(EFDbContext context) : base(context) { }
    }
}
