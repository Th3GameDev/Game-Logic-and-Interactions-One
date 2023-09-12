using System.Collections;
using UnityEngine;

public class EmissionColorFader : MonoBehaviour
{
    public Material material;
    public Color startColor = Color.blue;
    public Color endColor = Color.yellow;
    public float fadeDuration = 1.0f;

    private bool isFading = false;

    private void Start()
    {
        
        StartCoroutine(FadeEmission());
    }

    private IEnumerator FadeEmission()
    {
        while (true)
        {
            isFading = true;

            float timeElapsed = 0f;
            while (timeElapsed < fadeDuration)
            {
                float lerpFactor = timeElapsed / fadeDuration;
                Color lerpedColor = Color.Lerp(startColor, endColor, lerpFactor);
                material.SetColor("_EmissionColor", lerpedColor);

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            // Swap start and end colors for the next fade
            Color temp = startColor;
            startColor = endColor;
            endColor = temp;

            isFading = false;

            yield return new WaitForSeconds(fadeDuration);
        }
    }
}
