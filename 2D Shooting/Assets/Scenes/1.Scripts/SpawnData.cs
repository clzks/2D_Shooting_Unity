using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KM;
[TableName("SpawnData")]
public class SpawnData
{
    public float time { get; set; }
    public float position_x { get; set; }
    public float position_y { get; set; }
    public int prefabIndex { get; set; }
    public int flightPattern { get; set; }
    public int quantity { get; set; }
    public float delay { get; set; }
}
