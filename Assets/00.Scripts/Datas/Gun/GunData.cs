using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunKind
{
    Pistol, Shotgun, SniperRifle
}

[CreateAssetMenu(menuName = "Scriptable Object/GunData", fileName = "GunData")]
public class GunData : ScriptableObject
{
    public int gunID;
    public string gunName;
    public GunKind gunKind;
    public Element element;
    public int damage;
    public int ammoPerShot;
    public int bulletsPerShot;
    public float bulletSpread;
    public float reloadTime;
    public float fireRate;
    public float bulletSpeed;
    public bool canEquipParts;
    public int parts;
    public int maxAmmo;

    public float baseDamage;
    public float basefireRate;
}
