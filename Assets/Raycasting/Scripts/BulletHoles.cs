using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class BulletHoles : MonoBehaviour
{
    [SerializeField] private GameObject _bulletHolePrefab;

    [SerializeField] private float _fireRate = 0.5f;

    [SerializeField] private bool _canFire = true;

    [SerializeField] private bool _wallHit;

    private Vector3 _hitPos;

    private RaycastHit _hitPoint;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canFire)
        {
            _canFire = false;
            StartCoroutine(FireRate(_fireRate));

            if (_wallHit)
            {
                //Spawn Bullethole at hit point
                GameObject go = Instantiate(_bulletHolePrefab, _hitPos + (_hitPoint.normal * 0.0001f), Quaternion.LookRotation(_hitPoint.normal));
            }
        }
    }

    void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out _hitPoint, 8f))
        {
            _hitPos = _hitPoint.point;

            if (_hitPoint.collider.tag == "Wall")
            {
                _wallHit = true;

                Debug.DrawRay(ray.origin, Camera.main.transform.forward * 1000, Color.green);
            }
            else
            {

            }
        }
        else
        {
            _wallHit = false;
            Debug.DrawRay(ray.origin, Camera.main.transform.forward * 1000, Color.red);
        }
    }

    IEnumerator FireRate(float fireRate)
    {
        _canFire = false;

        yield return new WaitForSeconds(fireRate);

        _canFire = true;
    }
}
