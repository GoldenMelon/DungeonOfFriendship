using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

public class Character : MonoBehaviour
{
    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool carried = false;
    public bool active = true;
    public bool canBeCarried = false;
    public bool carryingSomebody = false;
    public float speed = 5;
    public float jumpForce = 10;
    public Transform groundCheck;
    public Carry carry;
    public bool CanJump = true;
    public bool StraightForward = false;
    public bool Blind = false;

    private Dictionary<int, GameObject> _levers;
    private Dictionary<int, GameObject> _urns;
    private Dictionary<int, GameObject> _doors;


    private Rigidbody2D rb2d;
    private Animator anim;
    private bool grounded = false;
    private bool jump = false;
    private bool inModuleWindow = false;

    // Use this for initialization
    void Start()
    {
        _levers = new Dictionary<int, GameObject>();
        _urns = new Dictionary<int, GameObject>();
        _doors = new Dictionary<int, GameObject>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!grounded && active && !carried)
        {
            float h = Input.GetAxisRaw("Horizontal");
            rb2d.AddForce(new Vector2(-h * 1000, 0));
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (!grounded && active && !carried)
        {
            float h = Input.GetAxisRaw("Horizontal");
            rb2d.AddForce(new Vector2(-h * 1000, 0));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Lever"))
        {
            if (!_levers.ContainsKey(other.gameObject.GetInstanceID()))
            {
                _levers.Add(other.gameObject.GetInstanceID(), other.gameObject);
            }
        }

        if (other.gameObject.CompareTag("Urn"))
        {
            if (!_urns.ContainsKey(other.gameObject.GetInstanceID()))
            {
                _urns.Add(other.gameObject.GetInstanceID(), other.gameObject);
            }
        }

        if (other.gameObject.CompareTag("Door"))
        {
            Debug.Log("DVER");
            if (!_doors.ContainsKey(other.gameObject.GetInstanceID()))
            {
                Debug.Log("DVER DOBAVLENA");
                _doors.Add(other.gameObject.GetInstanceID(), other.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Lever"))
        {
            _levers.Remove(other.gameObject.GetInstanceID());
        }

        if (other.gameObject.CompareTag("Door"))
        {
            Debug.Log("DVER UDOLENA");
            _doors.Remove(other.gameObject.GetInstanceID());
        }

        if (other.gameObject.CompareTag("Urn"))
        {
            _urns.Remove(other.gameObject.GetInstanceID());
        }
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        if (active && !carried)
        {
            float h = Input.GetAxisRaw("Horizontal");

            if (StraightForward)
            {
                h = facingRight ? Mathf.Abs(h) : -Mathf.Abs(h);
            }

            if (grounded && Math.Abs(h) < 0.05f)
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }
            else
            {
                rb2d.velocity = new Vector2(h * speed, rb2d.velocity.y);
            }

            if (h > 0 && !facingRight)
            {
                Flip();
            }
            else if (h < 0 && facingRight)
            {
                Flip();
            }

            if (jump)
            {
                rb2d.AddForce(new Vector2(0f, jumpForce));
                jump = false;
            }
        }

        anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));
        anim.SetBool("Jump", !grounded);
    }

    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        if (active && !carried)
        {
            if (Input.GetButtonDown("Pickup"))
            {
                carry.PickupOrDropCargo();
                carryingSomebody = carry.carryingSomething();
            }

            if (Input.GetButtonDown("Jump") && grounded)
            {
                if (CanJump)
                {
                    jump = true;
                }
            }

            if (Input.GetButtonDown("Interact") && !inModuleWindow)
            {
                if (_levers.Count != 0)
                {
                    _levers.First().Value.GetComponent<LeverController>().Activate();
                }

                if (_urns.Count != 0)
                {
                    _urns.First().Value.GetComponent<BlindUrnController>().Interact();
                    gameObject.transform.parent.GetComponent<CharacterSwitcher>().SwitchingEnabled = false;
                    inModuleWindow = true;
                }

                if (_doors.Count != 0)
                {
                    Debug.Log("JOPA");
                    _doors.First().Value.GetComponent<DoorController>().Activate(gameObject);
                }
            }

            if (inModuleWindow)
            {
                if (Input.GetButtonDown("Yes"))
                {
                    gameObject.transform.parent.GetComponent<CharacterSwitcher>().SwitchingEnabled = true;
                    _urns.First().Value.GetComponent<BlindUrnController>().Submit(true, this);
                }

                if (Input.GetButtonDown("No"))
                {
                    gameObject.transform.parent.GetComponent<CharacterSwitcher>().SwitchingEnabled = true;
                    _urns.First().Value.GetComponent<BlindUrnController>().Submit(false, this);
                }
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        carry.flipCargo();
    }
}