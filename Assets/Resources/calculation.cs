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
        public Text angleText;

        void Start()
        {
            if (multiTargetObject == null || cylinderTargetObject == null || cylinderTargetObject2 == null)
            {
                Debug.LogError("Please ensure all target objects are assigned.");
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
                CalculateAndDisplayAngle(); // Calculate and display angle
                LogWorldRotation();
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

        void CalculateAndDisplayAngle()
        {
            // 获取物体的正向向量作为方向向量
            Vector3 directionA = multiTargetObject.transform.forward; // multiTargetObject的正向向量
            Vector3 directionB = cylinderTargetObject.transform.forward; // cylinderTargetObject的正向向量

            float angle = Vector3.Angle(directionA, directionB);

            if (angleText != null)
            {
                angleText.text = $"Angle between {multiTargetObject.name} and {cylinderTargetObject.name}: {angle:F2} degrees";
            }

            // Debug.Log($"Angle between {multiTargetObject.name} and {cylinderTargetObject.name}: {angle:F2} degrees");
        }

        void LogWorldRotation()
        {
            // 获取物体的欧拉角表示的世界旋转角度
            // Vector3 rotationA = multiTargetObject.transform.eulerAngles; // multiTargetObject的欧拉角
            // Vector3 rotationB = cylinderTargetObject.transform.eulerAngles; // cylinderTargetObject的欧拉角

            // 在控制台中输出每个物体的旋转角度
            // Debug.Log($"{multiTargetObject.name} World Rotation - X: {rotationA.x:F2}°, Y: {rotationA.y:F2}°, Z: {rotationA.z:F2}°");
            // Debug.Log($"{cylinderTargetObject.name} World Rotation - X: {rotationB.x:F2}°, Y: {rotationB.y:F2}°, Z: {rotationB.z:F2}°");
        }
    }
}