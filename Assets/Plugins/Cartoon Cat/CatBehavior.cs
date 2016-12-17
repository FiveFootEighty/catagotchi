using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CatBehavior : MonoBehaviour {

    private NavMeshAgent agent;

    public Transform target;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = target.position;
    }

    void Update () {
        Debug.Log(agent.velocity.magnitude);
	}
}
