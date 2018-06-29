using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator at;
    public int a = 5;
    void Start()
    {
        at = GetComponentInChildren<Animator>();
        a = 10;
    }
    //行走动画
    public void PlayWalk(float direction)
    {
        at.SetBool("Walk", direction != 0);
    }
    //奔跑动画
    public void PlayRun(float direction, bool input)
    {
        at.SetBool("Run", direction != 0 && input);
    }
    //滑行动画
    public void PlaySlide(bool play)
    {
        at.SetBool("Slide", play);
    }
    //跳跃动画
    public void PlayJump(bool play)
    {
        at.SetBool("Jump", play);
    }
    //落下动画
    public void PlayFall(bool play)
    {
        at.SetBool("Fall", play);
    }
    //受伤动画
    public void PlayHurt(bool play)
    {
        at.SetBool("Hurt", play);
    }
    //死亡动画
    public void PlayDead()
    {
        at.SetBool("Dead", true);
    }
}
