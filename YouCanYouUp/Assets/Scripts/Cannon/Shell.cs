using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    RaycastHit hit;
    public float rayPos; //射线位置
    public float rayDir; //射线方向
    void Update()
    {
        if (Physics.Raycast(transform.position + Vector3.right * rayPos, Vector3.right * rayDir, out hit, 0.1f))//使用射线检测前方障碍物    
            gameObject.SetActive(false); //禁用自身
    }
    void OnTriggerEnter(Collider other) //触发一次
    {
        if (other.tag == "Player") //触发主角
        {
            other.GetComponent<Character>().Death(); //主角死亡
            gameObject.SetActive(false); //禁用自身
        }
    }
}