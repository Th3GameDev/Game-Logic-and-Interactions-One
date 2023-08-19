using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    private Waypoints _waypoints; // Reference to the Waypoints script, which holds the AI's path.
    private NavMeshAgent _agent; // Reference to the NavMeshAgent component used for movement.
   [SerializeField] private int _currentWaypointIndex; // Index of the current waypoint in the AI's path.

    private bool _hasStarted; // Flag indicating if the AI has started moving.

    // Start is called before the first frame update
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
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
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
            MoveForward();
        }

        if (_currentWaypointIndex == _waypoints.GetWaypointCount())
            _agent.isStopped = true;
    }

    void MoveForward()
    {
        // Move to the next waypoint by incrementing the currentWaypointIndex.
        _currentWaypointIndex++;

        // Set the destination to the next waypoint's position.
        _agent.SetDestination(_waypoints.GetWaypointPosition(_currentWaypointIndex));
    }
}
