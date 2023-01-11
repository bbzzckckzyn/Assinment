using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    public float hp = 100;
    Animator ani;
    private void Start()
    {
        ani = GetComponent<Animator>();
    }

    public void Hurt(float t)
    {
        hp -= t;
        ani.CrossFade("Hurt", 0.2f);
        if(hp<=0f)
        {
            Dead();
        }
    }

    public void Dead()
    {
        ani.Play("dead");
        Invoke("Clear", 1f);
        if(this.tag=="Enemy")
        {
            Manager.ins.money += 50;
        }
    }

    void Clear()
    {
        Destroy(this.gameObject);
    }

}
