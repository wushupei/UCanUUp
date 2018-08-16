using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpButton : MonoBehaviour
{
    Vector3 selfPos; //声明自身当前位置
    TextMesh numberText; //金币数量显示
    TextMesh score; //分数显示
    void Start()
    {
        selfPos = transform.position; //记录自身当前位置
        numberText = transform.parent.GetComponentInChildren<TextMesh>();       
        numberText.text= Random.Range(10, 30).ToString(); //给金币一个随机数
        score = GameObject.Find("Score").GetComponent<TextMesh>();
    }
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, selfPos, 0.1f);//如果发生位移,回到原先位置
    }
    void OnCollisionEnter(Collision player)//开始碰撞时调用
    {
        if (player.transform.tag == "Player")
        {
            player.rigidbody.AddForce(Vector3.up * 10, ForceMode.Impulse);//主角往上弹
            player.transform.GetComponent<Character>().jumpCount = 1; //让弹起的主角可以二段跳
            transform.position = transform.position + Vector3.down; //按钮发生位移      
            
            int number = int.Parse(numberText.text); //将金币数字转化为整数
            if (number > 0) //当数量大于0
            {
                numberText.text = (--number).ToString(); //数量-1
                score.text=(int.Parse(score.text)+1).ToString(); //分数+1
            }
        }
    }
}