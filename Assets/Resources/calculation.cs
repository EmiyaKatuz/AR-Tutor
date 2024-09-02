using UnityEngine;
using UnityEngine.UI;
using Vuforia;

namespace Resources
{
    public class DistanceCalculator : MonoBehaviour
    {
        public GameObject multiTargetObject; // The 3D Object attached to the multiTarget
        public GameObject cylinderTargetObject; // The 3D Object attached to the cylinderTarget
        public GameObject cylinderTargetObject2;
        public Text distanceText; // UI Text (legacy) component to display the distance
        public Text distanceText2;
        public Text distanceText3;

        void Start()
        {
            if (multiTargetObject == null || cylinderTargetObject == null || cylinderTargetObject2 == null)
            {
                Debug.LogError("Please ensure both multiTargetObject and cylinderTargetObject are assigned.");
            }

            var exposureMode = ExposureMode.EXPOSURE_MODE_CONTINUOUSAUTO;
            if (VuforiaBehaviour.Instance.CameraDevice.IsExposureModeSupported(exposureMode))
            {
                Debug.Log("Yes");
                VuforiaBehaviour.Instance.CameraDevice.SetExposureMode(ExposureMode.EXPOSURE_MODE_CONTINUOUSAUTO);
            }
            else
            {
                Debug.Log("No");
            }
        }

        void Update()
        {
            if (multiTargetObject.activeInHierarchy && cylinderTargetObject.activeInHierarchy)
            {
                CalculateAndDisplayDistance();
            }
        }

        void CalculateAndDisplayDistance()
        {
            Vector3 positionA = multiTargetObject.transform.position;
            Vector3 positionB = cylinderTargetObject.transform.position;
            Vector3 positionC = cylinderTargetObject2.transform.position;

            float distance = Vector3.Distance(positionA, positionB);
            float distance2 = Vector3.Distance(positionB, positionC);
            float distance3 = Vector3.Distance(positionA, positionC);

            string message =
                $"Distance between {multiTargetObject.name} and {cylinderTargetObject.name}: {distance} cm";
            string message2 =
                $"Distance between {cylinderTargetObject.name} and {cylinderTargetObject2.name}: {distance2} cm";
            string message3 =
                $"Distance between {multiTargetObject.name} and {cylinderTargetObject2.name}: {distance3} cm";

            // Display the distance on the UI Text (legacy) component
            if (distanceText && distanceText2 && distanceText3 != null)
            {
                distanceText.text = message;
                distanceText2.text = message2;
                distanceText3.text = message3;
            }
        }
    }
}