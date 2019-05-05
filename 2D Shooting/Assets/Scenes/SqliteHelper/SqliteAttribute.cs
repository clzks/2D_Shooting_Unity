using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KM
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableName : Attribute
    {
        private string _tableName = "";

        public TableName(string tableName)
        {
            _tableName = tableName;
        }

        public string GetTableName()
        {
            return _tableName;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class PrimaryKey : Attribute
    {
        private string _primaryKey = "";

        public PrimaryKey(string primaryKey)
        {
            _primaryKey = primaryKey;
        }

        public string GetPrimaryKeyName()
        {
            return _primaryKey;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnName : Attribute
    {
        private string _colName = "";

        public ColumnName(string colName)
        {
            _colName = colName;
        }

        public string GetColumnName()
        {
            return _colName;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DateTimeType : Attribute
    {
        private Type _convertType = typeof(long);

        public DateTimeType(Type convertType)
        {
            _convertType = convertType;
        }

        public Type GetDatetimeType()
        {
            return _convertType;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class Ignore : Attribute
    {
    }
}
