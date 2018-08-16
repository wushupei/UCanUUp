using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; //在编辑器把主角拖进去
    public float high; //摄像机高度设置
    void Update()
    {
        Vector3 pos = transform.position; //获取自身坐标
        transform.position = Vector3.Lerp(transform.position, new Vector3(pos.x, player.position.y + high, pos.z), Time.deltaTime*5);
    }
}
