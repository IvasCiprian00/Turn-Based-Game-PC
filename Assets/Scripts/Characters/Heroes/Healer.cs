using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Healer : HeroScript
{
    [SerializeField] private int _healAmount; 
    override public bool IsHealer() { return true; }
    public override int GetHealAmount() { return _healAmount; }
}
