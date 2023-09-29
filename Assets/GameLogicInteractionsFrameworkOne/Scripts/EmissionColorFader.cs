using System.Collections;
using UnityEngine;

public class EmissionColorFader : MonoBehaviour
{
    //This Script is used to fade the color of the barriers from blue to yellow and back to blue.
    [SerializeField] private Material _material;
    private Color _startColor = Color.blue;
    private Color _endColor = Color.yellow;
    private float _fadeDuration = 1.0f;

    private void Start()
    {
        StartCoroutine(FadeEmission());
    }

    private IEnumerator FadeEmission()
    {
        while (true)
        {
            float timeElapsed = 0f;
            while (timeElapsed < _fadeDuration)
            {
                float lerpFactor = timeElapsed / _fadeDuration;
                Color lerpedColor = Color.Lerp(_startColor, _endColor, lerpFactor);
                _material.SetColor("_EmissionColor", lerpedColor);

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            // Swap start and end colors for the next fade
            Color temp = _startColor;
            _startColor = _endColor;
            _endColor = temp;

            yield return new WaitForSeconds(_fadeDuration);
        }
    }
}
