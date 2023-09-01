using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform _gunBarrelPos;

    [Range(0.1f, 1f)][SerializeField] private float _fireRate = 0.3f;

    private bool _canFire;

    // Start is called before the first frame update
    void Start()
    {
        _canFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canFire)
        {
            Fire(_fireRate);
        }
    }

    void Fire(float fireRate)
    {
        _canFire = false;
        StartCoroutine(FireRateCoolDown(fireRate));

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit _hitPoint;

        if (Physics.Raycast(ray, out _hitPoint, 500f, 1 << 8))
        {           
            if (_hitPoint.collider.TryGetComponent<BasicAI>(out BasicAI ai))
            {
                ai.Damage(50);
            }
        }
    }

    IEnumerator FireRateCoolDown(float fireRate)
    {
        yield return new WaitForSeconds(fireRate);
        _canFire = true;
    }
}
