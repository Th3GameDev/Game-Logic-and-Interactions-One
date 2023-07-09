using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class Health : MonoBehaviour
{
    private float _maxHealth = 100f;
    [SerializeField] private float _currentHealth;

    [SerializeField] private Image _healthSlider;

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //_healthSlider.fillAmount = _currentHealth / _maxHealth;

        if (this._currentHealth <= 0)
        {
            this._currentHealth = 0f;
            
            Death();
        }
    }

    public void Damage(float damageAmount)
    {
        if (this._currentHealth > 0)
        {
            this._currentHealth -= damageAmount;

            UpdateHealthBar();
        }

    }

    public void Death()
    {      
        Destroy(this.gameObject, .1f);

        Destroy(this._healthSlider.transform.parent.gameObject, .1f);
    }

    public void UpdateHealthBar()
    {
        this._healthSlider.fillAmount = Mathf.Clamp(_currentHealth / _maxHealth, 0, 1f);
    }
}
