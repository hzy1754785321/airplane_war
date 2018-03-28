using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("MyGame/EnemySpawn")]
public class EnemySpawn : MonoBehaviour {
    public Transform m_enemy;      //敌人的perfab
    protected Transform m_transform;
	// Use this for initialization
	void Start () {
        m_transform = this.transform;
        StartCoroutine(SpawnEnemy());    //调用协程
	}

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(Random.Range(3,8));  //每5~15秒生成一个敌人
        Instantiate(m_enemy,m_transform.position,Quaternion.identity);  
        StartCoroutine(SpawnEnemy());    //循环
    }
	
	void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "item.png", true);
    }
}
