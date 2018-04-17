using UnityEngine;

namespace Assets.Scripts.Animals.Quests
{
    [RequireComponent(typeof(SphereCollider), typeof(Sprite), typeof(AudioSource))]
    public class Collectible : MonoBehaviour {

        public GameObject player;

        private Sprite foodSprite;

        private void Start()
        {
            if(player == null)
            {
                player = Camera.main.transform.parent.gameObject;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(gameObject.activeSelf && other.gameObject == player)
            {
                AnimalUI.instance.collectCollectible();
                GetComponent<AudioSource>().Play();

                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<SphereCollider>().enabled = false;
            }
        }

        public void setSprite(Sprite sprite)
        {
            foodSprite = sprite;
        }
    }
}
