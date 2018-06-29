using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Common, //普通状态
    Injured, //受伤状态
    Skidding, //滑行状态
    Death, //死亡状态
}
public class Character : MonoBehaviour
{
    public PlayerState state = PlayerState.Common;
    float injuredTimer; //受伤时间

    Rigidbody2D r2d; //刚体
    AnimationManager am; //动画管理类
    SpriteRenderer sr; //渲染器
    public float walkSpeed; //行走最大速度
    public float runSpeed; //奔跑最大速度
    float moveSpeed; //实际移动速度

    public Vector2 pSize; //声明主角碰撞器大小
    public LayerMask groundLayer; //设定成只能检测地面层
    Collider2D hit; //获取射线信息
    public float jumpHeight; //跳跃高度
    public float SlideDistance; //滑行距离

    BoxCollider2D box; //普通碰撞器

    void Start()
    {
        r2d = GetComponent<Rigidbody2D>();
        am = GetComponent<AnimationManager>();
        sr = GetComponentInChildren<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        IsFall();
        RecoveryOfInjury();
        SlideUp();

        MatchingCollider();
    }

    void MatchingCollider() //匹配碰撞器
    {
        switch (state)
        {
            case PlayerState.Skidding:
                SetCollider(-1.2f, 2.5f, 2);
                break;
            case PlayerState.Death:
                SetCollider(-1, 3, 2);
                break;
            default:
                SetCollider(0, 2, 4.4f);
                break;
        }
    }
    void SetCollider(float offsetY, float sizeX, float sizeY) //设置碰撞器
    {
        box.offset = new Vector2(0, offsetY);
        box.size = new Vector2(sizeX, sizeY);
    }
    void IsFall() //下落检测
    {
        //检测到速度在下落,播放下落动画
        if (r2d.velocity.y < -0.1f)
        {
            am.PlayFall(true);
            am.PlayJump(false);
            am.PlaySlide(false);
            if (state == PlayerState.Skidding)
                state = PlayerState.Common;
        }
        else
            am.PlayFall(false);
    }
    void RecoveryOfInjury() //受伤恢复
    {
        if (state == PlayerState.Injured) //如果受伤则开始计时
        {
            sr.material.color = new Color(1, 1, 1, Random.value); //主角闪动
            injuredTimer += Time.deltaTime;
        }
        if (injuredTimer > 0.5f) //如果受伤时间超过0.5秒,恢复为普通状态
        {
            am.PlayHurt(false);
            sr.material.color = new Color(1, 1, 1);
            state = PlayerState.Common;
            injuredTimer = 0;
        }
        else if (state != PlayerState.Injured) //如果在受伤时状态被改变
        {
            sr.material.color = new Color(1, 1, 1); //取消闪动
            injuredTimer = 0; //计时归零
        }
    }
    void SlideUp() //滑行起身
    {
        AnimatorStateInfo stateinfo = am.at.GetCurrentAnimatorStateInfo(0);
        if (state == PlayerState.Skidding) //如果进入滑行状态
        {
            //如果该动画播放完了
            if (stateinfo.IsName("Base Layer.Slide") && stateinfo.normalizedTime > 1.0f)
            {
                am.PlaySlide(false);
                state = PlayerState.Common; //变为普通状态
                r2d.Sleep();
            }
        }
        //在播放滑行动画时,状态发生改变,停止滑行动画
        if (stateinfo.IsName("Base Layer.Slide") && state != PlayerState.Skidding)
        {
            am.PlaySlide(false);
        }
    }
    //判断是否在地面
    bool IsGround()
    {
        //获取主角碰撞器大小
        pSize = box.bounds.size;
        //获取碰撞器对角线的一半
        float diagonal = Mathf.Sqrt(pSize.x * pSize.x + pSize.y * pSize.y) / 2;
        //发射射线检测地面获取地面信息
        hit = Physics2D.OverlapCircle(transform.position, diagonal, groundLayer);
        if (hit) //如果成功获取
        {
            //获取地面碰撞器大小
            Vector2 gSize = hit.GetComponent<BoxCollider2D>().bounds.size;
            //获取主角和地面的Y轴距离
            float distanceY = Mathf.Abs(transform.position.y - hit.transform.position.y);
            //获取主角和地面的y轴偏移量
            float yOffset = (pSize.y + gSize.y) / 2;
            //如果y轴距离大于等于偏移量则在地面
            return distanceY + 0.02f >= yOffset;         
        }
        else return false;
    }
    //移动 
    public void Move(float direction, bool input)
    {
        //奔跑或行走
        if (input)
            moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, 0.1f);
        else
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, 0.1f);
        //移动
        transform.Translate(moveSpeed * direction * Time.deltaTime, 0, 0, Space.World);
        //播放动画
        am.PlayWalk(direction);
        am.PlayRun(direction, input);
        //精灵翻转
        if (direction > 0)
            sr.flipX = false;
        if (direction < 0)
            sr.flipX = true;
    }
    //跳跃
    public void Jump()
    {
        if (IsGround()) //如果在地面
        {
            r2d.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse); //起跳
            am.PlayJump(true);
        }
    }
    //滑行
    public void Slide(float direction)
    {
        if (IsGround()) //如果在地面 
        {
            state = PlayerState.Skidding; //变为滑行状态
            am.PlaySlide(true);
            r2d.AddForce(Vector2.right * moveSpeed * direction * SlideDistance, ForceMode2D.Impulse); //向前滑行       
        }
    }
    //受伤
    public void Hurt(Transform enemy)
    {
        state = PlayerState.Injured; //变成受伤状态                                                                         
        Vector3 direction = transform.position - enemy.position; //获取主角与敌人的方向向量
        r2d.Sleep(); //消除主角身上所有惯性
        r2d.AddForce(direction * 50 + Vector3.down * 40, ForceMode2D.Impulse); //主角往该方向弹开
        am.PlayHurt(true);
    }
    //死亡
    public void Death()
    {
        state = PlayerState.Death;
        if (state == PlayerState.Death)
            am.PlayDead();
    }
}
