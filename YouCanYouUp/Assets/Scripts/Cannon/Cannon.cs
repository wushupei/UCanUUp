using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    GameObject shellObj; //声明炮弹
    Shell shell; //声明炮弹脚本
    float fireDir; //发射方向
    RaycastHit hit;
    LayerMask layer;
    void Start()
    {
        shellObj = transform.GetChild(0).gameObject; //获取炮弹(子物体)
        shell = shellObj.GetComponent<Shell>();

        layer = 1 << 8; //只检测地形
        if (Physics.Raycast(transform.position, Vector3.right, out hit, 1, layer))//往右发射射线判定炮弹发射方向
        {
            fireDir = -5; 
            shell.rayPos = -0.2f; //炮弹射线发射位置
            shell.rayDir = -0.2f; //炮弹射线发射方向
        }
        else
        {
            fireDir = 5;
            shell.rayPos = 0.2f;
            shell.rayDir = 0.2f;
        }
    }
    void Update()
    {
        Fire();
    }
    float lifeTimer; //炮弹生命周期
    void Fire() //开炮
    {
        if (shellObj.activeInHierarchy == false)
        {
            lifeTimer = 0; //计时结束
            shellObj.transform.position = transform.position; //从炮口发射
            shellObj.SetActive(true); //启用
            shellObj.GetComponent<Rigidbody>().velocity = new Vector3(fireDir, 0, 0);
            Fire(); //递归循环调用
        }
        else
            lifeTimer += Time.deltaTime; //炮弹启用后开始计时
        if (lifeTimer >= 5) //5秒后
            shellObj.SetActive(false); //禁用炮弹      
    }
}
