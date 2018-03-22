using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleUI : MonoBehaviour
{
    public float placePieceThreshold = 2.5f;
    [Space]
    public GameObject minimap;
    public Image finishedPuzzle;
    [Space]
    public float timeToFinishPuzzle = 180;

    public static PuzzleUI Instance
    {
        get; private set;
    }
    [Space]
    public Transform[] pieceSpawnTransforms;
    [Space]
    public Transform puzzlePieceContainer;
    [Space]
    public Transform cameraToTransform;
    public Transform cameraFromTransform;
    [Space]
    public Text timerText;
    public Image radialTimer;

    private Vector3 originalCamPos;
    private Vector3 fromUIToPlayer;
    private Quaternion originalCamRot;

    private Camera cam;

    private bool UIOpened = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        cam = Camera.main;
        puzzlePieceContainer.gameObject.SetActive(false);
        finishedPuzzle.canvasRenderer.SetAlpha(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(FadeInFinishedPuzzle(2));
        }
    }

    public void OpenPuzzleUi()
    {
        StartCoroutine(TranslateCameraToUI(0.75f));
    }

    IEnumerator TranslateCameraToUI(float time)
    {
        Vector3 camStartPos = cam.transform.position;
        originalCamPos = cam.transform.localPosition;
        originalCamRot = cam.transform.rotation;
        fromUIToPlayer = cam.transform.parent.position - transform.position;

        float startTime = Time.time;
        minimap.SetActive(false);
        while (Time.time < startTime + time)
        {
            float interpolateValue = (Time.time - startTime) / time;
            float smoothedInterpolatedValue = Mathf.Atan(interpolateValue * 3.43f - 0.9f) * 0.52f + 0.38f;
            cam.transform.position = Vector3.Lerp(camStartPos, cameraToTransform.position, smoothedInterpolatedValue);
            cam.transform.rotation = Quaternion.Lerp(originalCamRot, cameraToTransform.rotation, smoothedInterpolatedValue);
            yield return null;
        }
        cam.transform.position = cameraToTransform.position;
        cam.transform.rotation = cameraToTransform.rotation;


        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator TranslateCameraFromUI(float time)
    {
        Vector3 currentCamPos = cam.transform.position;
        Quaternion currentCamRot = cam.transform.rotation;

        cam.transform.parent.position = transform.position +
                                        new Vector3(-transform.forward.x, fromUIToPlayer.y,
                                            -transform.forward.z) * fromUIToPlayer.magnitude;

        float startTime = Time.time;

        while (Time.time < startTime + time)
        {
            float interpolateValue = (Time.time - startTime) / time;
            float smoothedInterpolatedValue = Mathf.Atan(interpolateValue * 3.43f - 0.9f) * 0.52f + 0.38f;
            cam.transform.position = Vector3.Lerp(currentCamPos, cam.transform.parent.TransformPoint(originalCamPos), smoothedInterpolatedValue);
            cam.transform.rotation = Quaternion.Lerp(currentCamRot, originalCamRot, smoothedInterpolatedValue);
            yield return null;
        }
        cam.transform.position = cam.transform.parent.TransformPoint(originalCamPos);
        cam.transform.rotation = originalCamRot;
        minimap.SetActive(true);


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void StartPuzzle()
    {
        puzzlePieceContainer.gameObject.SetActive(true);
        finishedPuzzle.canvasRenderer.SetAlpha(0);
        //numberOfPuzzlePieces = 0;
        //numberCorrectPuzzlePieces = 0;
        foreach (var rectTransform in puzzlePieceContainer.transform.GetComponentsInChildren<RectTransform>())
        {
            int startIndex = Mathf.RoundToInt(Random.value * (pieceSpawnTransforms.Length - 1));
            startIndex = (startIndex == pieceSpawnTransforms.Length - 1) ? startIndex - 1 : startIndex;

            rectTransform.position = Vector3.Lerp(pieceSpawnTransforms[startIndex].position,
                pieceSpawnTransforms[startIndex + 1].position, Random.value);
            //numberOfPuzzlePieces++;
        }
        StartCoroutine(StartTimer());
    }

    public void FindMissingPieces()
    {
        foreach (var piece in puzzlePieceContainer.transform.GetComponentsInChildren<PuzzlePiece>())
        {
            if (!piece.isSet)
            {
                piece.transform.SetAsLastSibling();
            }
        }
    }

    public void CheckIfPuzzleIsComplete()
    {
        foreach (var piece in puzzlePieceContainer.GetComponentsInChildren<PuzzlePiece>())
        {
            if (!piece.isSet)
            {
                return;
            }
        }
        StartCoroutine(FadeInFinishedPuzzle(2));
        StopCoroutine(StartTimer());
    }

    IEnumerator FadeInFinishedPuzzle(float time, IEnumerator nextCoroutine = null)
    {
        finishedPuzzle.canvasRenderer.SetAlpha(1);

        float startTime = Time.time;
        while (Time.time < startTime + time)
        {
            float interpolateValue = (Time.time - startTime) / time;
            foreach (var piece in puzzlePieceContainer.GetComponentsInChildren<Image>())
            {
                piece.canvasRenderer.SetAlpha(1 - interpolateValue);
            }
            
            yield return null;
        }
        Debug.Log("finished fading");
    }

    IEnumerator StartTimer()
    {
        float startTime = Time.time;
        float timeLeft = timeToFinishPuzzle;
        while (Time.time < startTime + timeToFinishPuzzle)
        {
            timeLeft -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(timeLeft / 60);
            int seconds = Mathf.FloorToInt(timeLeft % 60);
            timerText.text = minutes + ":" + seconds;
            radialTimer.fillAmount = timeLeft / timeToFinishPuzzle;
            yield return null;
        }
        Debug.Log("Time is up");
    }

}
