using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToChangeColor : MonoBehaviour
{
    private bool _canChangeColor;
    [SerializeField] private GameObject _objectHit;
    private Vector3 _hitPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canChangeColor)
        {
            _objectHit.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
        }
    }

    void FixedUpdate()
    {
        CheckMouseIsOverCube();
    }

    void CheckMouseIsOverCube()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            _objectHit = hit.collider.gameObject;    

            if (_objectHit.tag == "Cube")
            {
                _canChangeColor = true;


                Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);
            }
            else
            {
                _canChangeColor = false;
                Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);
            }
        }    
    }
}
