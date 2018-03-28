using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MyGame/player")]
public class player : MonoBehaviour
{
    public float m_speed = 1;      //移动速度
    protected Transform m_transform;
    public Transform m_rocket;   //指向子弹体
    public float m_rocketTimer = 0;   //发射子弹速度
    public int m_life = 3;     //主角生命值
    public AudioClip m_shootClip;    //声音,用于与声音文件相关联
    protected AudioSource m_audio;   //声音源,用于播放声音
    public Transform m_explosionFX;    //爆炸特效
    protected Vector3 m_targetPos;    //目标位置
    public LayerMask m_inputMask;     //鼠标射线碰撞层
    Rigidbody m_move;  //刚体
    Vector3 m_start;  //保存初始位置
    // Use this for initialization

    void Awake()
    {
        m_move = this.GetComponent<Rigidbody>();   //获取刚体
        m_start = m_move.position;   //获取初始位置
    }
    void Start()
    {
        m_transform = this.transform;        //初始化一次后将其储存，transform是移动游戏体积
        m_audio = this.GetComponent<AudioSource>();    //将其指向实践的声音源组件
        m_targetPos = this.m_transform.position;      //初始化目标点位置
    }

    void MoveTo()      //鼠标操作
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 ms = Input.mousePosition;   //捕获鼠标的屏幕位置
            Ray ray = Camera.main.ScreenPointToRay(ms);   //将屏幕位置转为射线
            RaycastHit hitinfo;   //用来记录射线碰撞信息
            bool iscast = Physics.Raycast(ray, out hitinfo, 1000, m_inputMask);   //???
            if (iscast)
            {
                m_targetPos = hitinfo.point;
            }
        }

        Vector3 pos = Vector3.MoveTowards(this.m_transform.position, m_targetPos, m_speed * Time.deltaTime);
        //MoveTowards是Vector3的函数，作用获得朝目标移动的位置
        this.m_transform.position = pos;   //更新当前位置
    }
    // Update is called once per frame
    void Update()
    {
             float movev = 0;    //纵向移动距离
             float moveh = 0;    //横向移动距离

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        //键盘操作
          if(Input.GetKey(KeyCode.UpArrow))
          {
              movev = movev - m_speed * Time.deltaTime;     //deltatime  每帧时间
          }
		
          if(Input.GetKey(KeyCode.DownArrow))
          {
              movev = movev + m_speed * Time.deltaTime;
          }

          if(Input.GetKey(KeyCode.LeftArrow))
          {
              moveh = moveh + m_speed * Time.deltaTime;
          }

          if(Input.GetKey(KeyCode.RightArrow))
          {
              moveh = moveh - m_speed * Time.deltaTime;
          }

        

          this.m_transform.Translate(new Vector3(moveh, 0, movev));   //移动（x,y,z）  

          if (m_move.position.x > 25 || m_move.position.x < -16 || m_move.position.z > 12 || m_move.position.z < -10 && m_life>0)
          {
              m_move.MovePosition(m_start);
          }
        //MoveTo();

        m_rocketTimer = m_rocketTimer - Time.deltaTime;
        if (m_rocketTimer <= 0)
        {
            m_rocketTimer = 0.18f;
            if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))   //按住空格或鼠标左键发射子弹
            {
                Instantiate(m_rocket, m_transform.position, m_transform.rotation);
                m_audio.PlayOneShot(m_shootClip);     //播放射击声音
            }
        }
    }

    public int getLife()
    {
        return m_life;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag.CompareTo("PlayerRocket") != 0)
        {
            Instantiate(m_explosionFX, m_transform.position, Quaternion.identity);
            m_life = m_life - 1;
            GameManager.Instance.ChangeLife(m_life);     //更新生命值UI
            if (m_life <= 0)
            {
                Instantiate(m_explosionFX, m_transform.position, Quaternion.identity);   //死亡后，播放爆炸特效
                Destroy(this.gameObject);   //自我销毁
            }
        }
    }
}


