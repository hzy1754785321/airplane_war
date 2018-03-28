using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MyGame/SuperEnemy")]
public class SuperEnemy :Enemy
{
 //   public Transform m_rocket;     
 //   public float m_fireTime = 2;    //射击计时器

  /*  void Awake()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");    //查找主角
        if(obj!=null)
        {
            m_player = obj.transform;
        }
    }
   */
    protected override void UpdateMove()
    {
        m_fireTime = m_fireTime - Time.deltaTime;
        if(m_fireTime<=0)
        {
            m_fireTime = 2;
            if(m_player!=null)
            {
                Vector3 relativePos = m_transform.position - m_player.position;  //计算子弹初始方向，使其面向主角
                Instantiate(m_rocket, m_transform.position,Quaternion.LookRotation(relativePos));   //创建出子弹
                                                          //Quatenion是计算四元数，用于3D旋转    LookRotation是注视旋转，使z轴对齐
            }
        }
        m_transform.Translate(new Vector3(0, 0, -m_speed * Time.deltaTime));
    }
}
    
	// Use this for initialization
