using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collectible quest", menuName = "New collectible quest")]
public class CollectionQuest : Quest {

    public Sprite collectibleSprite;
    public int numberOfCollectibles;
    [TextArea]
    public string notEnough;
    public string finishWithQuestText;

}
