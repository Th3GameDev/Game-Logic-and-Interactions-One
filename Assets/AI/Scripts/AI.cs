using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    private Waypoints _waypoints; // Reference to the Waypoints script, which holds the AI's path.
    private NavMeshAgent _agent; // Reference to the NavMeshAgent component used for movement.
    private int _currentWaypointIndex; // Index of the current waypoint in the AI's path.
    private bool _hasStarted; // Flag indicating if the AI has started moving.
    private bool _isMovingForward = true; // Flag indicating if the AI is moving forward.
    private bool _isAttacking; // Flag indicating if the AI is currently attacking.
    private bool _isJumping; //Flag indicating if the AI is currently Jumping.

    // Enumeration representing the different states of the AI.
    private enum AIState { Walking, Jumping, Attacking, Death }
    [SerializeField] private AIState _currentState; // The current state of the AI.

    void Start()
    {
        // Get references to the NavMeshAgent component and Waypoints script.
        _agent = GetComponent<NavMeshAgent>();
        _waypoints = FindObjectOfType<Waypoints>();

        if (_waypoints == null)
        {
            Debug.LogError("Failed To Find Waypoints Script");
        }

        _currentWaypointIndex = 0; // Start at the first waypoint in the AI's path.
        _hasStarted = false; // Initialize to false as the AI has not started moving yet.
    }

    void Update()
    {
        // If the 'E' key is pressed, stop the AI and set its state to Jumping.
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_currentState != AIState.Jumping)
            {
                _currentState = AIState.Jumping; // Set the AI's state to Jumping.
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _currentState = AIState.Death;
        }

        // Based on the current state, execute the corresponding behavior.
        switch (_currentState)
        {
            case AIState.Walking:
                Debug.Log("Walking");
                AIMovement(); // Execute the walking behavior.
                break;

            case AIState.Jumping:
                Debug.Log("Jumping");
                if (_isJumping == false)
                {
                    _isJumping = true;
                    _agent.isStopped = true; // Stop the AI's movement.
                    StartCoroutine(AIJumping());
                }
                break;

            case AIState.Attacking:
                Debug.Log("Attacking");

                if (_isAttacking == false)
                {
                    _isAttacking = true;
                    StartCoroutine(AIAttack()); // Start attacking behavior using a coroutine.
                }
                break;

            case AIState.Death:
                Debug.Log("Death");
                AIDeath(); //Trigger the AI Death behavior.
                break;
        }
    }

    // AI's walking behavior, moving towards the waypoints.
    void AIMovement()
    {
        // Start moving to the first waypoint if not started yet and waypoints are available.
        if (!_hasStarted && _waypoints != null && _waypoints.GetWaypointCount() > 0)
        {
            _agent.SetDestination(_waypoints.GetWaypointPosition(_currentWaypointIndex));
            _hasStarted = true; // Set the hasStarted flag to true to indicate the AI has started moving.
        }

        // Check if the agent has reached the current waypoint.
        if (_agent.remainingDistance < 0.5f && !_agent.pathPending)
        {
            _agent.isStopped = true;
            _currentState = AIState.Attacking; // If the current waypoint is reached, set the AI's state to Attacking.          
        }
    }

    // Moves the AI to the next waypoint.
    void SetNextWaypoint()
    {
        // Check if waypoints are available.
        if (_waypoints == null || _waypoints.GetWaypointCount() == 0)
        {
            Debug.LogWarning("No waypoints available.");
            return;
        }

        // Check if the agent is moving forward or backward on the path and call the appropriate function.
        if (_isMovingForward)
        {
            MoveForward();
        }
        else
        {
            MoveBackward();
        }
    }

    // Move the AI forward to the next waypoint.
    void MoveForward()
    {
        // Move to the next waypoint by incrementing the currentWaypointIndex.
        _currentWaypointIndex++;

        // Check if the AI has reached the last waypoint.
        if (_currentWaypointIndex >= _waypoints.GetWaypointCount())
        {
            // If the AI reached the last waypoint, start moving backward by setting the isMovingForward flag to false.
            _currentWaypointIndex = _waypoints.GetWaypointCount() - 2;
            _isMovingForward = false;
        }

        // Set the destination to the next waypoint's position.
        _agent.SetDestination(_waypoints.GetWaypointPosition(_currentWaypointIndex));
    }

    // Move the AI backward to the previous waypoint.
    void MoveBackward()
    {
        // Move to the previous waypoint by decrementing the currentWaypointIndex.
        _currentWaypointIndex--;

        // Check if the AI has reached the first waypoint after moving backward.
        if (_currentWaypointIndex < 0)
        {
            _agent.isStopped = true; // Stop the AI's movement as it reached the starting point.
            return;
        }

        // Set the destination to the previous waypoint's position.
        _agent.SetDestination(_waypoints.GetWaypointPosition(_currentWaypointIndex));
    }

    void AIDeath()
    {
        Destroy(this.gameObject, 0.2f);
    }

    IEnumerator AIJumping()
    {
        // Placeholder code for the jumping behavior.
        // For example, you could add animations or jump actions here.
        yield return new WaitForSeconds(1f);

        // Reset the jumping state after the jump behavior is completed.
        _isJumping = false;

        _agent.isStopped = false;

        // Set the AI's state back to Walking after attacking and moving.
        _currentState = AIState.Walking;
    }

    // Coroutine for the AI's attacking behavior.
    IEnumerator AIAttack()
    {
        // Placeholder code for the attacking behavior.
        // For example, you could add animations or attack actions here.

        yield return new WaitForSeconds(1f);

        // Reset the attacking state after the attack behavior is completed.
        _isAttacking = false;

        _agent.isStopped = false;

        SetNextWaypoint();

        // Set the AI's state back to Walking after attacking and moving.
        _currentState = AIState.Walking;
    }
}

