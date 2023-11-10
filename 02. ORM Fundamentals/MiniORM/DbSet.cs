namespace MiniORM
{
    using System.Collections;
    public class DbSet<TEntity> : ICollection<TEntity>
        where TEntity : class, new()
    {
        internal DbSet(IEnumerable<TEntity> entities)
        {
            this.Entities = entities.ToList();
            this.ChangeTracker = new ChangeTracker<TEntity>(entities);
        }
        internal IList<TEntity> Entities { get; set; }
        internal ChangeTracker<TEntity> ChangeTracker { get; set; }
        public int Count => this.Entities.Count;
        public bool IsReadOnly => this.Entities.IsReadOnly;
        public void Add(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), ExceptionMessages.EntityNullException);
            this.Entities.Add(entity);
            this.ChangeTracker.Add(entity);
        }
        public void Clear()
        {
            while (this.Entities.Any())
            {
                TEntity entitiesToDelete = this.Entities.First();
                this.Remove(entitiesToDelete);
            }
        }
        public bool Contains(TEntity entity)
            => this.Entities.Contains(entity);
        public void CopyTo(TEntity[] entities, int index)
            => this.Entities.CopyTo(entities, index);
        public bool Remove(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), ExceptionMessages.EntityNullException);
            var removedSuccessfully = Entities.Remove(entity);
            if(removedSuccessfully)
                ChangeTracker.Remove(entity);
            return removedSuccessfully;
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities.ToArray())
                Remove(entity);
        }

        public IEnumerator<TEntity> GetEnumerator()
            => this.Entities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }
}
