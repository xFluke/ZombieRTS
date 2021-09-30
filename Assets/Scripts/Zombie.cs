using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Zombie : MonoBehaviour
{
    [SerializeField] GameObject selectedIndicator;

    [SerializeField] GameObject destinationIndicatorPrefab;
    GameObject destinationIndicator;

    Vector3 targetPosition;
    float moveSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnableSelectedIndicator(bool enabled) {
        selectedIndicator.SetActive(enabled);
    }

    public void MoveTo(Vector3 destination) {
        if (destinationIndicator != null)
            Destroy(destinationIndicator);

        Debug.Log("Moving to " + destination);
        targetPosition = destination;

        destinationIndicator = Instantiate(destinationIndicatorPrefab, destination, Quaternion.identity);

        if ((targetPosition.x - transform.position.x) < 0) {
            transform.eulerAngles = new Vector2(0, 180);
        }
        else {
            transform.eulerAngles = new Vector2(0, 0);
        }

        GetComponent<Pathfinding.AIDestinationSetter>().target = destinationIndicator.transform;

    }
}
