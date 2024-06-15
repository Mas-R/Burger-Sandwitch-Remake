using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField] private LayerMask detectionLayer;
    [SerializeField] private float detectionDistance = 1f;
    [SerializeField] float speed=2f;
    [SerializeField] Animator animator;
    public LevelManager levelManager;
    public List<GameObject> stackObject;
    public Bread bread;
    public enum Bread { Not, Top, Bottom }
    private void Awake()
    {
        stackObject.Add(gameObject);
        animator = GetComponent<Animator>();
    }
    public float DetectionDistance // Property to access detectionDistance
    {
        get { return detectionDistance; }
        set { detectionDistance = value; }
    }
    void Update()
    {
        Debug.DrawRay(transform.position, Vector2.up * detectionDistance, Color.red);
        Debug.DrawRay(transform.position, Vector2.down * detectionDistance, Color.red);
        Debug.DrawRay(transform.position, Vector2.left * detectionDistance, Color.red);
        Debug.DrawRay(transform.position, Vector2.right * detectionDistance, Color.red);
    }
    public IEnumerator CheckDirection(Vector2 direction)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, detectionDistance, detectionLayer.value);
        if (hits.Length > 1 && bread != Bread.Bottom)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject != gameObject && hit.collider.gameObject.GetComponent<ObjectDetector>().bread != Bread.Top)
                {
                    GameObject hitObject = hit.collider.gameObject;
                    int heightObject = hitObject.GetComponent<ObjectDetector>().stackObject.Count;
                    foreach (GameObject go in stackObject)
                    {
                        go.GetComponent<SpriteRenderer>().sortingOrder += heightObject * 1;
                        //go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, go.transform.position.z+ heightObject *-1f);
                        hitObject.GetComponent<ObjectDetector>().stackObject.Add(go);
                    }
                    yield return StartCoroutine(MoveSequence(direction, heightObject));
                    if (hitObject.GetComponent<ObjectDetector>().stackObject.Count == levelManager.maxObject)
                    {
                        levelManager.CheckObject(hitObject.GetComponent<ObjectDetector>().stackObject);
                    }
                    transform.parent = hitObject.transform;
                    levelManager.draggingStarted = false;
                    GetComponent<Collider2D>().enabled = false;
                    GetComponent<ObjectDetector>().enabled = false;

                    yield break;
                }
            }
        }
        
            Debug.Log("Detected object: none");
            StartCoroutine(CantMove());
        
    }
    IEnumerator CantMove()
    {
        animator.SetTrigger("Wrong");
        yield return new WaitForSeconds(0.25f);
        levelManager.draggingStarted = false;
    }
    IEnumerator MoveSequence(Vector2 directionv2, int heightObject)
    {
        Vector3 direction = new Vector3(directionv2.x, directionv2.y, 0);
        yield return StartCoroutine(MoveToPosition(transform.position + Vector3.up * 0.2f * heightObject));
        yield return StartCoroutine(MoveToPosition(transform.position+direction));

    }

    

    IEnumerator MoveToPosition(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = target; // Ensure the object is exactly at the target position
    }
}
