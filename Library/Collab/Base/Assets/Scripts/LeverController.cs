using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    private bool leverSwitched = false;
    public GameObject door;
    float smoothTime = 0.3f;
    float velocity = 0.0f;
    private bool doorClosing = false;
    private Transform trans;
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            leverSwitched = true;
            spriteRenderer.flipX = leverSwitched;
            if (door && !doorClosing)
            {
                doorClosing = true;
                trans = door.gameObject.transform;
            }
        }

        if (doorClosing)
        {
            
            door.gameObject.GetComponent<BoxCollider2D>().enabled = false;

            float newPosition = Mathf.SmoothDamp(trans.position.y, -5f, ref velocity, smoothTime);
            Debug.Log(newPosition);
            trans.position = new Vector3(trans.position.x, newPosition, trans.position.z);
        }
    }
}