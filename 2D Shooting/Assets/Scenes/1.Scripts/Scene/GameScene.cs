using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public void Start()
    {
        //GameManager.Get().SetPlayer();
        GameManager.Get().LoadData();
    }
}
