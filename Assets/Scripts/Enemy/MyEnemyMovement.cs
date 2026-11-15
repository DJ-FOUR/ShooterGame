using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyEnemyMovement : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent nav;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        nav=GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(player.transform.position);
    }
}
