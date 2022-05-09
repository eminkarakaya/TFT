using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ozellik
{
    kartel,
    kimyasal,
    atilgan,
    yordle,
    kavgaci,
    hextech,
    fedai,
    sihirUstadi,
    kazanova,
    koruma,
    cifteAtis,
    bos
}
public class Hero : MonoBehaviour
{
    public HeroData heroData;
    Ozellik ozellik;
    Ozellik ozellik2;
    Ozellik ozellik3;
    public float speed;
    public float damage;
    public float range;
}
