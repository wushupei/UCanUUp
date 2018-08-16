using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    Rigidbody rig; //刚体   
    Vector3 selfSize; //主角自身大小
    Vector3 collSize; //主角碰撞器大小
    public Vector3 BeRebornPos; //重生点
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        selfSize = transform.localScale;
        collSize = GetComponent<BoxCollider>().bounds.size;
        BeRebornPos = transform.position;
    }

    ///////////////////////////////////////////////////移动方法/////////////////////////////////////////////
    public float dir; //获取水平输入决定移动方向
    public float moveSpeed; //移动速度
    public void Move() //移动
    {
        rig.velocity = new Vector3(dir * moveSpeed * Time.deltaTime, rig.velocity.y, rig.velocity.z); //移动
        if (dir == 0) //当没有输入时,X轴速度为0
            rig.velocity = new Vector3(0, rig.velocity.y, rig.velocity.z);
    }

    ///////////////////////////////////////////////////跳跃方法/////////////////////////////////////////////
    public float jumpHigh; //跳跃高度
    public int jumpCount = 0; //跳跃次数
    float roDir; //旋转方向
    public ParticleSystem jumpEffect; //二段跳特效
    float getRoDir(float dir)
    {
        if (dir < 0) //根据当前水平输入决定旋转方向
            return 1;
        else
            return -1;
    }
    public void Jump() //跳跃
    {
        if (jumpCount < 2) //如果跳跃次数小于2,可以还跳一次
        {
            if (jumpCount == 1)
                jumpEffect.Play();
            jumpCount++; //跳跃次数+1
            rig.Sleep(); //消除下落重力
            rig.AddForce(Vector3.up * jumpHigh, ForceMode.Impulse); //跳
            roDir = getRoDir(dir); //根据当前输入获取旋转方向
        }
    }

    ///////////////////////////////////////////////////碰撞器检测/////////////////////////////////////////////
    bool IsGround(Transform terrain, bool posNex) //判定主角和地形的位置关系
    {
        Vector3 groundSize = terrain.GetComponent<BoxCollider>().bounds.size; //获取地形碰撞器大小
        float off_X = Mathf.Abs(collSize.x + groundSize.x) / 2; //获取主角和地面碰撞器X轴长度和的一半(尺寸偏差)
        float dis_X = Mathf.Abs(transform.position.x - terrain.position.x); //获取主角同地面的X轴距离        
        return dis_X < off_X && posNex;
    }
    void OnCollisionEnter(Collision other)//开始碰撞时调用
    {
        if (other.transform.tag == "Terrain") //和地面碰撞
            //如果X轴距离小于尺寸偏差,且主角位置比地面高,判定主角在地面上
            if (IsGround(other.transform, transform.position.y > other.transform.position.y))
            {
                jumpCount = 0; //重置跳跃次数 
                ext = true; //可以挤压     
            }
    }
    bool isRotate; //是否可旋转
    void OnCollisionStay(Collision other)//碰撞中调用
    {
        isRotate = false; //和物体接触时禁止旋转
        if (other.transform.tag == "Terrain") //和墙面触发
            //如果主角既不在地面上,也不再地面下
            if (!IsGround(other.transform, transform.position.y > other.transform.position.y) &&
                !IsGround(other.transform, transform.position.y < other.transform.position.y))
                if (dir != 0) //当有水平输入,主角往上走
                {
                    rig.velocity = new Vector3(rig.velocity.x, Mathf.Abs(dir) * moveSpeed * Time.deltaTime, rig.velocity.z);
                    jumpCount = 1; //靠墙可以无限跳
                }
    }
    void OnCollisionExit(Collision other)//离开碰撞时调用        
    {
        isRotate = true; //可以旋转
        roDir = getRoDir(dir); //根据当前输入获取旋转方向
    }

    ///////////////////////////////////////////////////挤压方法/////////////////////////////////////////////
    public Transform son_Ext; //在编辑器把负责挤压的子物体拖进去
    public bool ext = false; //是否可以挤压
    public void Extrusion() //挤压
    {
        Vector3 shape = new Vector3(1.2f, 0.8f, 1);  //挤压幅度
        if (ext)
        {
            son_Ext.localScale = Vector3.Lerp(son_Ext.localScale, shape, Time.deltaTime * 10); //挤压
            if (Mathf.Abs(son_Ext.localScale.x - shape.x) < 0.02f) //接近临界值
                ext = false; //停止挤压
        }
        else
        {
            son_Ext.localScale = Vector3.Lerp(son_Ext.localScale, Vector3.one, Time.deltaTime * 10); //恢复
            if (Mathf.Abs(son_Ext.localScale.x - 1) < 0.02f)//接近原尺寸
                son_Ext.localScale = Vector3.one; //恢复原尺寸
        }
    }

    ///////////////////////////////////////////////////旋转方法/////////////////////////////////////////////
    public Transform son_Ro; //在编辑器把负责旋转的子物体拖进去
    public void AirRotate() //子物体空中旋转
    {
        if (isRotate == true)
            son_Ro.Rotate(0, 0, roDir * 1080 * Time.deltaTime);
        else
            RotateStop();
    }
    public void RotateStop() //停止子物体旋转
    {
        float euler = son_Ro.eulerAngles.z; //获取当前子物体选择角度
        if (euler > 315 || euler <= 45)
            son_Ro.eulerAngles = new Vector3(0, 0, 0);
        else if (euler > 225)
            son_Ro.eulerAngles = new Vector3(0, 0, 270);
        else if (euler > 135)
            son_Ro.eulerAngles = new Vector3(0, 0, 180);
        else if (euler > 45)
            son_Ro.eulerAngles = new Vector3(0, 0, 90);
    }
    public GameObject grave; //在编辑器将坟墓预制体拖进去
    public void Death() //死亡
    {
        Instantiate(grave, transform.position, Quaternion.identity);//原地留下一个坟墓
        transform.position = BeRebornPos; //返回生成点
    }
}
