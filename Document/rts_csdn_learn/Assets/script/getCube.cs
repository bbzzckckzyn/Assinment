using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getCube : MonoBehaviour
{
    public GameObject t;
    public GameObject talkUI;
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Cube")
        {
            talkUI.SetActive(true);
            t = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Cube")
        {
            talkUI.SetActive(false);
            //t = null;
        }
    }
    bool isGetCube;
    public GameObject oldCamera;
    public GameObject newCamera;

    public List<GameObject> startGameobj;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(isGetCube)
            {
                t.transform.position += Vector3.forward * 0.5f;
                t.GetComponent<Rigidbody>().isKinematic = false;
                t.GetComponent<Rigidbody>().useGravity = true;
                t.transform.parent = null;
                Invoke("StartGame", 1f);
            }
            if(t!=null && !isGetCube)
            {
                t.transform.parent = this.transform;
                t.transform.position += Vector3.up*0.5f; 
                isGetCube = true;
                t.GetComponent<Rigidbody>().useGravity = false;
            }
            
        }
    }

    void StartGame()
    {
        talkUI.SetActive(false);
        oldCamera.SetActive(false);
        newCamera.SetActive(true);
        for(int i =0;i<startGameobj.Count;++i)
        {
            startGameobj[i].SetActive(true);
        }
        this.transform.parent.gameObject.SetActive(false);
    }
}
