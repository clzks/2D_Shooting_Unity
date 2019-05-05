using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public float t = 0.0f;
    public float tUpper = 0.0f;
    public Vector2 dest = Vector2.zero;
    public Vector2 start = new Vector2(-2.65f, 4.74f);
    public int arcIntensity = 0;
    [SerializeField] private Vector2 cp = Vector2.zero;


    public Vector2 TempCenter = Vector2.zero;
    public Vector2 TempMoveDirection = Vector2.zero;
    public void Awake()
    {
        start = transform.position;
    }

    public IEnumerator Flight(int flightPattern)
    {
        FlightPattern[] Array = GameManager.Get()._tempFlightArray;
        int i = 0;

        while (Array.Length > i)
        {
            arcIntensity = Array[i].arcIntensity;
            t = 0.0f;
            tUpper = 0.0f;
            start = transform.position;
            dest = (Vector2)transform.position + new Vector2(Array[i].dest_x, Array[i].dest_y);
            tUpper = 1.0f / Array[i].time;
            i++;
            yield return new WaitForSeconds(Array[i - 1].time + Array[i - 1].delay);
        }
        
    }

   
    public void sc()
    {
        StartCoroutine(Flight(0));
    }

    public void Update()
    {
        t += tUpper * Time.deltaTime;
        transform.position = MovePattern(start, dest, arcIntensity, t, 0);
    }

    /// <summary>
    /// 호의 각도에 따른 이동방식
    /// </summary>
    /// <param name="start">시작점</param>
    /// <param name="dest">목적지</param>
    /// <param name="arcIntensity">호의 강도 (음수 : 왼쪽으로 휘는 정도, 양수 : 오른쪽으로 휘는 정도)</param>
    /// <param name="t">시간상수(0 ~ 1)</param>
    /// <param name="speedPattern">속도 패턴(추후 추가 예정 ㅋ)</param>
    /// <returns></returns>
    public Vector2 MovePattern(Vector2 start, Vector2 dest, int arcIntensity, float t, int speedPattern)
    {
        cp = Vector2.zero;
        Vector2 CenterPoint = Vector2.Lerp(start, dest, 0.5f);
        Vector2 MoveDirection =  new Vector2(start.y * -0.5f + dest.y * 0.5f , start.x * 0.5f - dest.x * 0.5f);
       
        TempCenter = CenterPoint;
        if (arcIntensity < -3)
        {
            arcIntensity = -3;
        }
        else if(arcIntensity > 3)
        {
            arcIntensity = 3;
        }

        MoveDirection = MoveDirection * 0.5f * arcIntensity;
        cp = CenterPoint + MoveDirection;
        TempMoveDirection = MoveDirection;
        if (t > 1.0f)
        {
            t = 1.0f;
        }

        return Fomula.BezierCurves(start, dest, cp, t);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 255);
        Gizmos.DrawLine(start, dest);

        Gizmos.DrawSphere(cp, 0.05f);
        Gizmos.DrawSphere(TempCenter, 0.05f);
        Gizmos.DrawLine(TempCenter, TempCenter + TempMoveDirection);

        Gizmos.color = new Color(255, 0, 0);
        Gizmos.DrawSphere(start, 0.05f);
        Gizmos.DrawSphere(dest, 0.05f);
    }
}
