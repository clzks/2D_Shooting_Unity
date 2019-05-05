using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using UnityEngine;

namespace KM
{
    public class SqliteDatabase
    {
        private static object _lock = new object();
        private SqliteConnection _dbConn = null;

        public SqliteDatabase(string connString)
        {
            _dbConn = new SqliteConnection(connString);
        }

        public void Destroy()
        {
            if (null != _dbConn)
            {
                try
                {
                    if (ConnectionState.Closed != _dbConn.State)
                    {
                        _dbConn.Close();
                    }
                }
                catch (SqliteException e)
                {
                    Console.WriteLine(e.Data.ToString());
                }
            }
        }

        public int Insert<T>(T insertData)
        {
            lock (_lock)
            {
                int sqlResult = 0;
                OpenDatabase();
                SqliteCommand dbCmd = _dbConn.CreateCommand();
                {
                    string tableName = GetTableName<T>();
                    string colNames = GetInsertColumn<T>(false);
                    string colNames2 = GetInsertColumn<T>(true);
                    string sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", tableName, colNames, colNames2);
                    dbCmd.CommandText = sql;
                    AddParams(insertData, ref dbCmd);

                    try
                    {
                        sqlResult = dbCmd.ExecuteNonQuery();
                        if (1 != sqlResult)
                        {
                            Debug.Log("Insert 쿼리 실패. " + insertData);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                }
                dbCmd.Dispose();
                CloseDatabase();

                return sqlResult;
            }
        }

        /// <summary>
        /// SELECT 쿼리
        /// </summary>
        /// <param name="sqlWhere">
        /// WHERE절에 인자값 치환 문자열은 @0 부터 시작하여 인자 개수에 맞게 순차 증가.(@0, @1, @2...)
        /// i.e> "WHERE name = @0 AND desc = @1", name, desc
        /// </param>
        /// <param name="paramList">
        /// 인자값 입력
        /// </param>
        /// <returns>열거형 데이터 반환</returns>
        public IEnumerable<T> Query<T>(string sqlWhere = "", params object[] paramList)
        {
            lock (_lock)
            {
                int sqlResult = 0;
                OpenDatabase();
                List<T> result = new List<T>();
                SqliteCommand dbCmd = _dbConn.CreateCommand();
                string tableName = GetTableName<T>();
                DataSet ds = new DataSet(tableName);
                {
                    string whereStatement = sqlWhere;
                    if ("" != whereStatement)
                    {
                        for (int i = 0; i < paramList.Length; i++)
                        {
                            dbCmd.Parameters.AddWithValue("@" + i.ToString(), paramList[i]);
                        }
                    }

                    dbCmd.CommandText = string.Format("SELECT * FROM {0} {1}", tableName, whereStatement);
                    SqliteDataAdapter dataAdapter = new SqliteDataAdapter(dbCmd);
                    sqlResult = dataAdapter.Fill(ds);
                    if (0 != sqlResult)
                    {
                        int rowCount = ds.Tables[0].Rows.Count;
                        for (int i = 0; i < rowCount; i++)
                        {
                            result.Add((T)MappingRowToObject<T>(ds.Tables[0].Rows[i]));
                        }
                    }
                    else
                    {
                        Debug.Log("Query 실행 오류.");
                    }
                }

                dbCmd.Dispose();
                CloseDatabase();

                return result;
            }
        }

        public object SingleOrNull<T>(object id)
        {
            lock (_lock)
            {
                object result = null;
                OpenDatabase();
                string tableName = GetTableName<T>();
                string whereStatement = GetUniqueWhereStatement<T>();
                DataSet ds = new DataSet(tableName);
                var dbCmd = _dbConn.CreateCommand();
                {
                    dbCmd.CommandText = string.Format("SELECT * FROM {0} WHERE {1}", tableName, whereStatement);
                    AddParamId<T>(id, ref dbCmd);
                    SqliteDataAdapter dataAdapter = new SqliteDataAdapter(dbCmd);
                    int sqlResult = dataAdapter.Fill(ds);
                    if (1 == sqlResult)
                    {
                        if (null != ds.Tables[0].Rows
                               && 1 == ds.Tables[0].Rows.Count)
                        {
                            result = MappingRowToObject<T>(ds.Tables[0].Rows[0]);
                        }
                    }
                    else
                    {
                        Debug.Log("SingleOrNull 쿼리 실패. id : " + id.ToString());
                    }
                }
                dbCmd.Dispose();
                CloseDatabase();

                return result;
            }
        }

        public int Update<T>(T updateData)
        {
            lock (_lock)
            {
                int sqlResult = 0;
                OpenDatabase();
                SqliteCommand dbCmd = _dbConn.CreateCommand();
                {
                    string tableName = GetTableName<T>();
                    string colNames = GetUpdateColumn<T>();
                    string whereStatement = GetUniqueWhereStatement<T>();
                    string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", tableName, colNames, whereStatement);
                    dbCmd.CommandText = sql;
                    AddParams(updateData, ref dbCmd);
                    AddParamId<T>(GetPrimaryKey(updateData), ref dbCmd);

                    try
                    {
                        sqlResult = dbCmd.ExecuteNonQuery();
                        if (1 != sqlResult)
                        {
                            Debug.Log("Update 쿼리 실패. " + updateData);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                }
                dbCmd.Dispose();
                CloseDatabase();

                return sqlResult;
            }
        }

        public int Delete<T>(object id)
        {
            string whereStatement = GetUniqueWhereStatement<T>();
            return Delete<T>(whereStatement, id);
        }

        public int Delete<T>(string whereStatement = "", params object[] paramList)
        {
            lock (_lock)
            {
                int sqlResult = 0;
                OpenDatabase();
                SqliteCommand dbCmd = _dbConn.CreateCommand();
                {
                    string tableName = GetTableName<T>();
                    string sql = string.Format("DELETE FROM {0} {1}", tableName, whereStatement);
                    dbCmd.CommandText = sql;
                    if ("" != whereStatement)
                    {
                        for (int i = 0; i < paramList.Length; i++)
                        {
                            dbCmd.Parameters.AddWithValue("@" + i.ToString(), paramList[i]);
                        }
                    }

                    try
                    {
                        sqlResult = dbCmd.ExecuteNonQuery();
                        if (1 != sqlResult)
                        {
                            Debug.Log("DELETE 쿼리 실패.");
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                }
                dbCmd.Dispose();
                CloseDatabase();

                return sqlResult;
            }
        }

        private void OpenDatabase()
        {
            _dbConn.Open();
        }

        private void CloseDatabase()
        {
            _dbConn.Close();
        }
        
        private object GetPrimaryKey<T>(T data)
        {
            string pkPropName = "id";
            var pk = typeof(T).GetCustomAttribute<PrimaryKey>();
            if (null != pk)
            {
                pkPropName = pk.GetPrimaryKeyName();
            }

            var prop = data.GetType().GetProperty(pkPropName);
            if (null != prop)
            {
                return prop.GetValue(data);
            }

            return null;
        }

        private bool IsPrimaryKey<T>(PropertyInfo propInfo)
        {
            if (0 == string.Compare(GetColumnName(propInfo), GetPrimaryKeyName<T>(), true))
            {
                return true;
            }

            return false;
        }

        private string GetPrimaryKeyName<T>()
        {
            var attr = typeof(T).GetCustomAttribute<PrimaryKey>();
            if (null != attr)
            {
                return GetColumnName(typeof(T).GetProperty(attr.GetPrimaryKeyName()));
            }
            return "id";
        }

        private bool IsIgnore(PropertyInfo propInfo)
        {
            var colAttrs = propInfo.GetCustomAttributes();
            foreach (var attr in colAttrs)
            {
                if (typeof(Ignore) == attr.GetType())
                {
                    return true;
                }
            }

            return false;
        }

        private string GetSelectColumn(PropertyInfo[] props)
        {
            string selectCol = "";
            for (int i = 0; i < props.Length; i++)
            {
                if (true == IsIgnore(props[i]))
                {
                    continue;
                }

                selectCol += string.Format("{0}, ", GetColumnName(props[i]));
            }

            selectCol = selectCol.Substring(0, selectCol.Length - 2);

            return selectCol;
        }

        private void AddParamId<T>(object id, ref SqliteCommand sqlCmd)
        {
            string paramName = "@" + GetPrimaryKeyName<T>();
            sqlCmd.Parameters.AddWithValue(paramName, id);
        }

        private void AddParams<T>(T insertObj, ref SqliteCommand sqlCmd)
        {
            var props = typeof(T).GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                string colName = GetColumnName(props[i]);
                if (true == IsIgnore(props[i]) || true == IsPrimaryKey<T>(props[i]))
                {
                    continue;
                }

                SqliteParameter param = new SqliteParameter("@" + colName);
                object paramValue = null;
                if (typeof(DateTime) == props[i].PropertyType)
                {
                    var dtType = props[i].GetCustomAttribute<DateTimeType>();
                    if (null != dtType)
                    {
                        if (typeof(string) == dtType.GetDatetimeType())
                        {
                            paramValue = ((DateTime)props[i].GetValue(insertObj)).ToString();
                        }
                        else
                        {
                            paramValue = ((DateTime)props[i].GetValue(insertObj)).ToBinary();
                        }
                    }
                    else
                    {
                        paramValue = ((DateTime)props[i].GetValue(insertObj)).ToBinary();
                    }
                }
                else
                {
                    paramValue = props[i].GetValue(insertObj);
                }
                param.Value = paramValue;
                sqlCmd.Parameters.Add(param);
            }
        }

        private string GetInsertColumn<T>(bool isVal = false)
        {
            string insertCol = "";
            var props = typeof(T).GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                if (true == IsIgnore(props[i]))
                {
                    continue;
                }

                string colName = GetColumnName(props[i]);
                if (0 != string.Compare(GetPrimaryKeyName<T>(), colName, true))
                {
                    if (true == isVal)
                    {
                        colName = "@" + colName;
                    }

                    insertCol += colName + ", ";
                }
            }
            insertCol = insertCol.Substring(0, insertCol.Length - 2);

            return insertCol;
        }

        private string GetUpdateColumn<T>()
        {
            string sql = "";
            var props = typeof(T).GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                if (true == IsIgnore(props[i]) || true == IsPrimaryKey<T>(props[i]))
                {
                    continue;
                }

                string colName = GetColumnName(props[i]);
                sql += string.Format("{0} = @{0}, ", colName);
            }

            sql = sql.Substring(0, sql.Length - 2);

            return sql;
        }

        private string GetUniqueWhereStatement<T>()
        {
            string whereStatement = "";
            whereStatement = string.Format("{0} = @{0}", GetPrimaryKeyName<T>());

            return whereStatement;
        }

        private string GetTableName<T>()
        {
            var attrs = typeof(T).GetCustomAttributes();
            foreach (var attr in attrs)
            {
                if (typeof(TableName) == attr.GetType())
                {
                    return ((TableName)attr).GetTableName();
                }
            }

            return typeof(T).Name;
        }

        private string GetColumnName(PropertyInfo propInfo)
        {
            var attrs = propInfo.GetCustomAttributes();
            foreach (var attr in attrs)
            {
                if (typeof(ColumnName) == attr.GetType())
                {
                    return ((ColumnName)attr).GetColumnName();
                }
            }

            string propName = propInfo.Name;
            return propName;
        }

        private object MappingRowToObject<T>(DataRow r)
        {
            object result = Activator.CreateInstance(typeof(T));
            var props = typeof(T).GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                if (true == IsIgnore(props[i]))
                {
                    continue;
                }

                string colName = GetColumnName(props[i]);
                var colValue = r[colName];
                object value = null;
                if (typeof(DateTime) == props[i].PropertyType)
                {
                    var dtType = props[i].GetCustomAttribute<DateTimeType>();
                    if (null != dtType)
                    {
                        if (typeof(string) == dtType.GetDatetimeType())
                        {
                            string strDatetime = (string)Convert.ChangeType(r[colName], typeof(string));
                            if (true == DateTime.TryParse(strDatetime, out DateTime resultDatetime))
                            {
                                value = resultDatetime;
                            }
                            else
                            {
                                value = new DateTime();
                            }
                        }
                        else
                        {
                            value = DateTime.FromBinary((long)r[colName]);
                        }
                    }
                    else
                    {
                        value = DateTime.FromBinary((long)r[colName]);
                    }
                }
                else
                {
                    value = Convert.ChangeType(r[colName], props[i].PropertyType);
                }
                props[i].SetValue(result, value);
            }

            return result;
        }
    }
}
