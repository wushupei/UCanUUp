using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsulatesActive : MonoBehaviour
{
    public GameObject ground;
    void OnTriggerEnter(Collider other) //触发时调用
    {
        if (other.tag == "Player")
        {         
            if (ground.activeInHierarchy == false)
            {
                ground.SetActive(true); //启用隔离地形
                other.GetComponent<Character>().BeRebornPos = other.transform.position; //重置主角重生点
            }
        }
    }
}
