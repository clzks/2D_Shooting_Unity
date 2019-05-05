//#define DATABASE_SAMPLE_RUN
using KM;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using System;

public class DatabaseManager : Singleton<DatabaseManager>
{
    private string _dbName = "2D_Shooting_DataBase.db";
    private string _dbPath = "";
    private string _connString = "";
    private static SqliteDatabase _database = null;

    private void Awake()
    {
        InitDatabaseConnection();
#if DATABASE_SAMPLE_RUN
        //  Database 샘플 코드 실행 함수
        DoSample();
#endif
    }

    private void OnApplicationQuit()
    {
        if (null != _database)
        {
            _database.Destroy();
        }
    }

    private void InitString()
    {
        _connString = "URI=file:" + Path.Combine(Application.persistentDataPath, _dbName);
        _dbPath = Path.Combine(Application.persistentDataPath, _dbName);
    }

    private bool InitDatabaseConnection()
    {
        if (null == _database)
        {
            InitString();
            CopyDatabaseToPersistentDataPath();
            _database = new SqliteDatabase(_connString);
        }

        return true;
    }

    private void CopyDatabaseToPersistentDataPath()
    {
        var srcFile = File.ReadAllBytes(Path.Combine(Application.streamingAssetsPath, _dbName));
        File.WriteAllBytes(Path.Combine(Application.persistentDataPath, _dbName), srcFile);
    }
    
    public IEnumerable<T> SelectDataAll<T>(string sqlWhere = "", params object[] paramList)
    {
        return _database.Query<T>(sqlWhere, paramList);
    }
    
    public object SelectData<T>(object id)
    {
        return _database.SingleOrNull<T>(id);
    }
    
    public int InsertData<T>(T insertData)
    {
        return _database.Insert(insertData);
    }

    public int DeleteData<T>(object id)
    {
        return _database.Delete<T>(id);
    }
    
    public int DeleteData<T>(string sqlWhere = "", params object[] paramList)
    {
        return _database.Delete<T>(sqlWhere, paramList);
    }

    public int UpdateData<T>(T updateData)
    {
        return _database.Update(updateData);
    }

    /// <summary>
    /// 샘플 코드
    /// </summary>
    private void DoSample()
    {
        //  INSERT SAMPLE
        TestModel t = new TestModel
        {
            _desc = "abc" + Time.deltaTime.ToString(),
            _characterName = "namename",
            updateDate = new DateTime(2019, 3, 17)
        };
        InsertData(t);                          //  인스턴스 t를 테이블에 추가함.

        //  SELECT ALL SAMPLE
        var list = SelectDataAll<TestModel>().ToList();     //  Table 전체 가져옴.

        //  SELECT WITH WHERE STATEMENT
        var whereList = SelectDataAll<TestModel>("WHERE id > @0", 0);   //  id 값이 2보다 큰 행들을 가져옴.

        //  SELECT ONE SAMPLE
        var find = SelectData<TestModel>(2);    //  PrimaryKey 값이 2인 행을 가져옴.

        //  UPDATE SAMPLE
        list[0]._characterName = "new name";
        int result = UpdateData(list[0]);                    //  list[0]의 PrimaryKey 값으로 행 갱신
        Debug.Log(result);

        var list2 = SelectDataAll<TestModel>().ToList();

        //  DELETE SAMPLE
        DeleteData<TestModel>("WHERE id = @0", list[3]._id);     //  PrimaryKey로 행 삭제
    }
}
