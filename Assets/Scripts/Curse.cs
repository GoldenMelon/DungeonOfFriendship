using UnityEngine;

public abstract class Curse : MonoBehaviour
{
   
   public GameObject modalWindow;
   public DoorController doorController;
   private bool cursed = false;

   public bool Interact()
   {
      if (!cursed)
      {
         modalWindow.SetActive(true);
         modalWindow.transform.SetAsLastSibling();
      }

      return !cursed;
   }

   public void Submit(bool answer, Character character)
   {
      modalWindow.SetActive(false);
      modalWindow.transform.SetAsLastSibling();
      if (answer)
      {
         doorController.doorOpened = true;
         cursed = true;
         curseCharacter(character);
      }
   }

   public abstract void curseCharacter(Character character);
}
