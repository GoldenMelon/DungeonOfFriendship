using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Carry : MonoBehaviour
{
    private Transform _carryLocation;
    [HideInInspector] public GameObject Cargo;

    private Dictionary<int, GameObject> _objects;

    void Start()
    {
        _objects = new Dictionary<int, GameObject>();
        _carryLocation = GetComponent<Transform>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("character"))
        {
            var instanceId = other.gameObject.GetInstanceID();
            if (!_objects.ContainsKey(instanceId))
            {
                _objects[instanceId] = other.gameObject;
            }

            Debug.Log("Able to pick cargo " + gameObject.name);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("character"))
        {
            var instanceId = other.gameObject.GetInstanceID();
            if (_objects.ContainsKey(instanceId))
            {
                _objects.Remove(instanceId);
            }
        }
    }

    private void pickUp()
    {
        if (!_objects.Any())
        {
            Debug.Log("Nothing to pick up");
            return;
        }

        GameObject tempCargo = _objects.First().Value;
        Character character = tempCargo.GetComponent<Character>();
        if (!character.canBeCarried)
        {
            Debug.Log("Character cant be carried");
            return;
        }

        Cargo = tempCargo;
        Cargo.transform.SetParent(_carryLocation);
        Cargo.transform.position = _carryLocation.position;

        character.carried = true;
        Cargo.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        Debug.Log("Picking up");
    }

    private void dropCargo()
    {
        if (!Cargo)
        {
            Debug.Log("Cargo not empty");
            return;
        }

        Cargo.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        Character character = Cargo.GetComponent<Character>();
        character.carried = false;
        character.canBeCarried = true;
        Cargo = null;

        _carryLocation.GetChild(0).gameObject.GetComponent<Rigidbody2D>().velocity = transform.forward * 10;
        _carryLocation.GetChild(0).parent = null;
    }

    // Update is called once per frame
    public void PickupOrDropCargo()
    {
        if (Cargo != null)
        {
            dropCargo();
        }
        else
        {
            pickUp();
        }
    }

    public bool carryingSomething()
    {
        return Cargo != null;
    }

    public void flipCargo()
    {
        if (Cargo != null)
        {
            var character = Cargo.GetComponent<Character>();
            character.facingRight = !character.facingRight;
        }
    }
}