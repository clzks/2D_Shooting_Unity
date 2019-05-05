using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KM;

[TableName("FlightPattern")]
public class FlightPattern
{
    /// <summary>
    /// dest : 현재 위치를 기준으로 더해줄 값
    /// </summary>
    public int index { get; set; }
    public float dest_x { get; set; }
    public float dest_y { get; set; }
    public int arcIntensity { get; set; }
    public float time { get; set; }
    public float delay { get; set; }
    public int speedPattern { get; set; }
}
