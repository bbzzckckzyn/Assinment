using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCamera : MonoBehaviour
{
    public float speed_fly = 15f;
    public float speed_rateta = 50f;
    float x;
    float y;
    private void Start()
    {
        y = -this.transform.rotation.eulerAngles.x;
        x = this.transform.rotation.eulerAngles.y;
    }
    float speed_;
    void move()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed_ = speed_fly * 3f;
        }
        else
        {
            speed_ = speed_fly;
        }
        transform.Translate(
            Input.GetAxis("Horizontal") * Time.deltaTime * speed_,
            0f,
            Input.GetAxis("Vertical") * Time.deltaTime * speed_);

        
        if(Input.GetMouseButton(1))
        {
            x += Input.GetAxis("Mouse X");
            y += Input.GetAxis("Mouse Y");

            transform.rotation = Quaternion.Euler(-y, x, 0);
        }
    }

    private void Update()
    {
        move();
    }
}
