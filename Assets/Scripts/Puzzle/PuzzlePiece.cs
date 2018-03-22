using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private float thresholdMultiplier = 300;

    public PuzzlePiece[] adjacentPieces;

    public bool isSet { get; private set; }

    [SerializeField]
    private Vector3 correctPosition;

    private static float clampOffset = 60;
    private RectTransform parentCanvas;
    private RectTransform rTransform;

    private Vector2 startDragPosition;
    private Vector2 startDragMousePosition;

    private Vector3 dragPositionOffest;
    private float panelHeight;

    private bool isBeingDragged = false;



    void Awake()
    {
        rTransform = GetComponent<RectTransform>();
        correctPosition = rTransform.position;
        parentCanvas = transform.GetComponentInParent<Canvas>().transform as RectTransform;
        //cam = Camera.main;
        if (adjacentPieces.Length > 4)
        {
            Debug.LogError("Too many adjacent pieces in " + gameObject.name);
        }
        Debug.Log(Vector2.Distance(correctPosition, rTransform.position));

        panelHeight = transform.parent.parent.GetComponent<RectTransform>().sizeDelta.y;
        Debug.Log(panelHeight);

    }

    public void place()
    {
        float distanceThreshold = PuzzleUI.Instance.placePieceThreshold;
        //Debug.Log(Vector3.Distance(correctPosition, rTransform.position) * thresholdMultiplier + " <= " + distanceThreshold + " can be set = " + canBeSet());
        if (Vector3.Distance(correctPosition, rTransform.position) * thresholdMultiplier <= distanceThreshold && canBeSet())
        {
            isSet = true;
            rTransform.position = correctPosition;
            PuzzleUI.Instance.CheckIfPuzzleIsComplete();
        }
        else
        {
            //Debug.Log("Missed");
        }
    }

    private bool canBeSet()
    {
        if (adjacentPieces.Length < 4)
        {
            return true;
        }
        foreach (var adjacentPiece in adjacentPieces)
        {
            if (adjacentPiece.isSet)
            {
                return true;
            }
        }
        return false;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isSet)
            return;
        if (parentCanvas == null)
        {
            Debug.LogError("ParentCanvas not found!");
        }
        rTransform.SetAsLastSibling();
        //startDragMousePosition = eventData.position;
        //startDragPosition = rTransform.localPosition;


        dragPositionOffest = rTransform.position - GetGlobalMousePosition(eventData);

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isSet)
            return;
        Vector3 rawPosition = GetGlobalMousePosition(eventData) + dragPositionOffest;
        float canvasWidth = parentCanvas.sizeDelta.x / 2;
        float canvasHeight = parentCanvas.sizeDelta.y / 2;
        Vector2 canvasPosition = parentCanvas.InverseTransformPoint(rawPosition);
        Vector2 clampedPosition = new Vector3(Mathf.Clamp(canvasPosition.x, -canvasWidth + clampOffset, canvasWidth - clampOffset), Mathf.Clamp(canvasPosition.y, -canvasHeight + clampOffset * 1.15f - panelHeight, canvasHeight - clampOffset * 1.15f));
        rTransform.position = parentCanvas.TransformPoint(clampedPosition);
        //Debug.Log("InverseTransform to canvas: " + parentCanvas.InverseTransformPoint(rTransform.position));

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        place();
    }

    private Vector3 GetGlobalMousePosition(PointerEventData eventData)
    {
        Vector3 globalMousePos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(parentCanvas, eventData.position, eventData.pressEventCamera, out globalMousePos);
        return globalMousePos;
    }

}
