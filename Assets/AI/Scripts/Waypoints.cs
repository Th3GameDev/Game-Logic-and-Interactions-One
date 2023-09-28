using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [Range(0f, 2f)]
    [SerializeField] private float _waypointSize = 0.5f;

    [SerializeField] private Transform[] _waypoints = new Transform[7];

    [SerializeField] private Transform _hidingWapointParent;

    [SerializeField] private Transform[] _hidingWaypoints = new Transform[17];

    private int _randomNum;
    private int _lastRandomNum;

    [SerializeField] private bool _drawPath;
    [SerializeField] private bool _loopPath;

    [SerializeField] private bool _drawHidingWaypoints;

    void Start()
    {
        //Fill the array with children 
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            _waypoints[i] = transform.GetChild(i);
        }

        for (int i = 0; i < _hidingWapointParent.childCount; i++)
        {
            _hidingWaypoints[i] = _hidingWapointParent.transform.GetChild(i);
        }
    }

    // Returns the position of the waypoint at the given index
    public Vector3 GetWaypointPosition(int index)
    {
        if (index >= 0 && index < _waypoints.Length)
        {
            return _waypoints[index].position;
        }

        Debug.LogWarning("Invalid waypoint index.");

        return Vector3.zero;
    }


    // Returns the total number of waypoints
    public int GetWaypointCount()
    {
        return _waypoints.Length;
    }

    //Returns a random Waypoint Possition
    public Vector3 GetRandomWaypoint()
    {
        _randomNum = Random.Range(0, _waypoints.Length);

        for (int i = 0; i < _waypoints.Length; i++)
        {
            if (_randomNum == _lastRandomNum)
            {
                while (_randomNum == _lastRandomNum)
                {
                    _randomNum = Random.Range(0, _waypoints.Length);
                }
            }

            _lastRandomNum = _randomNum;
        }

        return _waypoints[_randomNum].position;
    }

    public Vector3 GetHidingWaypointPosition(int index)
    {
        if (index >= 0 && index < _hidingWaypoints.Length)
        {
            return _hidingWaypoints[index].position;
        }

        Debug.LogWarning("Invalid Hiding Waypoint Index.");

        return Vector3.zero;
    }

    public Vector3 GetRandomHidingWaypoint()
    {
        _randomNum = Random.Range(0, _hidingWaypoints.Length);

        for (int i = 0; i < _hidingWaypoints.Length; i++)
        {
            if (_randomNum == _lastRandomNum)
            {
                while (_randomNum == _lastRandomNum)
                {
                    _randomNum = Random.Range(0, _hidingWaypoints.Length);
                }
            }

            _lastRandomNum = _randomNum;
        }

        return _hidingWaypoints[_randomNum].position;
    }

    public int GetHidingWaypointCount()
    {
        return _waypoints.Length;
    }



    //Draws the blue spheres and red lines.Used to visualize the waypoints
    private void OnDrawGizmos()
    {
        if (_drawPath)
        {
            foreach (Transform t in transform)
            {
                if (t.gameObject.name != "HidingWaypoints")
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(t.position, _waypointSize);
                }
            }

            for (int i = 0; i < _waypoints.Length - 1; i++)
            {
                Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
            }

            if (_loopPath)
                Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position);
        }

        if (_drawHidingWaypoints)
        {
            Vector3 boxSize = new Vector3(0.5f, 0.5f, 0.5f);
            Gizmos.color = Color.yellow;

            foreach (Transform t in _hidingWapointParent)
            {
                Gizmos.DrawCube(t.position, boxSize);
            }

            int[] waypointIndices = new int[] { 1, 2, 3, 4, 5, 6, 8, 9, 10, 11, 12, 13, 17, 18, 19, 20, 21, 22 };

            int[] hidingWaypointIndices = new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8 };

            for (int i = 0; i < waypointIndices.Length; i++)
            {
                int waypointIndex = waypointIndices[i];
                int hidingWaypointIndex = hidingWaypointIndices[i];

                Gizmos.DrawLine(_waypoints[waypointIndex].position, _hidingWaypoints[hidingWaypointIndex].position);
                Gizmos.DrawLine(_hidingWaypoints[hidingWaypointIndex].position, _waypoints[waypointIndex].position);
            }
        } 
    }
}
