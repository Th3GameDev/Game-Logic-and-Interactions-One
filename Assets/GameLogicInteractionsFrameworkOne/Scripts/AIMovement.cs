using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    private Waypoints _waypoints; // Reference to the Waypoints script, which holds the AI's path.
    private NavMeshAgent _agent; // Reference to the NavMeshAgent component used for movement.
   [SerializeField] private int _currentWaypointIndex; // Index of the current waypoint in the AI's path.

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
        if (_waypoints != null)
        {
            _agent.SetDestination(_waypoints.GetWaypointPosition(1));
        }
    }
}
