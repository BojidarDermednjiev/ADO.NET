namespace MiniORM
{
    using System.Reflection;
    using System.Collections;
    using Microsoft.Data.SqlClient;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class DbContext
    {
        private readonly DatabaseConnection _connection;
        private readonly IDictionary<Type, PropertyInfo> _dbSetPropties;
        protected DbContext(string connectionString)
        {
            this._connection = new DatabaseConnection(connectionString);
            this._dbSetPropties = DiscoverDbSets();
            using (new ConnectionManager(_connection))
            {
                InitializeDbSet();
            }

            MapAllRelations();
        }
        public static readonly Type[] AllowSqlTypes =
        {
            typeof(string),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(decimal),
            typeof(bool),
            typeof(DateTime)
        };
        private void InitializeDbSet()
        {
            foreach (KeyValuePair<Type, PropertyInfo> dbSet in _dbSetPropties)
            {
                Type dbSetType = dbSet.Key;
                PropertyInfo dbSetProp = dbSet.Value;
                MethodInfo populateDbSetGeneric = typeof(DbContext)
                    .GetMethod("PopulateDbSet", BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(dbSetType);
                populateDbSetGeneric.Invoke(this, new object[] { dbSetProp });
            }
        }

        private void PopulateDbSet<TEntity>(PropertyInfo dbSet)
            where TEntity : class, new()
        {
            IEnumerable<TEntity> entities = LoadTableEntities<TEntity>();
            DbSet<TEntity> dbSetInstance = new DbSet<TEntity>(entities);
            ReflectionHelper.ReplaceBackingField(this, dbSet.Name, dbSetInstance);
        }
        private void MapAllRelations()
        {
            foreach (var dbSetProperty in _dbSetPropties)
            {
                Type dbSetType = dbSetProperty.Key;
                var dbSet = dbSetProperty.Value.GetValue(this);
                MethodInfo mapRelationsGeneric = typeof(DbContext)
                    .GetMethod("MapRelations", BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(dbSetType);
                mapRelationsGeneric.Invoke(this, new[] { dbSet });
            }
        }
        public void MapRelations<TEntity>(DbSet<TEntity> dbSet)
            where TEntity : class, new()
        {
            Type entityType = typeof(TEntity);
            this.MapNavigationsProperties(dbSet);
            PropertyInfo[] collections = entityType
                .GetProperties()
                .Where(x => x.PropertyType.IsGenericType &&
                            x.PropertyType.GetGenericTypeDefinition() == typeof(ICollection))
                .ToArray();
            foreach (var collection in collections)
            {
                Type collectionType = collection.PropertyType.GenericTypeArguments.First();
                MethodInfo mapCollectionMethod = typeof(DbContext)
                    .GetMethod("MapCollection", BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(entityType, collectionType);
                mapCollectionMethod.Invoke(this, new object[] { dbSet, collections });
            }
        }
        private void MapCollection<TEntity, TCollection>(DbSet<TEntity> dbSet, PropertyInfo collection)
            where TEntity : class, new()
            where TCollection : class, new()
        {
            Type entityType = typeof(TEntity);
            Type collectionType = typeof(TCollection);

            PropertyInfo[] collectionTypePrimaryKeys = collectionType
                .GetProperties()
                .Where(x => x.HasAttribute<KeyAttribute>())
                .ToArray();

            PropertyInfo foreignKey = collectionTypePrimaryKeys.First();
            PropertyInfo primaryKey = entityType
                .GetProperties()
                .First(x => x.HasAttribute<KeyAttribute>());
            bool isManyToMany = collectionTypePrimaryKeys.Length > 2;
            if (isManyToMany)
            {
                foreignKey = collectionType
                    .GetProperties()
                    .First(x => collectionType
                        .GetProperty(x.GetCustomAttribute<ForeignKeyAttribute>().Name)
                        .PropertyType == entityType);
            }

            DbSet<TCollection> navigationsDbSet = (DbSet<TCollection>)this._dbSetPropties[collectionType].GetValue(this);
            foreach (TEntity entity in dbSet)
            {
                object primaryKeyValue = primaryKey.GetValue(entity);
                object navigationEntities = navigationsDbSet
                    .Where(x => foreignKey.GetValue(x).Equals(primaryKeyValue))
                    .ToArray();
                ReflectionHelper.ReplaceBackingField(entity, collection.Name, navigationEntities);
            }
        }
        private void MapNavigationsProperties<TEntity>(DbSet<TEntity> dbSet) where TEntity : class, new()
        {
            Type entityType = typeof(TEntity);
            PropertyInfo[] foreignKeys = entityType
                .GetProperties()
                .Where(x => x.HasAttribute<ForeignKeyAttribute>())
                .ToArray();
            foreach (var foreignKey in foreignKeys)
            {
                string navigationPropName = foreignKey.GetCustomAttribute<ForeignKeyAttribute>().Name;
                PropertyInfo navigationProp = entityType.GetProperty(navigationPropName);
                IEnumerable<object> navigationDbSet = (IEnumerable<object>)this._dbSetPropties[navigationProp.PropertyType].GetValue(this);
                PropertyInfo navigationEntityPrimaryKey = navigationProp.PropertyType.GetProperties()
                    .First(x => x.HasAttribute<KeyAttribute>());
                foreach (TEntity entity in dbSet)
                {
                    object foreignKeyValue = foreignKey.GetValue(entity);
                    object navigationPropValue = navigationDbSet.First(x =>
                        navigationEntityPrimaryKey.GetValue(x).Equals(foreignKeyValue));
                    navigationProp.SetValue(entity, navigationPropValue);

                }
            }
        }
        private IEnumerable<TEntity> LoadTableEntities<TEntity>()
            where TEntity : class, new()
        {
            Type table = typeof(TEntity);
            string[] columns = this.GetEntityColmnNames(table);
            string tableName = this.GetTableName(table);
            IEnumerable<TEntity> fetchedRows = _connection.FetchResultSet<TEntity>(tableName, columns).ToArray();
            return fetchedRows;
        }
        private string[] GetEntityColmnNames(Type enitityType)
        {
            string tableName = this.GetTableName(enitityType);
            string[] dbColumns = this._connection
                .FetchColumnNames(tableName)
                .ToArray();
            string[] columns = enitityType
                .GetProperties()
                .Where(x => dbColumns.Contains(x.Name)
                            && !x.HasAttribute<NotMappedAttribute>()
                            && AllowSqlTypes.Contains(x.PropertyType))
                .Select(x => x.Name)
                .ToArray();
            return columns;
        }
        public void SaveChanges()
        {
            var dbSets = this._dbSetPropties
                .Select(x => x.Value.GetValue(this))
                .ToArray();
            foreach (IEnumerable<object> dbSet in dbSets)
            {
                IEnumerable<object> invalidEntities = dbSet
                    .Where(x => !this.IsObjectValid(x))
                    .ToArray();
                if (invalidEntities.Any())
                    throw new InvalidOperationException(string.Format(ExceptionMessages.InvalidEntitiesException,
                        invalidEntities.Count(), dbSet.GetType().Name));
            }
            using (new ConnectionManager(_connection))
            {
                using (var transaction = _connection.StartTransaction())
                {
                    foreach (IEnumerable dbSet in dbSets)
                    {
                        Type dbSetType = dbSet.GetType().GetGenericArguments().First();
                        MethodInfo persistMethod = typeof(DbContext)
                            .GetMethod("Persist", BindingFlags.Instance | BindingFlags.NonPublic)
                            .MakeGenericMethod(dbSetType);

                        try
                        {
                            persistMethod.Invoke(this, new object[] { dbSet });
                        }
                        catch (TargetInvocationException tie)
                        {
                            throw tie.InnerException!;
                        }
                        catch (InvalidOperationException ioe)
                        {
                            transaction.Rollback();
                            throw ioe.InnerException!;
                        }
                        catch (SqlException se)
                        {
                            transaction.Rollback();
                            throw se.InnerException!;
                        }
                    }
                    transaction.Commit();
                }
            }
        }
        private bool IsObjectValid(object e)
        {
            ValidationContext validationContext = new ValidationContext(e);
            ICollection<ValidationResult> validationErrors = new List<ValidationResult>();
            var validation = Validator.TryValidateObject(e, validationContext, validationErrors, true);
            return validation;
        }
        private void Persist<TEntity>(DbSet<TEntity> dbSet)
            where TEntity : class, new()
        {
            var tableName = GetTableName(typeof(TEntity));
            var columns = _connection.FetchColumnNames(tableName).ToArray();
            if (dbSet.ChangeTracker.Added.Any())
                _connection.InsertEntities(dbSet.ChangeTracker.Added, tableName, columns);

            IEnumerable<TEntity> modifiedEntities = dbSet.ChangeTracker.GetModifiedEntities(dbSet).ToArray();
            if (modifiedEntities.Any())
                _connection.UpdateEntities(modifiedEntities, tableName, columns);
            if (dbSet.ChangeTracker.Removed.Any())
                _connection.DeleteEntities(dbSet.ChangeTracker.Removed, tableName, columns);
        }
        private string GetTableName(Type tableType)
        {
            string tableName = tableType.GetCustomAttribute<TableAttribute>()?.Name;
            if (tableName == null)
                tableName = _dbSetPropties[tableType].Name;
            return tableName;
        }
        private IDictionary<Type, PropertyInfo> DiscoverDbSets()
        {
            IDictionary<Type, PropertyInfo> dbSets = this.GetType()
                .GetProperties()
                .Where(x => x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .ToDictionary(key => key.PropertyType.GetGenericArguments().First(), x => x);
            return dbSets;
        }
    }
}
