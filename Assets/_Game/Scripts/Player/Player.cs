using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int takenDiamonds;

    public float speed;

    private int direction = 1;

    private Vector3[] arrows = new Vector3[4]
    {
        Vector3.left,
        Vector3.forward,
        Vector3.right,
        Vector3.back
    };

    private int[] speedDeltas = new int[4];

    private int arrowsLength;

    private int speedDeltasLength = 4;

    private bool isColliding;

    void Start()
    {
        AudioListener.pause = false;
        arrowsLength = arrows.Length;
    }

    // Update is called once per frame
    void Update()
    {
        base.transform.Translate(arrows[direction % arrowsLength] * (speed + (float)speedDeltas[direction % speedDeltasLength]) * Time.deltaTime);

        if (Input.GetKeyDown("space"))
        {
            direction++;
            turnPlayer();
        }
        isColliding = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isColliding)
        {
            return;
        }
        isColliding = true;
        if (other.gameObject.CompareTag("Coin"))
        {
            Debug.Log("1");
            base.gameObject.GetComponent<ParticleSystem>().Play();
            if (!other.gameObject.GetComponent<DiamondScript>().WasTaken())
            {
                other.gameObject.GetComponent<DiamondScript>().TakeThis();
                takenDiamonds++;
            }
            //updateCanvas = true;
        }
        else if (other.gameObject.CompareTag("Terminator"))
        {
            //endGame();
        }
    }

    private void turnPlayer()
    {
    }

}
