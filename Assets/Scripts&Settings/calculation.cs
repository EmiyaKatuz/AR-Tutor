using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

namespace Resources {
    public class DistanceCalculator : MonoBehaviour {
        public GameObject multiTargetObject;
        public GameObject cylinderTargetObject;
        public GameObject cylinderTargetObject2;
        public Text distanceText;
        public Text distanceText2;
        public Text distanceText3;
        public Text angleText;
        public Button rightButton;
        public Button leftButton;
        public Text promptTextTitle;
        public Text promptTextFigure;
        public int page = 0;

        public void rightClicked() {
            promptTextTitle.text = "Angle between objects: (degrees)";
            page += 1;
        }

        public void leftClicked() {
            promptTextTitle.text = "Distance between objects: (cm)";
            page -= 1;
        }

        void Start() {
            rightButton.onClick.AddListener(rightClicked);
            leftButton.onClick.AddListener(leftClicked);

            promptTextTitle.text = "Distance between objects: (cm)";

            if (multiTargetObject == null || cylinderTargetObject == null || cylinderTargetObject2 == null) {
                Debug.LogError("Please ensure all target objects are assigned.");
            }
        }

        void Update() {
            if (multiTargetObject.activeInHierarchy && cylinderTargetObject.activeInHierarchy) {
                // CalculateAndDisplayDistance();
                // CalculateAndDisplayAngle();
                // LogWorldRotation();

                if (page == 1) {
                    promptTextFigure.text = CalculateAndDisplayAngle();
                }
                else if (page == 0) {
                    promptTextFigure.text = CalculateAndDisplayDistance();
                }
            }
        }

        string CalculateAndDisplayDistance() {
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

            if (distanceText && distanceText2 && distanceText3 != null) {
                distanceText.text = message;
                distanceText2.text = message2;
                distanceText3.text = message3;
            }

            return distance.ToString();
        }

        string CalculateAndDisplayAngle() {
            Vector3 directionA = multiTargetObject.transform.forward;
            Vector3 directionB = cylinderTargetObject.transform.forward;
            float angle = Vector3.Angle(directionA, directionB);

            if (angleText != null) {
                angleText.text =
                    $"Angle between {multiTargetObject.name} and {cylinderTargetObject.name}: {angle:F2} degrees";
            }

            return angle.ToString();
            // Debug.Log($"Angle between {multiTargetObject.name} and {cylinderTargetObject.name}: {angle:F2} degrees");
        }

        void LogWorldRotation() {
            // 获取物体的欧拉角表示的世界旋转角度
            // Vector3 rotationA = multiTargetObject.transform.eulerAngles; // multiTargetObject的欧拉角
            // Vector3 rotationB = cylinderTargetObject.transform.eulerAngles; // cylinderTargetObject的欧拉角

            // 在控制台中输出每个物体的旋转角度
            // Debug.Log($"{multiTargetObject.name} World Rotation - X: {rotationA.x:F2}°, Y: {rotationA.y:F2}°, Z: {rotationA.z:F2}°");
            // Debug.Log($"{cylinderTargetObject.name} World Rotation - X: {rotationB.x:F2}°, Y: {rotationB.y:F2}°, Z: {rotationB.z:F2}°");
        }
    }
}