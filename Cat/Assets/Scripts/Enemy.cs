using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //碰撞
    private void OnCollisionEnter2D(Collision2D obj)
    {
        if (obj.transform.tag == "Player"&&obj.transform.GetComponent<Character>().state!=PlayerState.Death) //如果和主角碰撞
        {
            Vector3 playerPos = obj.transform.position; //获取主角当前坐标
            Vector3 selfPos = transform.position; //获取自身当前坐标
            float OffsetX = GetComponentInChildren<CircleCollider2D>().radius; //敌人是个圆,所以获取它的半径大小
            //如果主角在自己上方，并且在x轴偏移量之内,判定被主角踩到
            if (playerPos.y > selfPos.y && Mathf.Abs(playerPos.x - selfPos.x) <= OffsetX * 3)
            {
                obj.transform.GetComponent<Rigidbody2D>().Sleep();
                obj.transform.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 100, ForceMode2D.Impulse); //让主角往上弹
                obj.transform.GetComponent<AnimationManager>().PlayJump(true); //播放跳跃动画
            }
            else //否则就视为主角被攻击        
                obj.transform.GetComponent<Character>().Hurt(transform); //调用主角受伤方法           
        }
    }
}