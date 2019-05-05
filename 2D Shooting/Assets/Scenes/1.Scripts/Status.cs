using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KM;
[TableName("EnemyStatus")]
public class Status
{
    public int hp { get; set; }                                    // 체력
    public float speed { get; set; }                            // 속도? 필요한지는 모르겠음
    public int prefabIndex { get; set; }                    // 기체 텍스쳐 인덱스
    public int destroyEffectIndex { get; set; }                  // 파괴 이펙트
    public int bulletIndex { get; set; }                       // 총알 인덱스
    public int bulletPatternIndex { get; set; }                   // 총알 패턴 인덱스
    public int shootingPatternIndex { get; set; }
    //public float hitBox_x;                           // 충돌 박스 가로
    //public float hitBox_y;                           // 충돌 박스 세로
    //public float hitBoxPosition_x;                   // 충돌 박스 위치
    //public float hitBoxPosition_y;                   // 충돌 박스 위치
    //public float muzzlePosition_x;                   // 총구 위치
    //public float muzzlePosition_y;                   // 총구 위치
    public float spawnDelay { get; set; }                        // 이전에 생성된 오브젝트와의 시간차 

    public int possesItemPatternIndex { get; set; }
    public int score { get; set; }
    public int flightPatterIndex { get; set; }
    public float spawnPlace_x { get; set; }
    public float spawnPlace_y { get; set; }
    // 아이템 패턴
    // 점수
    // 비행 패턴
    // 생성 위치

}
