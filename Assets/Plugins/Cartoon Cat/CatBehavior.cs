using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CatBehavior : MonoBehaviour {

    private NavMeshAgent agent;
    private Animator animator;

    private bool hasEaten = false;
    private bool bowlFilled = false;
    private bool bowlOnGround = false;

    public Transform target;
    public Transform playerLocation;
    private Transform bowl;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = target.position;

        animator = GetComponent<Animator>();
    }

    void Update () {

        // if cat hasnt eaten
        // if the food bowl is not on the ground
        // go to the player
        // if the food bowl is on the ground
        // if the food bowl is empty
        // go to the empty bowl
        // if the food bowl is not empty
        // go to the food
        // when the player teleports, wait 2 seconds then walk to the player then go idle





        animator.SetFloat("speed", agent.velocity.magnitude);

        if (!agent.pathPending)
        {
            Debug.Log("1");
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                Debug.Log("2");
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log("3");
                    // Done
                    if (!hasEaten && bowlOnGround)
                    {
                        Debug.Log("4");
                        animator.SetInteger("eat", 1);
                        Invoke("StopEating", 6);
                    }
                }
            }
        }
    }

    private void StopEating() {
        hasEaten = true;
        animator.SetInteger("eat", 0);
    }

    public void PlayerTeleported()
    {
        if (!hasEaten && !bowlOnGround)
        {
            // in 2 seconds go to the players location
            Invoke("GoToPlayerLocation", 2);
        }
    }

    private void GoToPlayerLocation()
    {
        agent.destination = playerLocation.position;
    }

    public void BowlOnTheGround(bool hasFood, Transform bowl)
    {
        this.bowl = bowl;
        bowlOnGround = true;
        if (!hasEaten)
        {
            // in 2 seconds go to the bowls location
            Invoke("GoToBowlLocation", 0.5f);
        }
        
    }

    private void GoToBowlLocation()
    {
        agent.destination = bowl.position;
    }

    public void FoodPutIntoBowl(bool isOnGround)
    {

    }
}
