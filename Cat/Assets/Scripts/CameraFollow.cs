using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
    }
    void LateUpdate()
    {
        //获取玩家的坐标
        Vector3 pos = player.transform.position;
        //获取摄像机和玩家的距离
        float distance = Vector3.Distance(transform.position, pos); 
        //距离越远摄像机跟随越快
        transform.position = Vector3.Lerp(transform.position, pos + Vector3.back + Vector3.up, Time.deltaTime * distance);
    }
}
