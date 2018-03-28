using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

[AddComponentMenu("MyGame/GameManager")]
public class GameManager : MonoBehaviour {
    public static GameManager Instance;    //静态实例

    public Transform m_canvas_main;      //显示分数到UI界面
    public Transform m_canvas_gameover;   //游戏失败UI界面
    public Text m_text_score;        //显示得分UI文字
    public Text m_text_best;        //显示最高分UI文字
    public Text m_text_life;      //显示生命值UI文字

    protected int m_score=0;      //得分
    public static int m_best_score=0;  //最高分
    protected player m_player;    //主角

    public AudioClip m_musicClip;    //背景音乐
    public AudioSource m_Audio;    //音乐源
	// Use this for initialization

	void Start () {
        Instance = this;
        m_Audio = this.gameObject.AddComponent<AudioSource>();   //使代码添加音乐组件
        m_Audio.clip = m_musicClip;       //指定音源
        m_Audio.loop = true;            //循环播放
        m_Audio.Play();               //播放音乐

        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();  //获取主角

        //获取UI控件
        m_text_score = m_canvas_main.transform.Find("Test_score").GetComponent<Text>();
        m_text_best = m_canvas_main.transform.Find("Text_best").GetComponent<Text>();
        m_text_life = m_canvas_main.transform.Find("Text_life").GetComponent<Text>();
        m_text_score.text = string.Format("分数   {0}", m_score);       //初始化UI分数
        m_text_best.text = string.Format("最高分   {0}", m_best_score);    //初始化UI最高分
        m_text_life.text = string.Format("生命值   {0}", m_player.m_life);    //初始化UI生命值

        var restart_button = m_canvas_gameover.transform.Find("Button_restart").GetComponent<Button>();  //获取重新开始游戏按钮
        restart_button.onClick.AddListener(delegate()    //按钮事件回调
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);  //重新开始关卡
        });
        m_canvas_gameover.gameObject.SetActive(false);  //默认隐藏游戏失败UI
	}

    //增加分数
    public void AddScore(int point)
    {
        m_score = m_score + point;

        //更新最高分记录
        if (m_best_score < m_score)
                m_best_score = m_score;
        m_text_score.text = string.Format("分数  {0}", m_score);
        m_text_best.text = string.Format("最高分  {0}", m_best_score);
    }
	
    public void ChangeLife(int life)
    {
        m_text_life.text = string.Format("生命值  {0}", life);    //更新生命值UI
        if(life<=0)
        {
            m_canvas_gameover.gameObject.SetActive(true);   //如果生命为0，显示游戏失败UI
        }
    }
}
