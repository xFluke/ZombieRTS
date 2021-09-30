using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSControls : MonoBehaviour
{
    [SerializeField] Transform selectionAreaTransform;

    Vector3 startPosition;
    public List<Zombie> selectedZombies;

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

        if (Input.GetMouseButtonDown(1)) {
            Vector3 targetPosition = GetMouseWorldPosition();

            List<Vector3> targetPositionList = GetPositionListAround(targetPosition, new float[] { 3f, 6f, 9f }, new int[] { 5, 10, 20 });
            //List<Vector3> targetPositionList = GetPositionListAround(targetPosition, 3f, 5);

            int targetPositionListIndex = 0;

            foreach (Zombie zombie in selectedZombies) {
                zombie.MoveTo(targetPositionList[targetPositionListIndex]);
                targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
            }
        }
    }

    Vector3 GetMouseWorldPosition() {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        return mouseWorldPosition;
    }

    List<Vector3> GetPositionListAround(Vector3 startPosition, float[] ringDistanceArray, int[] ringPositionCountArray) {
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPosition);

        for (int i = 0; i < ringDistanceArray.Length; i++) {
            positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[i], ringPositionCountArray[i]));
        }
        return positionList;
    }

    List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount) {
        List<Vector3> positionList = new List<Vector3>();

        for (int i = 0; i < positionCount; i++) {
            float angle = i * (360f / positionCount);
            Vector3 dir = ApplyRotationToVector(new Vector3(1, 0), angle);
            Vector3 position = startPosition + dir * distance;
            positionList.Add(position);
        }
        return positionList;
    }

    Vector3 ApplyRotationToVector(Vector3 vec, float angle) {
        return Quaternion.Euler(0, 0, angle) * vec;
    }
}
