using UnityEngine;

public class DiamondScript : MonoBehaviour
{
	private Animator anim;

	private int takeHash = Animator.StringToHash("Disappear");

	public bool wasTaken;

	private void Start()
	{
		anim = GetComponent<Animator>();
		wasTaken = false;
	}

	public void HideThis()
	{
		base.gameObject.SetActive(value: false);
	}

	public void TakeThis()
	{
		if (!wasTaken)
		{
			base.gameObject.GetComponent<AudioSource>().Play();
			anim.SetTrigger(takeHash);
			wasTaken = true;
		}
	}

	public bool WasTaken()
	{
		return wasTaken;
	}
}
