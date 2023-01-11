using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public GameObject theDoor;
    public GameObject cube;
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            theDoor.transform.rotation = Quaternion.Euler(0, 90, 0);
            cube.SetActive(true);
        }

    }
}
