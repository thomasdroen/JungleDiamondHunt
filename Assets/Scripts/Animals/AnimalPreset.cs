using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Animal", menuName = "New animal")]
public class AnimalPreset : ScriptableObject {

    public string species;
    public new string name;
    public AudioClip sound;
    public Sprite image;

}
