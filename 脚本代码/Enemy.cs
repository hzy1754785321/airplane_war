using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MyGane/Enemy")]
public class Enemy : MonoBehaviour {

    public float m_speed = 1;   //速度
    public float m_life = 10;    //生命
    protected float m_roSpeed = 30;   //旋转速度
    public int m_point = 10;    //分数
    public Transform m_rocket;     //子弹
    public float m_fireTime = 4;    //射击计时器
    protected Transform m_player;   //主角
    public Transform m_transform;
    internal Renderer m_renderer;     //模型渲染组件
    internal bool m_isActiv=false;    //是否激活
    public Transform m_explosionFX;
	// Use this for initialization
	void Start () {
        m_transform = this.transform;
        m_renderer = this.GetComponent<Renderer>();    //获得模型渲染组件
	}

    void Awake()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");    //查找主角
        if (obj != null)
        {
            m_player = obj.transform;
        }
    }

    void OnBecameVisible()    //当模型进入屏幕
    {
        m_isActiv = true;
    }

	// Update is called once per frame
	void Update () {
        UpdateMove();
       
        if(m_isActiv&& !this.m_renderer.isVisible)    //如果模型移动到屏幕外
        {
            Destroy(this.gameObject);
        }
	}

   
    protected virtual void UpdateMove()
    {
        m_fireTime = m_fireTime - Time.deltaTime;
        if (m_fireTime <= 0)
        {
            m_fireTime = 2;
            if (m_player != null)
            {
                Vector3 relativePos = m_transform.position - m_player.position;  //计算子弹初始方向，使其面向主角
                Instantiate(m_rocket, m_transform.position, Quaternion.LookRotation(relativePos));   //创建出子弹
                //Quatenion是计算四元数，用于3D旋转    LookRotation是注视旋转，使z轴对齐
            }
        }
        float rx = Mathf.Sin(Time.time) * Time.deltaTime;   //左右移动
        m_transform.Translate(new Vector3(rx, 0, -m_speed * Time.deltaTime)); //前进
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag.CompareTo("PlayerRocket")==0)     //如果被子弹撞到
        {
            Rocket rocket = other.GetComponent<Rocket>();
            if(rocket!=null)
            {
                Instantiate(m_explosionFX, m_transform.position, Quaternion.identity);
                m_life = m_life - rocket.m_power;     //生命减少
                if(m_life<=0)
                {
                    GameManager.Instance.AddScore(m_point);    //更新UI上的分数
                    Instantiate(m_explosionFX, m_transform.position, Quaternion.identity);  //爆炸特效
                    Destroy(this.gameObject);        //自我销毁
                }
            }
        }
        else if(other.tag.CompareTo("Player")==0)      //如果主角被子弹撞到
        {
            m_life = 0;
            Destroy(this.gameObject);                //自我销毁
        }
    }


}
