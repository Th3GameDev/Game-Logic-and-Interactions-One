using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float _moveSpeed;

    private Vector3 _targetPos;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var distance = Vector3.Distance(_targetPos, transform.position);

        if (distance > 0.5f)
        {
            //var direction = _targetPos - transform.position;

            //direction.Normalize();

            //transform.Translate(direction * _moveSpeed * Time.deltaTime);

            MoveToPosition(_targetPos);
        }
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        targetPosition.y = 0.5f;
        _targetPos = targetPosition;
    }

    void MoveToPosition(Vector3 targetPosition)
    {
        var direction = _targetPos - transform.position;

        direction.Normalize();

        transform.Translate(direction * _moveSpeed * Time.deltaTime);
    }
}
