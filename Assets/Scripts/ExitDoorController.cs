using UnityEngine;

public class ExitDoorController : AbstractDoorController
{
    public CharacterSwitcher CharacterSwitcher;
    public bool doorOpened = true;
    private Animator _animator;

    public override void Activate(GameObject character)
    {
        CharacterSwitcher.DeleteCharacter(character);
//        character.transform.position = destinationDoor.transform.position;
    }
}