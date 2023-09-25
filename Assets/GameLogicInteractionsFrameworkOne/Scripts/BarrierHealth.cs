using System.Collections;
using UnityEngine;

public class BarrierHealth : MonoBehaviour
{
    private int _maxHealth = 100;
    private int _currentHealth;
    private Renderer _renderer;
    private Material _material;
    private Color _emissionColor;
    private bool _isBlinking = false;
    private MeshCollider _barrierCol;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _barrierCol = GetComponent<MeshCollider>();
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
        _emissionColor = _material.GetColor("_EmissionColor");
    }

    public void TakeDamage(int damage)
    {       
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {           
            _renderer.enabled = false;
            _barrierCol.enabled = false;
            Invoke("ReactivateBarrier", 3.0f);
        }
        else
        {
            if (!_isBlinking)
            {
               
                _isBlinking = true;
                StartCoroutine(BlinkBarrier());
            }
        }
    }

    private IEnumerator BlinkBarrier()
    {
       
        for (int i = 0; i < 2; i++)
        {
            _renderer.material.SetColor("_EmissionColor", Color.red);
            yield return new WaitForSeconds(0.2f); 
            _renderer.material.SetColor("_EmissionColor", _emissionColor);
            yield return new WaitForSeconds(0.2f); 
        }
        _isBlinking = false;
    }

    private void ReactivateBarrier()
    {
        _currentHealth = _maxHealth;
        _barrierCol.enabled = true;
        _renderer.enabled = true;
        _renderer.material = _material;
    }
}