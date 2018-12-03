using UnityEngine;

public class DoorController : AbstractDoorController
{

	public GameObject destinationDoor;
	public bool doorOpened = false;
	private Animator _animator;

	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		_animator.SetBool("Opened", doorOpened);
	}

	public override void Activate(GameObject character)
	{
		Debug.Log("PIdorasina najal knopku");
		if (destinationDoor && doorOpened)
		character.transform.position = destinationDoor.transform.position;
	}
}
