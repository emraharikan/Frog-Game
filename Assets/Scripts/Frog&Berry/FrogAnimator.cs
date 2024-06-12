using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogAnimator : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer; 
    public int blendShapeIndex = 0; 
    public float blinkDuration = 1.0f; 
    public float blinkInterval = 3.0f; 

    private void Start()
    {
        StartCoroutine(BlinkEyes());
    }

    private IEnumerator BlinkEyes()
    {
        while (true)
        {
            // Gözleri yavaşça kapat
            yield return StartCoroutine(ChangeBlendShapeValue(0, 100, blinkDuration / 2));

            // Gözleri yavaşça aç
            yield return StartCoroutine(ChangeBlendShapeValue(100, 0, blinkDuration / 2));

       
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private IEnumerator ChangeBlendShapeValue(float startValue, float endValue, float duration)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float blendShapeValue = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, blendShapeValue);
            yield return null;
        }

        skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, endValue); 
    }
}