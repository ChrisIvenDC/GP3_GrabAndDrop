using UnityEngine;

public class DragDropItem : MonoBehaviour
{
    private bool isDragging = false;
    private Rigidbody currentlyDraggedRigidbody;
    private Vector3 offset;
    private int originalLayer;
    public GameObject PickupUI;
    public GameObject DropUI;
    bool Active = true;

    [Header("Smooth Movement")]
    public float smoothSpeed = 5f;

    void Update()
    {
        PlayerReach playerReach = GetComponent<PlayerReach>();

        if (playerReach != null && playerReach.IsRaycastHit()) 
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit) && Active)
            {
                PickupUI.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {

                    Rigidbody hitRigidbody = hit.collider.GetComponent<Rigidbody>();

                    if (hitRigidbody != null)
                    {

                        DropUI.SetActive(true);
                        isDragging = true;
                        currentlyDraggedRigidbody = hitRigidbody;

                        originalLayer = currentlyDraggedRigidbody.gameObject.layer;

                        int temporaryLayer = LayerMask.NameToLayer("TemporaryLayer");
                        currentlyDraggedRigidbody.gameObject.layer = temporaryLayer;

                        offset = currentlyDraggedRigidbody.transform.position - hit.point;

                        currentlyDraggedRigidbody.isKinematic = true;
                    }
                }
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            PickupUI.SetActive(false);
            Active = false;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Active = true;
        }

        if (!playerReach.IsRaycastHit())
        {
            PickupUI.SetActive(false);
        }


            if (isDragging && currentlyDraggedRigidbody != null)
        {
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.transform.position.y));

            MoveWithCollisions(targetPosition);

            if (Input.GetMouseButtonUp(0))
            {
                DropUI.SetActive(false);
                currentlyDraggedRigidbody.gameObject.layer = originalLayer;

                isDragging = false;
                currentlyDraggedRigidbody.isKinematic = false;
                currentlyDraggedRigidbody = null;
            }
        }


    }



    private void MoveWithCollisions(Vector3 targetPosition)
    {
        currentlyDraggedRigidbody.MovePosition(Vector3.Lerp(currentlyDraggedRigidbody.transform.position, targetPosition, smoothSpeed * Time.deltaTime));
    }
}
