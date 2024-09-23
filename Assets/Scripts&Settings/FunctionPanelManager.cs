using UnityEngine;
using UnityEngine.UI;

public class FunctionPanelManager : MonoBehaviour
{
    // Text component that references the top and bottom information bars
    public Text topNameText;
    public Text bottomInfoText;
    public Text topInfoText;

    // Referencing the Back Button
    public Button backButton;

    // Reference menu background panel
    public GameObject menuBackground;

    // Current Functional Data
    private FunctionData currentFunctionData;

    void Start()
    {
        // Register the click event of the back button
        backButton.onClick.AddListener(OnBackButtonClicked);

        // Initially hide the function panel
        gameObject.SetActive(false);
    }

    // Method of displaying the Function Details panel
    public void ShowFunctionPanel(FunctionData data)
    {
        currentFunctionData = data;

        // Update the text in the top information bar
        topNameText.text = data.name;

        // Update the text in the top info field
        //topInfoText.text = data.info;

        // Update the text in the bottom output field
        //bottomInfoText.text = data.output;

        // Hide menu background
        menuBackground.SetActive(false);

        // Display function panel
        gameObject.SetActive(true);
    }

    // Returns the click event of the button
    public void OnBackButtonClicked()
    {
        // Hide Function Panel
        gameObject.SetActive(false);

        // Show menu background
        menuBackground.SetActive(true);
    }
}