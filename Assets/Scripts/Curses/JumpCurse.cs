public class JumpCurse : Curse
{
    public override void curseCharacter(Character character)
    {
        character.CanJump = false;
    }
}