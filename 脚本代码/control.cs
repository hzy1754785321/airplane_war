using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class control : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler {
    public void OnEndDrag(PointerEventData eventData)    
    {
        transform.position = m_StartPos;
        isDrag = false;
    }
    //获得Image初始位置，最后回归位置
    Vector3 m_StartPos;
    Vector3 m_PlayerDir;

    protected player m_player;
    public void OnDrag(PointerEventData eventData)
    {
        //得到Image的代表的按钮移动的方向。
        Vector3 dir = Input.mousePosition - m_StartPos;
        //注意是Input.mousPosition 的距离，
        if (Vector3.Distance(m_StartPos, Input.mousePosition) < 23f)
        {
            transform.position = Input.mousePosition;
        }
        else
        {
            //固定Image的移动范围。
            transform.position = m_StartPos + dir.normalized * 23f;
        }
        isDrag = true;
        m_PlayerDir = new Vector3(dir.x, 0f, dir.y);
    }

    public GameObject m_Player;
    Rigidbody m_rigi;
    Vector3 m_start;
    bool isDrag = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_StartPos = transform.position;
    }

    void Awake()
    {
        m_rigi = m_Player.GetComponent<Rigidbody>();
        m_start = m_rigi.position;
    }

    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<player>(); ;    //查找主角         
    }

    void Update()    //摇杆操作
    {
        int life = m_player.getLife();
        //停止拖动后物体停止移动，设置一个bool值，
        if (isDrag&&life>0)
        {
            //使用刚体的移动，
            m_rigi.MovePosition(m_rigi.position + m_PlayerDir * Time.deltaTime * 0.045f);
            if (m_rigi.position.x > 22 || m_rigi.position.x < -18 || m_rigi.position.z > 12 || m_rigi.position.z < -10)
            {
                m_rigi.MovePosition(m_start);
            }
        }
           
    }
 
}

