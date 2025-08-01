using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementAnimated : MonoBehaviour
{
    // Reference to the player's Transform (found by tag in Start)
    private Transform player;

    // Core components
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    [Header("AI Settings")]
    [SerializeField]
    private float detectionRadius;      // How close the player must be to detect
    private bool playerDetected = false;// Tracks if we've already “seen” the player

    [Header("Power-Up States")]
    [HideInInspector]
    public bool isEnraged = false;     // Set true by PowerUpManager when enrage begins
    public bool immuneToFreeze = false;// If true, this enemy never freezes

    void Start()
    {
        // Cache NavMeshAgent and Animator
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        // Find the player in the scene by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError($"[{name}] Player not found! Make sure it's tagged \"Player\".");
        }

        // Sanity checks
        if (navMeshAgent == null)
            Debug.LogError($"[{name}] NavMeshAgent component missing!");
        if (animator == null)
            Debug.LogError($"[{name}] Animator component missing!");
    }

    void Update()
    {
        // If any core component is null, bail out
        if (player == null || navMeshAgent == null || animator == null)
            return;

        // Compute the current distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // DEBUG: show distance each frame
        Debug.Log($"[{name}] DistanceToPlayer: {distanceToPlayer:F2}");

        // DETECTION: only fire once when the player enters radius OR if this enemy is enraged
        if (!playerDetected && (distanceToPlayer <= detectionRadius || isEnraged))
        {
            playerDetected = true;
            navMeshAgent.ResetPath();  // clear any old paths
            Debug.Log($"[{name}] Player detected! (Enraged: {isEnraged})");
        }

        // CHASE: if we've detected the player, always set destination to them
        if (playerDetected)
        {
            navMeshAgent.SetDestination(player.position);
            Debug.Log($"[{name}] Chasing player at {player.position}");
        }
        else
        {
            // Optional: idle behavior
            navMeshAgent.ResetPath();
        }

        // ANIMATION: feed movement into animator
        Vector3 localVel = transform.InverseTransformDirection(navMeshAgent.velocity);
        animator.SetFloat("MoveX", localVel.x, 0.1f, Time.deltaTime);
        animator.SetFloat("MoveZ", localVel.z, 0.1f, Time.deltaTime);
    }

    // Draws a yellow wire sphere in the Scene view when this object is selected
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
