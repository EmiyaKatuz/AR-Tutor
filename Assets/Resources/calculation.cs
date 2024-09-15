using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

namespace Resources
{
    public class DistanceCalculator : MonoBehaviour
    {
        public GameObject multiTargetObject;

        public GameObject cylinderTargetObject;

        //public GameObject cylinderTargetObject2;
        //public Text distanceText;
        //public Text distanceText2;
        //public Text distanceText3;
        //public Text angleText;
        public Button rightButton;
        public Button leftButton;
        public Text promptTextTitle;
        public Text promptTextFigure;
        public int page;

        private void RightClicked()
        {
            promptTextTitle.text = "Angle between objects: (degrees)";
            page += 1;
        }

        private void LeftClicked()
        {
            promptTextTitle.text = "Distance between objects: (cm)";
            page -= 1;
        }

        void Start()
        {
            rightButton.onClick.AddListener(RightClicked);
            leftButton.onClick.AddListener(LeftClicked);

            promptTextTitle.text = "Distance between objects: (cm)";

            if (multiTargetObject == null || cylinderTargetObject == null)
            {
                Debug.LogError("Please ensure all target objects are assigned.");
            }
        }

        void Update()
        {
            try {
                if (multiTargetObject.activeInHierarchy && cylinderTargetObject.activeInHierarchy)
                {
                    // CalculateAndDisplayDistance();
                    // CalculateAndDisplayAngle();
                    // LogWorldRotation();

                    if (page == 1)
                    {
                        promptTextFigure.text = CalculateAndDisplayAngle();
                    }
                    else if (page == 0)
                    {
                        promptTextFigure.text = CalculateAndDisplayDistance();
                    }
                }
            }
            catch {

            }

        }

        string CalculateAndDisplayDistance() {
            try {
                Vector3 positionA = multiTargetObject.transform.position;
                Vector3 positionB = cylinderTargetObject.transform.position;
                //Vector3 positionC = cylinderTargetObject2.transform.position;

                float distance = Vector3.Distance(positionA, positionB);
                //float distance2 = Vector3.Distance(positionB, positionC);
                //float distance3 = Vector3.Distance(positionA, positionC);

                /*string message =
                    $"Distance between {multiTargetObject.name} and {cylinderTargetObject.name}: {distance} cm";
                string message2 =
                    $"Distance between {cylinderTargetObject.name} and {cylinderTargetObject2.name}: {distance2} cm";
                string message3 =
                    $"Distance between {multiTargetObject.name} and {cylinderTargetObject2.name}: {distance3} cm";

                if (distanceText != null)
                {
                    distanceText.text = message;
                    //distanceText2.text = message2;
                    //distanceText3.text = message3;
                }
                */

                return distance.ToString(CultureInfo.InvariantCulture);

            }
            catch {
                return "err";
            }
        }

        string CalculateAndDisplayAngle()
        {
            Vector3 directionA = multiTargetObject.transform.forward;
            Vector3 directionB = cylinderTargetObject.transform.up;
            float angle = Vector3.Angle(directionA, directionB);

            /*if (angleText != null)
            {
                angleText.text =
                    $"Angle between {multiTargetObject.name} and {cylinderTargetObject.name}: {angle:F2} degrees";
            }
            */

            return angle.ToString(CultureInfo.InvariantCulture);
            // Debug.Log($"Angle between {multiTargetObject.name} and {cylinderTargetObject.name}: {angle:F2} degrees");
        }

        void LogWorldRotation()
        {
            // Get the world rotation angle represented by the Euler angle of the object
            // Vector3 rotationA = multiTargetObject.transform.eulerAngles; // multiTargetObject的欧拉角
            // Vector3 rotationB = cylinderTargetObject.transform.eulerAngles; // cylinderTargetObject的欧拉角

            // Output the rotation angle of each object in the console
            // Debug.Log($"{multiTargetObject.name} World Rotation - X: {rotationA.x:F2}°, Y: {rotationA.y:F2}°, Z: {rotationA.z:F2}°");
            // Debug.Log($"{cylinderTargetObject.name} World Rotation - X: {rotationB.x:F2}°, Y: {rotationB.y:F2}°, Z: {rotationB.z:F2}°");
        }
    }
}