using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._01_Scripts.Weapons
{
    internal interface InterfaceWeapons
    {
        public void Attack();
        public void NotAttacking();
        public void ResetWeapon();
        public WeaponType weaponType { get; set; }
        public int manaCost { get; set; }
    }
    public enum WeaponType
    {
        Melee,
        Range,
        Magic,
        Throwable
    }
}
