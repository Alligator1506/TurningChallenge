using UnityEngine;

public class InstructionsScript : MonoBehaviour
{
	public GameObject instructions;

	private Touch touch;

	private bool touched;

	private void Start()
	{
		instructions.SetActive(value: true);
	}

	private void Update()
	{
		for (int i = 0; i < Input.touchCount; i++)
		{
			touch = Input.GetTouch(i);
			if (touch.phase == TouchPhase.Ended && touch.tapCount >= 1)
			{
				touched = true;
			}
		}
		if (touched || Input.GetKeyDown("space"))
		{
			instructions.SetActive(value: false);
			touched = false;
		}
	}
}
