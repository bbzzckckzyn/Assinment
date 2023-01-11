using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemy : MonoBehaviour
{
    public Transform pos;
    public GameObject enemy;

    float curTime = 0f;
    void create()
    {
        Instantiate(enemy, pos.position, Quaternion.identity);
        if (createTime > 5) createTime -= 0.5f;
    }
    float createTime = 10f;
    private void Update()
    {
        if(curTime > createTime)
        {
            create();
            curTime = 0f;
        }
        else
        {
            curTime += Time.deltaTime;
        }
    }
}
