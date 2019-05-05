using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KM;

[TableName("Test")]
[PrimaryKey("_id")]
public class TestModel
{
    [ColumnName("id")]
    public int _id { get; set; }

    [ColumnName("desc")]
    public string _desc { get; set; }

    [ColumnName("name")]
    public string _characterName { get; set; }

    [Ignore]
    public DateTime updateDate { get; set; }

    public TestModel()
    {
        _id = 0;
        _characterName = "";
        _desc = "";
    }

    public override string ToString()
    {
        string propData = "";
        foreach (var item in GetType().GetProperties())
        {
            propData += item.GetValue(this).ToString() + "\n";
        }
        return propData;
    }
}
