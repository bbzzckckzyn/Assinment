using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class arrow : MonoBehaviour
{
	public Vector3 target;
	public float speed = 10;
	private float distanceToTarget;
	private bool move = true;

	CapsuleCollider cap;

    private void Start()
    {
		///
		///need !!!!!!!!!!!!!!!!!
		///
		//if(EventSystem)
        cap = GetComponent<CapsuleCollider>();
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Unit") return;
		if (other.tag == "weapon") return;
		move = false;
		
		cap.enabled = false;
		transform.parent = other.gameObject.transform;
		if(other.tag == "enemy")
        {
			other.gameObject.GetComponent<base_>().set_hp(10f);
        }
	}

	public void shoot_(Vector3 pos)
    {
		distanceToTarget = Vector3.Distance(this.transform.position, pos);
		Vector3 offset = new Vector3(distanceToTarget / 15f * Random.Range(-1f, 1f),
			0,distanceToTarget / 15f * Random.Range(-1f, 1f));
		target = pos + offset;

		//distanceToTarget = Vector3.Distance(this.transform.position, target);
		StartCoroutine(Shoot());
	}

	IEnumerator Shoot()
	{

		while (move)
		{
	
			Vector3 targetPos = target;
			//朝向目标  (Z轴朝向目标)
			this.transform.LookAt(targetPos);
			//根据距离衰减 角度
			float angle = Mathf.Min(1, Vector3.Distance(this.transform.position, targetPos) / distanceToTarget) * 45;
			//旋转对应的角度（线性插值一定角度，然后每帧绕X轴旋转）
			this.transform.rotation = this.transform.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);
			//当前距离目标点
			float currentDist = Vector3.Distance(this.transform.position, target);

			//if(transform.eulerAngles.x < 90 && transform.eulerAngles.x > 0)
			{
				speed += Time.deltaTime * 30f;
				//Debug.Log(speed + "  " + transform.eulerAngles.x);
            }
			//平移 （朝向Z轴移动）
			this.transform.Translate(Vector3.forward * Mathf.Min(speed * Time.deltaTime, currentDist));
			//this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
			yield return null;
		}
	}

    #region old_code
    /*
	public Vector3 target;
	public void shoot(Vector3 t)
    {
		target = t;
		Fire(transform.position,target);
    }

    // Update is called once per fram
    void Update()
	{
		if (_isFiring)
		{
			UpdateArrow();
		}
	}

	// 所有變量
	public float rotateX = 45;      // 箭的最大X軸旋轉角度
	public float speed = 150;        // 箭的速度
	public float height = 10;       // 箭的最大高度 
	private Vector3 _startPos, _stopPos, _curPos;   // 起始位置，目標位置，當前位置
	private float _angleToStop;     // 從起始點到目標點的角度
	private float _startHeight, _stopHeight;    // 起始高度，結束高度
	private bool _isFiring = false;     //判斷箭是否正在移動
	private float _totalDistance, _curDistance; // 總距離， 當前距離
	private Vector3 _curRotation; // 當前的旋轉角度

	// 發射函數，你只要調用這一個函數就能發射箭了
	public void Fire(Vector3 start, Vector3 stop)
	{
		_startPos = start;
		_stopPos = stop;
		_angleToStop = GetAngleToStop(start, stop); // 計算 起始位置 到 目標位置的角度
		_startHeight = start.y;
		_stopHeight = stop.y;
		_curDistance = 0;

		// 計算總距離
		Vector3 v = _stopPos - _startPos;
		_totalDistance = Mathf.Sqrt(v.x * v.x + v.z * v.z);

		// 設置當前位置
		transform.position = start;
		_curPos = start;

		// 設置當前X，Y軸的旋轉角度
		Vector3 rotation = transform.eulerAngles;
		if (rotateX > 0)
		{
			rotation.x = -rotateX;
		}
		rotation.y = _angleToStop;

		transform.eulerAngles = rotation;
		_curRotation = rotation;

		// 設置判斷爲發射狀態，讓Update函數能夠更新
		_isFiring = true;
	}

	// 計算 起始位置 到 目標位置的角度
	private float GetAngleToStop(Vector3 startPos, Vector3 stopPos)
	{
		stopPos.x -= startPos.x;
		stopPos.z -= startPos.z;

		float deltaAngle = 0;
		if (stopPos.x == 0 && stopPos.z == 0)
		{
			return 0;
		}
		else if (stopPos.x > 0 && stopPos.z > 0)
		{
			deltaAngle = 0;
		}
		else if (stopPos.x > 0 && stopPos.z == 0)
		{
			return 90;
		}
		else if (stopPos.x > 0 && stopPos.z < 0)
		{
			deltaAngle = 180;
		}
		else if (stopPos.x == 0 && stopPos.z < 0)
		{
			return 180;
		}
		else if (stopPos.x < 0 && stopPos.z < 0)
		{
			deltaAngle = -180;
		}
		else if (stopPos.x < 0 && stopPos.z == 0)
		{
			return -90;
		}
		else if (stopPos.x < 0 && stopPos.z > 0)
		{
			deltaAngle = 0;
		}

		float angle = Mathf.Atan(stopPos.x / stopPos.z) * Mathf.Rad2Deg + deltaAngle;
		return angle;
	}

	// 更新箭到下一個位置
	private void SetNextStep()
	{
		// 計算X,Z軸移動向量，然後再把它們乘移動距離，這樣就能移動到下一個位置
		float deltaX = Mathf.Sin(_angleToStop * Mathf.Deg2Rad);
		float deltaZ = Mathf.Cos(_angleToStop * Mathf.Deg2Rad);
		float l = speed * Time.deltaTime;
		_curPos.x += deltaX * l;
		_curPos.z += deltaZ * l;

		// 增加當前距離，用來判斷是否到達終點了
		_curDistance += l;

		/************************************************/
    // 計算出當前的高度
    // 這個是一元二次方程(ax^2 + bx)，大家都知道它是一條拋物線的方程，也是弓箭軌道最重要的地方。
    // 我會在下面跟大家詳解如果運用簡單的一元二次方程來做弓箭的拋物線效果
    /************************************************/
    /*
	float a = -1;
	float b = _totalDistance;
	float apex = _totalDistance / 2;
	float deltaHeight = 1 / ((-apex) * (apex - _totalDistance) / height);
	float deltaDistance = _curDistance / _totalDistance;
	float h = deltaDistance * (_stopHeight - _startHeight) + _startHeight;
	h += deltaHeight * (a * (_curDistance * _curDistance) + b * _curDistance);
	_curPos.y = h;

	// 更新當前箭的位置
	transform.position = _curPos;

	// 旋轉X軸
	if (rotateX > 0)
	{
		_curRotation.x = -rotateX * (1 + -2 * deltaDistance);
		transform.eulerAngles = _curRotation;
	}
}

// 判斷是否到達
private bool IsArrived()
{
	return _curDistance >= _totalDistance;
}

private void UpdateArrow()
{
	SetNextStep();

	// 如果到達了目標地點就取消發射狀態
	//if (IsArrived())
	//{
	//	_isFiring = false;
	//}
}
private void OnTriggerEnter(Collider other)
{
	if (other.tag == "Unit") return;
	_isFiring = false;
}*/
    #endregion

}
