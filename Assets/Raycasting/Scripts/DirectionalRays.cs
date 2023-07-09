using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalRays : MonoBehaviour
{
    private bool _isGrounded = false;

    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGrounded)
        {
            _rb.useGravity = false;
            _rb.isKinematic = true;
        }
        else
        {
            _rb.useGravity = true;
            _rb.isKinematic = false;
        }
    }

    void FixedUpdate()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 3f))
        {
            if (hitInfo.collider.tag == "Ground")
            {
                _isGrounded = true;
            }
            else
            {
                _isGrounded = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector3.down * 3f, Color.red);
        //Debug.DrawLine(transform.position, Vector3.down * 1f, Color.red);
    }
}
