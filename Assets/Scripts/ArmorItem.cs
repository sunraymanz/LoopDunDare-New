using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorItem : MonoBehaviour
{
    public int def;
    public int hp;
    public int energy;
    public int energyRegen;
    public Sprite spr;
    public ArmorItem token;

    private void Awake()
    {
        def = token.def;
        hp = token.hp;
        energy = token.energy;
        energyRegen = token.energyRegen;
        spr = token.spr;
    }

}
