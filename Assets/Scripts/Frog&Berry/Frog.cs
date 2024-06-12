using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using DG.Tweening;

public class Frog : MonoBehaviour
{
   
    public GameObject tongue;
    public SplineComputer splineComputer;
    private bool isMovingForward = true;
    private SplineFollower splineFollower; 
    private LineRenderer lineRenderer;
    private int forwardPositionCount = 0; // İleri hareket sırasında eklenen köşe sayısı
    public List<GameObject> collectedItems = new List<GameObject>();
    public List<GameObject> parents = new List<GameObject>();
    public bool isSucceded;

    void Start()
    {
        splineFollower = tongue.GetComponent<SplineFollower>();
        lineRenderer = tongue.GetComponent<LineRenderer>();
        splineFollower.spline = splineComputer; 
        splineFollower.follow = false;
        lineRenderer.positionCount = 1; // İlk köşe noktası
        lineRenderer.SetPosition(0, tongue.transform.position);

    }

    void Update()
    {
        

        if (Input.GetMouseButtonDown(1)) 
        {
            GameManager.instance.TriggerLevelStart();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform == transform)
                    {
                        Debug.Log("Raycast hit: " + hit.transform.name);
                        Frog frog = hit.transform.GetComponent<Frog>();
                        if (frog != null)
                        {
                            tongue.GetComponent<BoxCollider>().enabled = true;
                            isMovingForward = true;
                            splineFollower.follow = true;
                            forwardPositionCount = 0; // İleri hareket sırasında eklenen köşe sayısını sıfırla
                            lineRenderer.positionCount = 1; // Line Renderer'ı sıfırla
                            lineRenderer.SetPosition(0, tongue.transform.position); // İlk köşe noktası
                            isSucceded = false;
                            GameManager.instance.DecreaseMovement(1);
                           
                        }

                    }
                }
        }

        if (splineFollower.follow)
        {
            AddLineRendererPosition();

            if (splineFollower.result.percent >= 1f && isMovingForward)
            {
                isMovingForward = false;
                splineFollower.direction = Spline.Direction.Backward;
                AudioManager.instance.CollectedSoundPlay();
                isSucceded = true; 
            }
            else if (splineFollower.result.percent <= 0f && !isMovingForward)
            {
                splineFollower.follow = false;
                AddLineRendererPosition();
                ResetTongue();
            }

            if (isSucceded)
            {
                UpdateBerriesPositions();
            }
           
        }

        if (!isMovingForward && collectedItems.Count > 0)
        {
            StartCoroutine(ShrinkAndDisableParents());
        }

        if (collectedItems.Count < 1 && !isMovingForward && isSucceded)
        {
            ShrinkAndDisableParent();
            GameManager.instance.TriggerHaptic();
        }

   
    }

    void AddLineRendererPosition() 
    {
        if (isMovingForward)
        {
            float minDistance = 0.1f; 
            if (lineRenderer.positionCount == 1 || Vector3.Distance(lineRenderer.GetPosition(lineRenderer.positionCount - 1), tongue.transform.position) >= minDistance)
            {
                int positionCount = lineRenderer.positionCount;
                lineRenderer.positionCount = positionCount + 1;
                lineRenderer.SetPosition(positionCount, tongue.transform.position);
                forwardPositionCount = positionCount + 1; 
            }
        }
        else
        {
            
            if (lineRenderer.positionCount > 1)
            {
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, tongue.transform.position);
                if (Vector3.Distance(lineRenderer.GetPosition(lineRenderer.positionCount - 2), tongue.transform.position) < 0.1f)
                {
                    lineRenderer.positionCount--;
                }
            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Berry"))
        {
            CellManager otherCellManager = other.gameObject.GetComponentInParent<CellManager>();
            CellManager thisCellManager = gameObject.GetComponentInParent<CellManager>();

            if (otherCellManager != null && thisCellManager != null && otherCellManager.cellColor == thisCellManager.cellColor)
            {
                collectedItems.Add(other.gameObject); 
                parents.Add(other.transform.parent.gameObject); 
                AudioManager.instance.RightSoundPlay();
                GameManager.instance.TriggerHaptic();
                other.gameObject.transform.DOScale(1.3f, 0.2f).SetLoops(2, LoopType.Yoyo);
            }

            if (otherCellManager != null && thisCellManager != null && otherCellManager.cellColor != thisCellManager.cellColor)
            {
                tongue.GetComponent<BoxCollider>().enabled = false;
                collectedItems.Clear();
                parents.Clear();
                isMovingForward = false;
                splineFollower.direction = Spline.Direction.Backward;
                AudioManager.instance.WrongSoundPlay();
                GameManager.instance.TriggerHaptic();
            }

        }

        if (other.CompareTag("Frog"))
        {
            tongue.GetComponent<BoxCollider>().enabled = false;
            collectedItems.Clear();
            parents.Clear();
            isMovingForward = false;
            splineFollower.direction = Spline.Direction.Backward;
            AudioManager.instance.WrongSoundPlay();
            GameManager.instance.TriggerHaptic();
        }

        if (other.gameObject.CompareTag("Arrow"))
        {
            parents.Add(other.transform.parent.gameObject);
        }
    }

    void UpdateBerriesPositions() // Berry objelerini Line Renderer ile hareket ettir
    {
        if (!isMovingForward)
        {
            float step = 1f / (collectedItems.Count + 1);
            float totalDistance = Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(lineRenderer.positionCount - 1));

            for (int i = 0; i < collectedItems.Count; i++)
            {
                if (collectedItems[i] != null && collectedItems[i].CompareTag("Berry"))
                {
                    collectedItems[i].transform.SetParent(null);
                    float speedFactor = 1.0f; 
                    int index = Mathf.Clamp((int)((lineRenderer.positionCount - 1) * (i + 1) * step * speedFactor), 0, lineRenderer.positionCount - 1);
                    float distanceToMove = Vector3.Distance(collectedItems[i].transform.position, lineRenderer.GetPosition(index));
                    float duration = distanceToMove / speedFactor;

                    collectedItems[i].transform.DOMove(lineRenderer.GetPosition(index), duration);
                }
                else
                {
                    return;
                }
            }
        }

    }

    void ShrinkAndDisableParent() // Frog nesnesini küçült ve yok et
    {
        Transform parentTransform = transform.parent;
        if (parentTransform != null)
        {
            GameObject newCell = gameObject.GetComponentInParent<CellManager>().directlyBelowCell;
            parentTransform.DOScale(0f, 0.2f).OnComplete(() =>
            {
                parentTransform.gameObject.SetActive(false);
                newCell.GetComponent<CellManager>().selectedChild.SetActive(true);
                newCell.GetComponent<CellManager>().selectedChild.transform.DOScale(0.8f, 0.1f).SetEase(Ease.InBounce);
               // Destroy(transform.parent.gameObject,.5f);
            });
        }
    }


    IEnumerator ShrinkAndDisableParents() // Beey ve Arrow objelerinin Cell objelerini küçült ve yok et
    {
        int count = parents.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            if (i < parents.Count)
            {
                GameObject parent = parents[i];
                if (parent != null)
                {
                    GameObject newCell = parent.GetComponent<CellManager>().directlyBelowCell;
                    parent.transform.DOScale(0f, 0.5f).OnComplete(() =>
                    {
                        newCell.GetComponent<CellManager>().selectedChild.SetActive(true);
                        newCell.GetComponent<CellManager>().selectedChild.transform.DOScale(0.8f, 0.1f).SetEase(Ease.InBounce);
                        parent.SetActive(false);
                    });
                    yield return new WaitForSeconds(0.3f);
                }
            }
        }

        parents.Clear();
        isSucceded = true;
    }

    void ResetTongue()
    {
        tongue.GetComponent<BoxCollider>().enabled = false;
        parents.Clear();
        isMovingForward = false;
        splineFollower.follow = false;
        splineFollower.direction = Spline.Direction.Forward;
        splineFollower.SetPercent(0);
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, tongue.transform.position);
    }

}