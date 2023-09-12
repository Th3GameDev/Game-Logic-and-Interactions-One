using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform _gunBarrelPos;

    [Range(0.1f, 1f)][SerializeField] private float _fireRate = 0.3f;

    private int _maxAmmo = 5;
    private int _currentAmmo;

    private bool _canFire;
    private bool _isReloading;

    [SerializeField] private AudioSource[] _audioSource;
    [SerializeField] private AudioClip[] _audioClips;

    void Start()
    {
        _canFire = true;
        _currentAmmo = _maxAmmo;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canFire && _currentAmmo > 0 && _isReloading == false)
        {
            Fire(_fireRate);
        }
        else if (Input.GetMouseButtonDown(0) && _currentAmmo == 0)
        {
            Reload();
        }

        //Testing
        if (Input.GetMouseButton(1))
        {
            Camera.main.fieldOfView = 25;
        }
        else
        {
            Camera.main.fieldOfView = 55;
        }

        if (Input.GetKeyDown(KeyCode.R) && _isReloading == false)
        {
            Reload();
        }
    }

    void Fire(float fireRate)
    {
        _audioSource[0].Play();

        _currentAmmo--;

        UIManager.Instance.UpdateAmmoCount(_currentAmmo);

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
        else if (Physics.Raycast(ray, out _hitPoint, 500f, 1 << 10))
        {
            _audioSource[1].Play();
        }
    }

    private void Reload()
    {
        if (_isReloading == false)
        {
            _isReloading = true;
            StartCoroutine(WeaponReload());
        }
    }

    public int GetAmmoCount()
    {
        return _currentAmmo;
    }

    IEnumerator WeaponReload()
    {
        _audioSource[0].PlayOneShot(_audioClips[1]);
        yield return new WaitForSeconds(_audioClips[1].length);
        _currentAmmo = _maxAmmo;
        UIManager.Instance.UpdateAmmoCount(_currentAmmo);
        _isReloading = false;
    }

    IEnumerator FireRateCoolDown(float fireRate)
    {
        yield return new WaitForSeconds(fireRate);
        _canFire = true;
    }
}
