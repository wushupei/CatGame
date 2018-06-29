using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    Character chara;

    void Start()
    {
        chara = GetComponent<Character>();
    }
    void Update()
    {
        float h = Input.GetAxis("Horizontal"); //获取水平方向输入
        float v = Input.GetAxis("Vertical"); //获取垂直方向输入

        if (chara.state == PlayerState.Common)
        {
            float direction = Input.GetAxis("Horizontal");
            chara.Move(direction, Input.GetKey(KeyCode.Space));

            if (Input.GetKeyDown(KeyCode.J))
                chara.Jump();

            if (Input.GetKeyDown(KeyCode.L))
                chara.Slide(direction);
        }
    }
}
