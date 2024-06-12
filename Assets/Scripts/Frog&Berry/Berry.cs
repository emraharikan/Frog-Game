using UnityEngine;
using DG.Tweening;


public class Berry : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mouth"))
        {
            if (GameManager.instance.isInGame)
            {
                GameObject frogObj = other.transform.parent.gameObject;
                Frog frogScript = frogObj.GetComponent<Frog>();
                if (frogScript != null)
                {
                    transform.DOScale(0f, 0.2f).OnComplete(() =>
                    {
                        frogScript.collectedItems.Remove(gameObject);
                        Destroy(gameObject, 1);
                    });
                }
            }
            
        }
    }
}
