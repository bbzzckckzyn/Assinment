using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum State
{
    idle,
    move,
    attack
}
public class base_ : MonoBehaviour
{
    #region start
    public NavMeshAgent agent;
    public Animator animator;
    AudioSource audio;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        outline_ = GetComponent<Outline>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        //属性 test
        hp = 100f;
    }
    #endregion

    #region 状态机
    [SerializeField]
    public State state;
    public State State
    {
        get { return state; }
        set
        {
            state = value;
            switch(state)
            {
                case State.idle:
                    animator.CrossFade("crossbow_01_idle", 0.2f);

                    break;
                    case State.move:
                    Debug.Log("switch move");
                    animator.CrossFade("crossbow_03_run", 0.2f);
                    
                    break;
                case State.attack:
                    animator.CrossFade("crossbow_04_attack_A", 0.2f);

                    break;
            }
        }
    }
    public void SetIdle()
    {
        if(State != State.idle )State = State.idle;
    }
    #endregion

    #region state_on_update
    void state_on_update()
    {
        switch(state)
        {
            case State.idle:


                break;
            case State.move:
                if(Vector3.Distance(transform.position,position)<1.5f)
                {
                    State = State.idle;
                }

                break;
            case State.attack:


                break;
        }
    }
    #endregion

    #region 动作
    Vector3 position;
    public void move(Vector3 pos)
    {
        position = pos;
        State = State.move;

        agent.isStopped = false;
        agent.SetDestination(pos);
    }

    public GameObject arrow;
    GameObject arrow_temp;
    public Transform arrow_pos;
    Vector3 arrow_target;
    public void attack_arrow(Vector3 pos)
    {
        State = State.attack;
        transform.LookAt(pos);
        arrow_target = pos;

    }
    public void shoot_arrow()
    {
        if (arrow_temp == null) return;
        arrow_temp.SetActive(true);
        arrow_temp.transform.position = arrow_pos.position;
        arrow_temp.GetComponent<ArrowLine>().shoot_(arrow_target);
        audio.Play();
    }
    public void reload()
    {
        arrow_temp = Instantiate(arrow, arrow_pos.position, Quaternion.identity);
        arrow_temp.SetActive(false);
    }

    #endregion

    #region 选取
    public bool is_select;
    Outline outline_;
    public void switch_select()
    {
        if (is_select)
        {
            is_select = false;
            outline_.enabled = false;
        }
        else
        {
            is_select = true;
            outline_.enabled = true;
        }
    }

    #endregion

    #region 属性
    [SerializeField] private float hp = 100f;
    public void set_hp(float t)
    {
        hp -= t;
        if(hp < 0f || hp == 0f)
        {
            death();
        }
    }

    void death()
    {
        animator.CrossFade("death", 0.2f);
        Invoke("clear_self", 3f);
    }
    void clear_self()
    {
        Destroy(this.gameObject);
    }

    #endregion

    private void Update()
    {
        state_on_update();
    }
}
