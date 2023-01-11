using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLine : MonoBehaviour
{
	public Vector3 target;
	public float speed = 10;
	private bool move = true;
	public float hurtLife = 30f;
	CapsuleCollider cap;

	private void Start()
	{
		cap = GetComponent<CapsuleCollider>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Unit") return;
		if (other.tag == "weapon") return;
		move = false;

		cap.enabled = false;
		transform.parent = other.gameObject.transform;
		if (other.tag == "Enemy")
		{
			other.GetComponent<Life>().Hurt(hurtLife);
			this.gameObject.SetActive(false);
		}
	}

	void Clear()
    {
		Destroy(this.gameObject);
    }

	public void shoot_(Vector3 pos)
    {
		target = pos;
		this.transform.LookAt(target);
		StartCoroutine(Shoot());
		Invoke("Clear", 5f);
	}
	IEnumerator Shoot()
    {
		while(move)
        {
			transform.Translate(Vector3.forward * Time.deltaTime * speed);
			yield return null;
        }
    }

}
