namespace MiniORM
{
    using System.Reflection;
    using System.ComponentModel.DataAnnotations;
    internal class ChangeTracker<T>
        where T : class, new()
    {
        private readonly IList<T> _allEntities;
        private readonly IList<T> _added;
        private readonly IList<T> _removed;

        private ChangeTracker()
        {
            _added = new List<T>();
            _removed = new List<T>();
        }

        public ChangeTracker(IEnumerable<T> entities)
            : this()
        {
            _allEntities = this.CloneEntities(entities);
        }

        public IReadOnlyCollection<T> AllEntities => (IReadOnlyCollection<T>)this._allEntities;
        public IReadOnlyCollection<T> Added => (IReadOnlyCollection<T>)this._added;
        public IReadOnlyCollection<T> Removed => (IReadOnlyCollection<T>)this._removed;
        public void Add(T entity) => _added.Add(entity);
        public void Remove(T entity) => _removed.Add(entity);


        public IEnumerable<T> GetModifiedEntities(DbSet<T> dbSet)
        {
            IList<T> modifiedEntities = new List<T>();
            PropertyInfo[] primaryKeys = typeof(T).GetProperties()
                .Where(pi => pi.HasAttribute<KeyAttribute>())
                .ToArray();
            foreach (var proxyEntities in this.AllEntities)
            {
                var primaryKeyValues = this.GetPrimaryKeyValues(primaryKeys, proxyEntities).ToArray();
                var entities = dbSet.Entities
                    .Single(e => GetPrimaryKeyValues(primaryKeys, e).SequenceEqual(primaryKeyValues));
                var isModified = IsModified(proxyEntities, entities);
                if(isModified)
                    modifiedEntities.Add(proxyEntities);
            }

            return modifiedEntities;
        }

        private IEnumerable<object> GetPrimaryKeyValues(PropertyInfo[] primaryKeys, T proxyEntities)
            => primaryKeys.Select(x => x.GetValue(proxyEntities));
        private bool IsModified(T proxyEntities, T entities)
        {
            var monitoredProp = typeof(T).GetProperties()
                .Where(pi => DbContext.AllowSqlTypes.Contains(pi.PropertyType));
            var modifiedProp = monitoredProp.Where(pi => !Equals(pi.GetValue(entities), pi.GetValue(proxyEntities)))
                .ToArray();
            return modifiedProp.Any();
        }

        private IList<T> CloneEntities(IEnumerable<T> entities)
        {
            var clonedEntities = new List<T>();
            var propertiesToClone = typeof(T).GetProperties()
                .Where(pi => DbContext.AllowSqlTypes.Contains(pi.PropertyType))
                .ToArray();
            foreach (var entity in entities)
            {
                T entityClone = Activator.CreateInstance<T>();
                foreach (PropertyInfo property in propertiesToClone)
                {
                    object originalValue = property.GetValue(entities);
                    property.SetValue(entityClone, originalValue);
                }

                clonedEntities.Add(entityClone);
            }

            return clonedEntities;
        }
    }
}