using UnityEngine;

public class LeverController : MonoBehaviour
{
    private bool leverSwitched = false;
    public GameObject door;
    public float yOffset;
    private Vector2 velocity = Vector2.zero;
    private SpriteRenderer _spriteRenderer;
    private float _startY;
    private float _startX;

    // Use this for initialization
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _startX = door.transform.position.x;
        _startY = door.transform.position.y;
    }

    public void Activate()
    {
        if (!leverSwitched)
        {
            leverSwitched = true;
            _spriteRenderer.flipX = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float yPosition = leverSwitched ? _startY + yOffset : _startY;
        door.transform.position = Vector2.SmoothDamp(door.transform.position, new Vector2(_startX, yPosition), ref velocity, 0.3f);
    }
}