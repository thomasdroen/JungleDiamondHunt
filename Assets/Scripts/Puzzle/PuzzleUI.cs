using System.Collections;
using Assets.Scripts.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Puzzle
{
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
        [Space] public PuzzleStartButton startButton;
        [Space]
        public Text timerText;
        public Image radialTimer;

        private Vector3 originalCamPos;
        private Vector3 fromUIToPlayer;
        private Quaternion originalCamRot;
        private Coroutine timerCoroutine;

        private Camera cam;

        private bool UIOpened = false;
        public Transform mazeTeleport;
        [Space] public Animator DiamondAnimator;
        public Transform diamond;
        //public GameOverManager gameOver;

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
                StartCoroutine(TranslateCameraFromUI(0.75f));
            }
        }

        public void OpenPuzzleUi()
        {
            if (UIOpened)
            {
                if (timerCoroutine == null)
                {
                    StartCoroutine(TranslateCameraFromUI(0.75f));
                    UIOpened = false;
                }
            }
            else
            {
                StartCoroutine(TranslateCameraToUI(0.75f));
            }
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

            cam.transform.parent.position = cameraFromTransform.position;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Quaternion toRot = Quaternion.LookRotation(transform.position - cam.transform.parent.position, Vector3.up);

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

            //RigidbodyFirstPersonController.player.enabled = true;
            DiamondAnimator.enabled = true;
            AudioManager.Instance.PlaySound("lock");

            StartCoroutine(finishGame(3.5f));
        }

        IEnumerator finishGame(float time)
        {
            float startTime = Time.time;

            AudioManager.Instance.PlaySound("choir");

            while (Time.time < startTime + time)
            {
                Vector3 toDiamond = diamond.position - cam.transform.position;
                cam.transform.rotation =
                    Quaternion.Lerp(cam.transform.rotation, Quaternion.LookRotation(toDiamond, Vector3.up), 5 * Time.deltaTime);

                yield return null;
            }
            MenuManager.Instance.FinishGame();
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlaySound("victory");
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
            timerCoroutine = StartCoroutine(StartTimer());
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
            StopCoroutine(timerCoroutine);
        }

        public void reset()
        {
            StopAllCoroutines();
            timerText.text = "";
            radialTimer.fillAmount = 1;
            startButton.reset();

            puzzlePieceContainer.gameObject.SetActive(false);
            finishedPuzzle.canvasRenderer.SetAlpha(0);
            timerCoroutine = null;
        }

        IEnumerator FadeInFinishedPuzzle(float time)
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
            StartCoroutine(TranslateCameraFromUI(0.75f));
            //Debug.Log("finished fading");
        }

        IEnumerator StartTimer()
        {
            float startTime = Time.time;
            float timeLeft = timeToFinishPuzzle;
            while (Time.time < startTime + timeToFinishPuzzle)
            {
                timeLeft -= Time.deltaTime;
                int minutes = (int)(timeLeft / 60);
                int seconds = (int)(timeLeft % 60);
                timerText.text = minutes + ":" + string.Format("{0:00}", seconds);
                radialTimer.fillAmount = timeLeft / timeToFinishPuzzle;
                yield return null;
            }
            StartCoroutine(TeleportToMaze(0f));
            //Debug.Log("Time is up");
        }

        IEnumerator TeleportToMaze(float delay)
        {
            yield return StartCoroutine(TranslateCameraFromUI(0.75f));
            yield return new WaitForSeconds(delay);
            RigidbodyFirstPersonController.player.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            RigidbodyFirstPersonController.player.transform.position = mazeTeleport.position;

            reset();
        }
    }
}
