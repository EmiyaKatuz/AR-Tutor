using UnityEngine;
using UnityEngine.UI;

namespace Resources
{
    public class ButtonController : MonoBehaviour
    {
        public Button rightButton;

        private void Start()
        {
            rightButton.onClick.AddListener(OnButtonClick);
        }

        private static void OnButtonClick()
        {
            Debug.Log("Button clicked!");
        }
    }
}