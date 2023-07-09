using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove : MonoBehaviour
{
    [SerializeField] private Player _player;

    private Vector3 _positionOffset = new Vector3(0, 0.5f, 0);

    private Vector3 _hitPosition;

    private bool _canMove;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();

        if (_player == null)
            Debug.LogError("Failed To Find Player Script");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canMove)
        {
           _player.SetTargetPosition(_hitPosition);
        }
    }

    void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {

            //Debug.DrawRay(ray.origin, Camera.main.transform.forward * 1000, Color.green);

            _hitPosition = hit.point;


            if (hit.collider.tag == "Ground")
            {
                _canMove = true;

                Debug.DrawRay(ray.origin, Camera.main.transform.forward * 1000, Color.green);
            }
            else
            {
                _canMove = false;

                Debug.DrawRay(ray.origin, Camera.main.transform.forward * 1000, Color.red);
            }
        }
    }
}
