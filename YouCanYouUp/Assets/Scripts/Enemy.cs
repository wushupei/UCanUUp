using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody rig; //声明刚体
    void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }
    RaycastHit hit;
    bool changeDir; //是否改变方向
    float moveSpeed = 100; //移动速度
    void Update()
    {
        LoopMove(); //移动
    }
    void LoopMove() //来回移动
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f)) //往下发射射线检测
            changeDir = false; //当射线打到东西时不用改变方向
        else if (changeDir == false) //当没打到东西
        {
            changeDir = true; //可以改变方向
            moveSpeed *= -1; //改变移动方向
        }
        rig.velocity = new Vector3(moveSpeed * Time.deltaTime, rig.velocity.y, rig.velocity.z); //移动
    }
    void OnCollisionEnter(Collision player) //碰撞检测
    {

        if (player.transform.tag == "Player") //如果和主角碰撞
        {
            Vector3 playerPos = player.transform.position; //获取主角当前坐标
            Vector3 selfPos = transform.position; //获取自身当前坐标
            float OffsetX = GetComponentInChildren<BoxCollider>().bounds.size.x / 2f; //获取偏移量(自身宽度的一半)
            //如果主角在自己上方，并且在x轴偏移量之内,判定被主角踩到
            if (playerPos.y > selfPos.y && Mathf.Abs(playerPos.x - selfPos.x) <= OffsetX)
            {
                player.rigidbody.AddForce(transform.up * 10, ForceMode.Impulse);//让主角往上弹
                Destroy(gameObject); //销毁自身               
            }
            else //否则就视为主角被攻击,调用主角死亡方法
                player.gameObject.GetComponent<Character>().Death();
        }
        else
            moveSpeed *= -1; //碰到两侧墙壁改变移动方向 
    }
}
