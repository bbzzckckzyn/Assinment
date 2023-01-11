using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindEnemy : MonoBehaviour
{
    public base_ base_;
    public bool isFind = false;
    public GameObject t;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy" && !isFind && base_.state == State.idle)
        {
            base_.attack_arrow(other.transform.position + Vector3.up);
            isFind = true;
            t = other.gameObject;
        }
    }

    float setIsFind = 0f;
    private void Update()
    {
        if(isFind && setIsFind<3f)
        {
            setIsFind += Time.deltaTime;
            if (setIsFind > 3f)
            {
                isFind = false;
                setIsFind = 0f;
            }
        }
    }
}
