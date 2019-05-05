using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Actor _parentActor;

    public void Start()
    {
        _parentActor = GetComponent<Actor>();
    }

    // Update is called once per frame
    void Update()
    {
        _parentActor.Move(0);

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            _parentActor.Move(1);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _parentActor.Move(2);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            _parentActor.Move(3);

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            _parentActor.Move(4);
        }
    }
}
