using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    GameObject selectedIndicator;

    // Start is called before the first frame update
    void Start()
    {
        selectedIndicator = transform.Find("Selected").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableSelectedIndicator(bool enabled) {
        selectedIndicator.SetActive(enabled);
    }
}
