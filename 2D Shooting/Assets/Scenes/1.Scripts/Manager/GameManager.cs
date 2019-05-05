using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
public class GameManager : Singleton<GameManager>
{
    GameObject _player = null;
    /// <summary>
    /// PrefabsList. 0: Player, 1: Enemy
    /// </summary>
    public List<GameObject> _prefabList = new List<GameObject>();   //
    public List<Sprite[]> _spriteList = new List<Sprite[]>();           // 스프라이트 
    public List<GameObject> _enemyPool = new List<GameObject>();

    public FlightPattern[] _tempFlightArray = new FlightPattern[6];
    public FlightPattern[][] _flightPatternArray = new FlightPattern[100][];
    private void Start()
    {

    }


    public void SetPlayer()
    {
        _player = GameObject.Find("Player");
        Player _memPlayer = _player.AddComponent<Player>();
        Controller _memController = _player.AddComponent<Controller>();

        _memPlayer._status.hp = 100;
        _memPlayer._status.speed = 0.01f;

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            AIController flight = GameObject.Find("Enemy").GetComponent<AIController>();
            flight.sc();
        }
    }

    public void LoadData()
    {
        
        LoadEnemyData();
        LoadBossData();
        LoadBulletData();
        LoadMapData();
        LoadSprites();
        LoadPrefabs();
        LoadFlightPattern();
    }

    public void LoadEnemyData()
    {

    }

    public void LoadBossData()
    {

    }

    public void LoadBulletData()
    {

    }

    public void LoadMapData()
    {

    }

    public void LoadPrefabs()
    {
        GameObject obj = (GameObject)Resources.Load(StringTable.Prefab_Folder + "Player");
        _prefabList.Add(obj);
        obj = (GameObject)Resources.Load(StringTable.Prefab_Folder + "Enemy");
        _prefabList.Add(obj);
    }

    public void LoadSprites()
    {
        Sprite[] sp = Resources.LoadAll<Sprite>(StringTable.Sprite_Folder + "boss0");
        _spriteList.Add(sp);
        sp = Resources.LoadAll<Sprite>(StringTable.Sprite_Folder + "boss50");
        _spriteList.Add(sp);
        sp = Resources.LoadAll<Sprite>(StringTable.Sprite_Folder + "boss80");
        _spriteList.Add(sp);
        sp = Resources.LoadAll<Sprite>(StringTable.Sprite_Folder + "boss100");
        _spriteList.Add(sp);
        sp = Resources.LoadAll<Sprite>(StringTable.Sprite_Folder + "BoxBlue");
        _spriteList.Add(sp);
        sp = Resources.LoadAll<Sprite>(StringTable.Sprite_Folder + "BoxGreen");
        _spriteList.Add(sp);
        sp = Resources.LoadAll<Sprite>(StringTable.Sprite_Folder + "Coin");
        _spriteList.Add(sp);
        sp = Resources.LoadAll<Sprite>(StringTable.Sprite_Folder + "Enemy");
        _spriteList.Add(sp);
        sp = Resources.LoadAll<Sprite>(StringTable.Sprite_Folder + "PFB");
        _spriteList.Add(sp);
        sp = Resources.LoadAll<Sprite>(StringTable.Sprite_Folder + "PFB2");
        _spriteList.Add(sp);
        sp = Resources.LoadAll<Sprite>(StringTable.Sprite_Folder + "PFB3");
        _spriteList.Add(sp);
        sp = Resources.LoadAll<Sprite>(StringTable.Sprite_Folder + "Player");
        _spriteList.Add(sp);
        sp = Resources.LoadAll<Sprite>(StringTable.Sprite_Folder + "PLB2");
        _spriteList.Add(sp);
        sp = Resources.LoadAll<Sprite>(StringTable.Sprite_Folder + "PLB3");
        _spriteList.Add(sp);
        sp = Resources.LoadAll<Sprite>(StringTable.Sprite_Folder + "Power");
        _spriteList.Add(sp);
        sp = Resources.LoadAll<Sprite>(StringTable.Sprite_Folder + "PRB2");
        _spriteList.Add(sp);
        sp = Resources.LoadAll<Sprite>(StringTable.Sprite_Folder + "PFB3");
        _spriteList.Add(sp);
    }

    public void LoadFlightPattern()
    {
        //FlightPattern pattern = new FlightPattern();
        //FlightPattern pattern2 = new FlightPattern();
        //for (int i = 0; i < 3; ++i)
        //{
        //    pattern.dest = new Vector2(5.24f, -1.89f);
        //    pattern.arcIntensity = 2;
        //    pattern.time = 2.0f;
        //    pattern.delay = 0.3f;
        //    _tempFlightArray[0 + i * 2] = pattern;
        //    pattern2.dest = new Vector2(-5.24f, -1.89f);
        //    pattern2.arcIntensity = 2;
        //    pattern2.time = 2.0f;
        //    pattern2.delay = 0.0f;
        //    _tempFlightArray[1 + i * 2] = pattern2;
        //}

        List<int> countList = new List<int>();
        //var fpList = DatabaseManager.Get().SelectDataAll<FlightPattern>();

        for (int i = 0; i < 100; ++i)
        {
            var fpList = DatabaseManager.Get().SelectDataAll<FlightPattern>("WHERE index = @0", i);
            if(fpList != null)
            {
                //_flightPatternArray[i] = fpList;
            }
            else
            {
                break;
            }
        }


    }
}
