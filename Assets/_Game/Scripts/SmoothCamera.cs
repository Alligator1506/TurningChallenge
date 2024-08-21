using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
	public float dampTime = 0.15f;

	private Vector3 velocity = Vector3.zero;

	public Transform target;

	private void Update()
	{
		if ((bool)target)
		{
			Vector3 vector = GetComponent<Camera>().WorldToViewportPoint(target.position);
			Vector3 vector2 = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, vector.z));
			Vector3 vector3 = base.transform.position + vector2;
			base.transform.position = Vector3.SmoothDamp(base.transform.position, vector3, ref velocity, dampTime);
		}
	}
}
