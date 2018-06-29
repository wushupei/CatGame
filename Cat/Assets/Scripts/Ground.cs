using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    //碰撞
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player") //如果主角落到地上       
        {
            float py = collision.transform.GetComponent<Character>().pSize.y; //主角y轴尺寸
            float gy = GetComponent<BoxCollider2D>().bounds.size.y; //地面y轴尺寸
            float yOffset = (py + gy) / 2; //获取y轴尺寸偏移量

            float distance = Mathf.Abs(collision.transform.position.y - transform.position.y); //y轴坐标距离
            if (distance >= yOffset) //如果y轴距离超过y轴尺寸偏移量         
                collision.transform.GetComponent<AnimationManager>().PlayJump(false); //跳跃结束
        }
    }
}
