using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LayerMask : MonoBehaviour
{
    private Health _health;

    [SerializeField] private bool _friendlyFire = false;

    private bool _canAttack;    

    [SerializeField] private TextMeshProUGUI _friendlyFireValueText;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {
            FriendlyFire();
        }

        if (Input.GetMouseButtonDown(0) && _canAttack || Input.GetMouseButtonDown(0) && _friendlyFire)
        {
            if (_health != null) 
            {
                //Debug.Log("LayerMask: CalledDamage");
                _health.Damage(10f);
            }
        }
    }

    void FixedUpdate()
    {
        //Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 1 << 8))
        {
            Debug.DrawRay(ray.origin, Camera.main.transform.forward * 1000, Color.green);

            Debug.Log($"Hit: &{hitInfo.collider.name}");          

            if (hitInfo.collider.TryGetComponent<Health>(out Health health))
            {
                //Debug.Log("TryToGetComponent Ran");
                _health = health;
            }

            //_meshRenderer = hitInfo.collider.GetComponent<MeshRenderer>();

            _canAttack = true;

        }
        else if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 1 << 8 | 1 << 9) && _friendlyFire)
        {
            Debug.DrawRay(ray.origin, Camera.main.transform.forward * 1000, Color.green);

            Debug.Log($"Hit: & {hitInfo.collider.name}");

            if (hitInfo.collider.TryGetComponent<Health>(out Health health))
            {
                //Debug.Log("TryToGetComponent Ran");
                _health = health;
            }

            //_meshRenderer = hitInfo.collider.GetComponent<MeshRenderer>();

            _canAttack = true;
        }
        else
        {
            Debug.DrawRay(ray.origin, Camera.main.transform.forward * 1000, Color.red);
            _canAttack = false;
        }
    }


    public void FriendlyFire()
    {
        _friendlyFire = !_friendlyFire;

        if (_friendlyFire)
        {
            _friendlyFireValueText.color = new Color32(27, 255, 0, 255);
            _friendlyFireValueText.text = "ON";
        }
        else
        {
            _friendlyFireValueText.color = new Color32(255, 0, 0, 255);
            _friendlyFireValueText.text = "OFF";
        }
    }
}
