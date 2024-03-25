using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSetupTest : MonoBehaviour
{
    public CharacterStats character;
    public PlayerClass playerClass;
    public int levelToSet;

    public void GoLevelUp()
    {
        character.LevelUp();
    }
    public void Initialize()
    {
        character.SetUpCharacter(playerClass, levelToSet);
    }
}
