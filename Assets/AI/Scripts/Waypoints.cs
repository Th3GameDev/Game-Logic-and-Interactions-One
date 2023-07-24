﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [Range(0f, 2f)]
    [SerializeField] private float _waypointSize = 0.5f;

    [SerializeField] private Transform[] _waypoints = new Transform[6];

    private int _randomNum;
    private int _lastRandomNum;

    void Start()
    {
        
        //Fill the array with children 
        for (int i = 0; i < transform.childCount; i++)
        {
            _waypoints[i] = transform.GetChild(i);
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

    //Draws the blue spheres and red lines. Used to visualize the waypoints 
    private void OnDrawGizmos()
    {
        foreach (Transform t in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(t.position, _waypointSize);
        }

        Gizmos.color = Color.red;

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }

        Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position);
    }
}