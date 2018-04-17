using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Puzzle
{
    public class PuzzleFindButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {

        public Color highlightColor;
        private Color originalColor;

        public Vector3 pressedPosition;
        private Vector3 originalPosition;
        private bool hasBeenPressed = false;

        // Use this for initialization
        void Start()
        {
            originalColor = GetComponent<Renderer>().material.color;
            originalPosition = transform.localPosition;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            GetComponent<Renderer>().material.color = highlightColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {

            GetComponent<Renderer>().material.color = originalColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {


            //transform.localPosition = originalPosition + transform.rotation * pressedPosition;
            hasBeenPressed = true;
            PuzzleUI.Instance.FindMissingPieces();

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            transform.localPosition = originalPosition + transform.rotation * pressedPosition / 5;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            transform.localPosition = originalPosition;
        }
    }
}
