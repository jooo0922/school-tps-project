using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/PlayerCharacterData", fileName = "Player Character Data")]
public class PlayerCharacterData : ScriptableObject
{
    public string characterName;
    public float health;
    public float speed;
    public float jumpForce;
}
