using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    NavMeshAgent nav;
    Animator ani;
    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        attackTarget = Manager.ins.castle;
        ani = GetComponent<Animator>();
    }
    public float attackGap = 2f;
    float tempAttackGap = 0f;
    bool canAttack = true;

    public Life attackTarget;
    public Life tempTarget;
    private void Update()
    {


        if(tempAttackGap > attackGap && !canAttack)
        {
            canAttack = true;
            tempAttackGap = 0f;
        }
        else
        {
            tempAttackGap += Time.deltaTime;
        }
        if(isAttackCastle)
        {
            if(canAttack)
            {
                attack(attackTarget);
                return;
            }
        }

        if(tempTarget==null)
        {
            nav.SetDestination(attackTarget.transform.position);
            if (Vector3.Distance(transform.position, attackTarget.transform.position)<7f)
            {
                if(canAttack)
                {
                    attack(attackTarget);
                    canAttack = false;
                }
            }
        }
        else
        {
            nav.SetDestination(tempTarget.transform.position);
            print(Vector3.Distance(transform.position, tempTarget.transform.position));
            if (Vector3.Distance(transform.position, tempTarget.transform.position) < 1.5f)
            {
                if (canAttack)
                {
                    attack(tempTarget);
                    canAttack = false;
                }
            }
        }

       
    }

    Life t;
    void attack(Life life)
    {
        transform.LookAt(life.transform);
        ani.CrossFade("attack", 0.2f);
        canAttack = false;
        t = life;
    }
    public void EnemyHurt()
    {
        t.Hurt(50f);
    }
    bool isAttackCastle = false;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Castle")
        {
            //isAttackCastle = true;

            //attack(attackTarget);
        }
    }
}
