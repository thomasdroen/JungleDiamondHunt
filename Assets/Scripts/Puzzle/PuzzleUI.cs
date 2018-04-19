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
        public Transform cameraToMaxFov;
        public Transform cameraToMinFov;
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

        private bool puzzleStarted = false;

        public bool UIOpened { get; private set; }
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
            if (UIOpened && puzzleStarted &&Input.GetKeyDown(KeyCode.T))
            {
                foreach(PuzzlePiece piece in puzzlePieceContainer.GetComponentsInChildren<PuzzlePiece>())
                {
                    if (!piece.isSet)
                    {
                        piece.CheatPiece();
                    }
                }
            }
        }

        private void OnEnable()
        {
            SettingsMenu.OnChangeFov += OnChangeFoV;
        }

        private void OnDisable()
        {
            SettingsMenu.OnChangeFov -= OnChangeFoV;
        }

        public void OnChangeFoV(object obj, SettingsFovEventArgs args)
        {
            if (UIOpened)
            {
                Debug.Log("Update fov");
                cam.transform.position = getCameraEndPos();
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
            EnablePlayer(false);

            Vector3 camStartPos = cam.transform.position;
            Vector3 camEndPos = getCameraEndPos();
            originalCamPos = cam.transform.localPosition;
            originalCamRot = cam.transform.rotation;
            fromUIToPlayer = cam.transform.parent.position - transform.position;

            float startTime = Time.time;
            minimap.SetActive(false);
            while (Time.time < startTime + time)
            {
                float interpolateValue = (Time.time - startTime) / time;
                float smoothedInterpolatedValue = Mathf.Atan(interpolateValue * 3.43f - 0.9f) * 0.52f + 0.38f;
                cam.transform.position = Vector3.Lerp(camStartPos, camEndPos, smoothedInterpolatedValue);
                cam.transform.rotation = Quaternion.Lerp(originalCamRot, cameraToMaxFov.rotation, smoothedInterpolatedValue);
                yield return null;
            }
            cam.transform.position = camEndPos;
            cam.transform.rotation = cameraToMaxFov.rotation;
            UIOpened = true;

            showCursor(true);
        }

        private Vector3 getCameraEndPos()
        {
            return Vector3.Lerp(cameraToMinFov.position, cameraToMaxFov.position, (cam.fieldOfView - 65)/25);
        }

        private void EnablePlayer(bool enable)
        {
            cam.GetComponent<HeadBob>().enabled = enable;
            RigidbodyFirstPersonController.player.enabled = enable;
        }

        IEnumerator TranslateCameraFromUI(float time)
        {
            Vector3 currentCamPos = cam.transform.position;
            Quaternion currentCamRot = cam.transform.rotation;

            cam.transform.parent.position = cameraFromTransform.position;
            UIOpened = false;
            showCursor(true);

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
            puzzleStarted = true;
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
            StartCoroutine(FinishGame());
            StopCoroutine(timerCoroutine);
        }

        public void reset()
        {
            StopAllCoroutines();
            timerText.text = "";
            radialTimer.fillAmount = 1;
            startButton.reset();
            puzzleStarted = false;
            UIOpened = false;

            puzzlePieceContainer.gameObject.SetActive(false);
            finishedPuzzle.canvasRenderer.SetAlpha(0);
            timerCoroutine = null;

            foreach(PuzzlePiece piece in puzzlePieceContainer.transform.GetComponentsInChildren<PuzzlePiece>())
            {
                piece.isSet = false;
            }
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
            puzzleStarted = false;
            StartCoroutine(TeleportToMaze(0f));
            AudioManager.Instance.PlaySound("Buzzer");
        }

        IEnumerator TeleportToMaze(float delay)
        {
            yield return StartCoroutine(TranslateCameraFromUI(0.75f));
            yield return new WaitForSeconds(delay);
            EnablePlayer(true);
            showCursor(false);
            RigidbodyFirstPersonController.player.transform.position = mazeTeleport.position;
            timeToFinishPuzzle += 30;
            reset();
            AudioManager.Instance.PlaySound("Scream");
        }

        IEnumerator FinishGame()
        {
            yield return StartCoroutine(FadeInFinishedPuzzle(2));
            yield return StartCoroutine(TranslateCameraFromUI(0.75f));

            DiamondAnimator.enabled = true;
            AudioManager.Instance.PlaySound("lock");

            StartCoroutine(finishGame(3.5f));
        }

        private void showCursor(bool show)
        {
            if (show)
                Cursor.lockState = CursorLockMode.None;
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            Cursor.visible = show;
        }
    }
}
