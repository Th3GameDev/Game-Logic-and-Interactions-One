using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToInstantiate : MonoBehaviour
{

    [SerializeField] private GameObject _ObjectToSpawn;

    [SerializeField] private Vector3 _positionOffset = new Vector3(0, 0.5f, 0);

    private Vector3 _hitPosition;

    private bool _canSpawnObject;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canSpawnObject)
        {
            SpawnObject();
        }
    }

    void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            _hitPosition = hit.point;

            if (hit.collider.tag == "Ground")
            {
                _canSpawnObject = true;
            }
            else
            {
                _canSpawnObject = false;
            }
        }
    }

    void SpawnObject()
    {
        GameObject go = Instantiate(_ObjectToSpawn, _hitPosition += _positionOffset, Quaternion.identity);
    }
}
