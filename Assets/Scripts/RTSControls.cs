using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSControls : MonoBehaviour
{
    [SerializeField] Transform selectionAreaTransform;

    Vector3 startPosition;
    List<Zombie> selectedZombies;

    // Start is called before the first frame update
    void Start()
    {
        selectedZombies = new List<Zombie>();
        selectionAreaTransform.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            selectionAreaTransform.gameObject.SetActive(true);
            startPosition = GetMouseWorldPosition();
            startPosition.z = 0f;
        }

        if (Input.GetMouseButton(0)) {
            Vector3 currentMousePosition = GetMouseWorldPosition();
            Vector3 lowerLeft = new Vector3(Mathf.Min(startPosition.x, currentMousePosition.x), Mathf.Min(startPosition.y, currentMousePosition.y));
            Vector3 upperRight = new Vector3(Mathf.Max(startPosition.x, currentMousePosition.x), Mathf.Max(startPosition.y, currentMousePosition.y));
            selectionAreaTransform.position = lowerLeft;
            selectionAreaTransform.localScale = upperRight - lowerLeft;
        }

        if (Input.GetMouseButtonUp(0)) {
            selectionAreaTransform.gameObject.SetActive(false);


            Collider2D[] collider2Ds = Physics2D.OverlapAreaAll(startPosition, GetMouseWorldPosition());

            foreach (Zombie zombie in selectedZombies) {
                zombie.EnableSelectedIndicator(false);
            }
            selectedZombies.Clear();

            foreach (Collider2D collider2D in collider2Ds) {
                Zombie zombie = collider2D.GetComponent<Zombie>();

                if (zombie != null) {
                    zombie.EnableSelectedIndicator(true);
                    selectedZombies.Add(zombie);
                }
            }

            Debug.Log(selectedZombies.Count);
        }
    }

    Vector3 GetMouseWorldPosition() {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        return mouseWorldPosition;
    }
}
