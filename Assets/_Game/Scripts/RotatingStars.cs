using UnityEngine;

public class RotatingStars : MonoBehaviour
{
	public float rotation;

	private void Update()
	{
		base.transform.Rotate(0f, 0f, rotation, Space.World);
	}
}
