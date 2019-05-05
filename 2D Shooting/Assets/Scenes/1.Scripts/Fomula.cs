using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fomula
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="start">출발지</param>
    /// <param name="dest">목적지</param>
    /// <param name="cp">조절점</param>
    /// <param name="t">시간변수 (0 ~ 1)</param>
    public static Vector2 BezierCurves(Vector2 start, Vector2 dest, Vector2 cp, float t)
    {
        if(t < 0 || t > 1)
        {
            Debug.Log("잘못된 t값 입니다");
            return new Vector2(0,0);
        }
        
        Vector2 v1 = Vector2.Lerp(start, cp, t);
        
        Vector2 v2 = Vector2.Lerp(cp, dest, t);

        return Vector2.Lerp(v1, v2, t);
    }
    
    
    //public static Vector2 Arc(Vector2 start, Vector2 dir, float length ,int intensity, float t)
    //{
    //    Vector2 dest = Vector2.zero;
    //    int mul = intensity > 0 ? 1 : -1;
    //
    //    switch (dir)
    //    {
    //     
    //    }
    //
    //
    //    return Vector2.zero;
    //}
}
