using UnityEngine;
using Sirenix.OdinInspector;

public class Test : MonoBehaviour {
 
 
	public float speed = 2f; // 控制动画速度
	public float amplitude = 3f; // 控制振幅

	void Update()
	{
		// 使用 Mathf.Cos 计算物体的垂直移动
		float y = amplitude * Mathf.Cos(Time.time * speed);
		transform.position = new Vector3(transform.position.x, y, transform.position.z);
	}
}