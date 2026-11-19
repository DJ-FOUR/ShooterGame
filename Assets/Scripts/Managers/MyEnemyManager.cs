using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemyManager : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject CreateEnemyPoint;

    public float FirstCreatENemyTime = 0;
    public float CreatEnemyTime = 3;
    
    void Start()
    {
        InvokeRepeating("Spawn", FirstCreatENemyTime, CreatEnemyTime);     //固定频率重复调用
    }

    void Update()
    {

    }
    private void Spawn()
    {
        Instantiate(Enemy,CreateEnemyPoint.transform.position, CreateEnemyPoint.transform.rotation); 
    }
}
