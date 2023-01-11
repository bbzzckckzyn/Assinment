using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class first : MonoBehaviour
{
    //camera作为子物体
    public Transform _camera;
    float x = 0f;
    float y = 0f;
    public float move_speed = 7.5f;
    public float rorate_speed = 60f;

    void move_()
    {
        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * move_speed, 0f, Input.GetAxis("Vertical") * Time.deltaTime * move_speed);
    }
    void rotate_()
    {
        x += Input.GetAxis("Mouse X") * 50f * Time.deltaTime;
        y -= Input.GetAxis("Mouse Y") * 50f * Time.deltaTime;
        if (y > 20f) y = 20f;
        if (y < -20f) y = -20f;
        if (x > 360f) x -= 360f;
        if (x < -360f) x += 360f;
        transform.rotation = Quaternion.Euler(y, x, 0.0f);
        transform.rotation = Quaternion.Euler(0f, x, 0.0f);
        _camera.rotation = Quaternion.Euler(y, x, 0.0f);
    }
    private void LateUpdate()
    {
        move_();
        rotate_();
    }
}
