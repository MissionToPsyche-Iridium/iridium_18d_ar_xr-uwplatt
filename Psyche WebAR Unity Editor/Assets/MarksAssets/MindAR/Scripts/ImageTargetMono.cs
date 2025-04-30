using UnityEngine;
using UnityEngine.Events;

namespace MarksAssets.MindAR {
    public class ImageTargetMono : MonoBehaviour {
        public int targetIndex = 0;
        public UnityEvent targetFound;
        public UnityEvent targetLost;

        [SerializeField] private GameObject Cube;

        #pragma warning disable CS0414
        private bool ChangeSceneCheck;
        private Vector3 SavedpositionSceneCheck;

        public ImageTarget imageTarget;
        private Vector3 position = new Vector3();
        private Quaternion rotation = new Quaternion();
        private Vector3 scale = new Vector3();

        void Start () {
        #if UNITY_WEBGL && !UNITY_EDITOR
        if (!MindAR.isRunning()) MindAR.start();

        imageTarget = MindAR.imageTargets[targetIndex];

        imageTarget.targetFound += OnTargetFound;

        imageTarget.targetLost += OnTargetLost;

        enabled = false;

        ChangeSceneCheck = false;
        #endif
        }

        public void OnTargetFound()
        {
            Cube.SetActive(true);
            targetFound.Invoke();
            enabled = true;
            ChangeSceneCheck = false;

            Debug.Log("OnTargetFound");
        }

        public void OnTargetLost()
        {
            Cube.SetActive(false);
            targetLost.Invoke();
            enabled = false;
            ChangeSceneCheck = false;

            Debug.Log("OnTargetLost");
        }

        void Update () {
#if UNITY_WEBGL && !UNITY_EDITOR
            //Debug.Log(imageTarget.posx + " " + imageTarget.posy + " " + imageTarget.posz + " " + Cube.activeSelf);
            if (!Cube.activeSelf)
            {
                if (!ChangeSceneCheck)
                {
                    SavedpositionSceneCheck = new Vector3(imageTarget.posx, imageTarget.posy, imageTarget.posz);
                    ChangeSceneCheck = true;
                }
                else if (imageTarget.posx != SavedpositionSceneCheck.x)
                {
                    Cube.SetActive(true);
                    ChangeSceneCheck = false;
                }
                imageTarget.FlingTargetToFarLands();
            }


            position.Set(imageTarget.posx, imageTarget.posy, imageTarget.posz);
            rotation.Set(imageTarget.rotx, imageTarget.roty, imageTarget.rotz, imageTarget.rotw);
            scale.Set(imageTarget.scale, imageTarget.scale, imageTarget.scale);

            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = scale;
#endif
        }
    }
}
