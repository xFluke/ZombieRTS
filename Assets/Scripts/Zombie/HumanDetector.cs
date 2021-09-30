using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanDetector : MonoBehaviour
{
    public bool HumanInRange => detectedHuman != null;

    private Transform detectedHuman;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Human")) {
            detectedHuman = collision.transform;
            Debug.Log(detectedHuman.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Human")) {
            detectedHuman = null;
        }
    }

    public Transform GetNearestHumanTransform() {
        return detectedHuman.transform;
    }
}
