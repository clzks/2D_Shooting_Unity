using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public Status _status;
    //public Airframe _airframe;
    public int MoveState = 0;               // 0: Strate, 1 : Left, 2 : Right

    public void Awake()
    {
        _status = new Status();
        //_airframe = new Airframe();
    }

    public void Update()
    {
        
    }

    public void FixedUpdate()
    {
        
    }

    /// <summary>
    /// Move Object. 1 : Left, 2 : Right, 3 : Up, 4: Down
    /// </summary>
    /// <param name="i"></param>
    public void Move(int i)
    {
        switch (i)
        {
            case 0:
                MoveState = 0;
                break;

            case 1:
                transform.Translate(Vector2.left * _status.speed);
                MoveState = 1;
                break;

            case 2:
                transform.Translate(Vector2.right * _status.speed);
                MoveState = 2;
                break;

            case 3:
                transform.Translate(Vector2.up * _status.speed);
                break;

            case 4:
                transform.Translate(Vector2.down * _status.speed);
                break;
        }
        
    }
}
