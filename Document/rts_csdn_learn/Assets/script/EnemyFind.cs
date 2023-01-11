using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFind : MonoBehaviour
{
    public Enemy enemy;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Unit")
        {
            if(enemy.tempTarget==null)
            {
                enemy.tempTarget = other.GetComponent<Life>();
            }
        }
    }


}
