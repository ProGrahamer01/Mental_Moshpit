using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementAnimated : MonoBehaviour
{
    //if made public id be able to drag and drop the player game object from the hierarchy to the inspector 
    private Transform player;

    private NavMeshAgent navMeshAgent;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>(); // or just GetComponent<Animator>() if it's on the same object

    }


    // Update is called once per frame
  void Update()
    {
        if (player != null && navMeshAgent != null)
        {
            navMeshAgent.SetDestination(player.position);
        }

        float speed = navMeshAgent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }

}
