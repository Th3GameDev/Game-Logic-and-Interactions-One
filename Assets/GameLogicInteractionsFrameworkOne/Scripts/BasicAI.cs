using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicAI : MonoBehaviour
{
    private Waypoints _waypoints;
    private NavMeshAgent _agent;
    private Animator _anim;

    private enum AIState { Running, Hiding, Death }
    [SerializeField] private AIState _currentState;

    private bool _hasStarted;
    private bool _isHiding;
    private bool _isDying;

    private int _maxHealth = 100;
    [SerializeField] private int _currentHealth = 0;

    [SerializeField] private int _currentWaypointIndex;
    [SerializeField] private int _currentHidingWaypointIndex;

    private float _rotationSpeed = 400f;

    private Quaternion _lookRotation;

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _maxHealth;

        _agent = GetComponent<NavMeshAgent>();
        _waypoints = FindObjectOfType<Waypoints>();
        _anim = GetComponent<Animator>();

        if (_waypoints == null)
        {
            Debug.LogError("Failed To Find Waypoints Script");
        }

        _currentWaypointIndex = 0;
        _currentHidingWaypointIndex = 0;

        _currentState = AIState.Running;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _currentState = AIState.Death;
        }

        switch (_currentState)
        {
            case AIState.Running:
                _anim.SetFloat("Speed", 3.5f);
                Movement();
                break;

            case AIState.Hiding:
                if (_isHiding == false)
                {
                    _isHiding = true;
                    _agent.isStopped = true;
                    StartCoroutine(AIHide());
                }
                break;

            case AIState.Death:
                if (!_isDying) 
                {
                    _isDying = true; 
                    _agent.isStopped = true;
                    StartCoroutine(AIDeath());
                }
                break;
        }
    }

    void Movement()
    {
        // Start moving to the first waypoint if not started yet and waypoints are available.
        if (!_hasStarted && _waypoints != null && _waypoints.GetWaypointCount() > 0)
        {
            _agent.SetDestination(_waypoints.GetWaypointPosition(_currentWaypointIndex));
            _hasStarted = true;
        }

        // Check if the agent has reached the current waypoint.
        if (_agent.remainingDistance < 0.5f && !_agent.pathPending)
        {
            switch (_currentWaypointIndex)
            {
                case 1:
                    _agent.SetDestination(_waypoints.GetHidingWaypointPosition(_currentHidingWaypointIndex));

                    if (_agent.remainingDistance < 0.5f && !_agent.pathPending)
                        _currentState = AIState.Hiding;
                    break;

                case 3:
                    _agent.SetDestination(_waypoints.GetHidingWaypointPosition(_currentHidingWaypointIndex));

                    if (_agent.remainingDistance < 0.5f && !_agent.pathPending)
                        _currentState = AIState.Hiding;
                    break;
                case 5:
                    _agent.SetDestination(_waypoints.GetHidingWaypointPosition(_currentHidingWaypointIndex));

                    if (_agent.remainingDistance < 0.5f && !_agent.pathPending)
                        _currentState = AIState.Hiding;
                    break;
                case 8:
                    _agent.SetDestination(_waypoints.GetHidingWaypointPosition(_currentHidingWaypointIndex));

                    if (_agent.remainingDistance < 0.5f && !_agent.pathPending)
                        _currentState = AIState.Hiding;
                    break;
                case 10:
                    _agent.SetDestination(_waypoints.GetHidingWaypointPosition(_currentHidingWaypointIndex));

                    if (_agent.remainingDistance < 0.5f && !_agent.pathPending)
                        _currentState = AIState.Hiding;
                    break;
                case 12:
                    _agent.SetDestination(_waypoints.GetHidingWaypointPosition(_currentHidingWaypointIndex));

                    if (_agent.remainingDistance < 0.5f && !_agent.pathPending)
                        _currentState = AIState.Hiding;
                    break;
                case 17:
                    _agent.SetDestination(_waypoints.GetHidingWaypointPosition(_currentHidingWaypointIndex));

                    if (_agent.remainingDistance < 0.5f && !_agent.pathPending)
                        _currentState = AIState.Hiding;
                    break;
                case 19:
                    _agent.SetDestination(_waypoints.GetHidingWaypointPosition(_currentHidingWaypointIndex));

                    if (_agent.remainingDistance < 0.5f && !_agent.pathPending)
                        _currentState = AIState.Hiding;
                    break;
                case 21:
                    _agent.SetDestination(_waypoints.GetHidingWaypointPosition(_currentHidingWaypointIndex));

                    if (_agent.remainingDistance < 0.5f && !_agent.pathPending)
                        _currentState = AIState.Hiding;
                    break;

                default:
                    MoveForward();
                    break;
            }
        }

        if (_currentWaypointIndex == _waypoints.GetWaypointCount())
        {
            this.gameObject.SetActive(false);
            ResetAI();
            SpawnManager.Instance.OnEnemyKilled();
        }
    }

    void MoveForward()
    {
        // Move to the next waypoint by incrementing the currentWaypointIndex.
        _currentWaypointIndex++;

        // Set the destination to the next waypoint's position.
        _agent.SetDestination(_waypoints.GetWaypointPosition(_currentWaypointIndex));

    }

    public void Damage(int damageAmount)
    {
        if (_currentHealth > 0)
        {
            _currentHealth -= damageAmount;
        }
    }

    private void ResetAI()
    {
        _anim.ResetTrigger("Death");
        _hasStarted = false;
        _currentState = AIState.Running;
        _currentWaypointIndex = 0;
        _currentHidingWaypointIndex = 0;
        _currentHealth = _maxHealth;
    }

    IEnumerator AIDeath()
    {
        _anim.SetTrigger("Death");
        _currentHealth = 0;
        yield return new WaitForSeconds(4f);
        this.gameObject.SetActive(false);
        ResetAI();
        SpawnManager.Instance.OnEnemyKilled();
        GameManager.Instance.UpdateScore(50);
        _isDying = false;
    }

    private IEnumerator AIHide()
    {
        _anim.SetBool("Hiding", true);

        //Rotate AI to -180 On The Y Axis
        _lookRotation = Quaternion.Euler(0, -180, 0);
        while (Quaternion.Angle(transform.rotation, _lookRotation) > 0.5f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, _rotationSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        _currentHidingWaypointIndex++;
        _isHiding = false;
        _agent.isStopped = false;
        _anim.SetBool("Hiding", false);
        MoveForward();
        _currentState = AIState.Running;
    }
}
