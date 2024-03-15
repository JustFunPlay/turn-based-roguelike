using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] protected int maxHP;
    [SerializeField] protected int currentHP;
    [SerializeField] protected int attack;
    [SerializeField] protected int magic;
    [SerializeField] protected float critChance;
    [SerializeField] protected int speed;
    [SerializeField] protected float armor;
    [SerializeField] protected float resistance;
}
