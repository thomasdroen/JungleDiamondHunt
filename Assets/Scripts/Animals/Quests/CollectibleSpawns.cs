using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpawns : MonoBehaviour {

    
    List<Collectible> collectibles;
    // Use this for initialization
    void Start() {
        collectibles = new List<Collectible>();
        foreach (Collectible col in gameObject.GetComponentsInChildren<Collectible>())
        {
            collectibles.Add(col);
            col.gameObject.SetActive(false);
        }
    }

    public void spawnCollectibles(int amount)
    {


        if (amount > collectibles.Count)
        {
            Debug.LogError("Requested more collectible spawns than available! Add more, or reduce amount.");
            return;
        }

        RandomExtensions.Shuffle(new System.Random(), collectibles);
        for(int i = 0; i < amount; i++)
        {
            collectibles[i].gameObject.SetActive(true);
        }
    }
	
	public void despawnCollectibles()
    {
        foreach(Collectible col in collectibles)
        {
            col.gameObject.SetActive(false);
        }
    }
}
