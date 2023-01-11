using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class test : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform pos;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(pos.position);
    }

    // Update is called once per frame
    void Update()
    {

        agent.SetDestination(pos.position);
    }
}
