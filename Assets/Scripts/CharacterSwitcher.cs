using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSwitcher : MonoBehaviour
{
    public List<GameObject> gameObjects;
    public Camera camera;
    public LayerMask allMask;
    public LayerMask characterMask;
    private int position = 0;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    public bool SwitchingEnabled = true;

    // Use this for initialization
    void Start()
    {
        reactivate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Tab") && SwitchingEnabled)
        {
            position = (position + 1) % gameObjects.Count;
            reactivate();
        }

        if (Input.GetButtonDown("Restart"))
        {
            SceneManager.LoadScene("Level 1");
        }

        if (gameObjects.Count != 0)
        {
            Vector3 targetPosition = gameObjects[position].transform.TransformPoint(new Vector3(3, 0, -20));

            // Smoothly move the camera towards that target position
            camera.transform.position =
                Vector3.SmoothDamp(camera.transform.position, targetPosition, ref velocity, smoothTime);

            if (gameObjects[position].GetComponent<Character>().Blind)
            {
                camera.cullingMask = characterMask;
            }
            else
            {
                camera.cullingMask = allMask;
            }
        }
    }

    void reactivate()
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            Character character = gameObjects[i].GetComponent<Character>();

            character.active = i == position;
            if (character.carryingSomebody || character.carried)
            {
                character.canBeCarried = false;
            }
            else
            {
                character.canBeCarried = i != position;
            }


            if (i != position)
            {
                Debug.Log(i + "can be carried");
            }
            else
            {
                Debug.Log(i + "cant be carried");
            }
        }
    }

    public void DeleteCharacter(GameObject character)
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (gameObjects[i] == character)
            {
                Character component = gameObjects[i].GetComponent<Character>();
                if (component.carryingSomebody)
                {
                    gameObjects.Remove(component.carry.Cargo);
                    Destroy(component.carry.Cargo);
                }
                gameObjects.Remove(character);
                Destroy(character);
            }
        }

        if (gameObjects.Count == 0)
        {
            SceneManager.LoadScene("Ending");
        }
        else
        {
            position = position % gameObjects.Count;
            reactivate();
        }
    }
}