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
        //获取当前输入调用移动方法
        chara.dir = Input.GetAxis("Horizontal");
        chara.Move();
        //按空格跳跃
        if (Input.GetKeyDown(KeyCode.Space))
            chara.Jump();

        chara.AirRotate(); //旋转
        chara.Extrusion(); //挤压
    }
}
