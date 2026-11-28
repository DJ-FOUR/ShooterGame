using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float Smoothing = 5f;

    private GameObject player;
    private Vector3 offset;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Start is called before the first frame update
    private void Start()
    {
        offset = transform.position - player.transform.position;
    }
    private void FixedUpdate()
    {
        //通过插值实现相机的延迟跟踪
        transform.position = Vector3.Lerp(transform.position, offset + player.transform.position, Smoothing*Time.deltaTime);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
