using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler
{
    private Vector2 direction;
    private Vector2 startPos, endPos;
    private LevelManager levelManager;
    public float swipeThreshold = 100f;
    public Camera raycastCamera;
    [SerializeField] GameObject objectSelected;
    private bool isTouchActive = false;
    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
        direction = Vector2.zero;
        raycastCamera = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
       
        if (!levelManager.draggingStarted)
        {
            startPos = eventData.pressPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (!levelManager.draggingStarted)
        {
            endPos = eventData.position;

            Vector2 difference = endPos - startPos; 

            if (difference.magnitude > swipeThreshold)
            {
                if (Mathf.Abs(difference.x) > Mathf.Abs(difference.y)) 
                {
                    direction = difference.x > 0 ? Vector2.right : Vector2.left; 
                }
                else 
                {
                    direction = difference.y > 0 ? Vector2.up : Vector2.down;
                }
            }
            else
            {
                direction = Vector2.zero;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        StartCoroutine(StartCalculation());
    }
    public IEnumerator StartCalculation()
    {
        if (!isTouchActive)
        {
            yield break;
        }
        if (!levelManager.draggingStarted && direction != Vector2.zero)
        {
            levelManager.draggingStarted = true;

            if (objectSelected != null)
            {
                ObjectDetector detector = objectSelected.GetComponent<ObjectDetector>();
                yield return StartCoroutine(detector.CheckDirection(direction));
                levelManager.draggingStarted = false;

            }
        }

        //reset the variables
        objectSelected = null;

        startPos = Vector2.zero;
        endPos = Vector2.zero;
        isTouchActive = false;

        //draggingStarted = false;

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isTouchActive)
        {
            return;
        }
        isTouchActive = true;

        if (!levelManager.draggingStarted)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;
                objectSelected = clickedObject;
            }
        }
        
    }
}