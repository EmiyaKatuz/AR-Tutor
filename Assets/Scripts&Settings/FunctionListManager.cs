using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FunctionListManager : MonoBehaviour
{
    public GameObject functionItemPrefab;
    public List<FunctionData> functionDataList;
    public FunctionPanelManager functionPanelManager;
    public ResultBehaviour resultBehaviour;

    void Start()
    {
        foreach (var data in functionDataList)
        {
            GameObject item = Instantiate(functionItemPrefab, transform);
            FunctionItemController controller = item.GetComponent<FunctionItemController>();
            if (controller != null)
            {
                // Assignment FunctionData
                controller.functionData = data;

                // Initialize the display
                if (controller.functionText)
                {
                    controller.functionText.text = data.name;
                }

                // Assign the function result text
                if (controller.functionResult)
                {
                    controller.functionResult.text = data.output;
                }

                // Assign the ResultBehaviour and FunctionPanelManager references
                // We need to get the Button component to add the click listener
                Button button = item.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.AddListener(() =>
                    {
                        // When the button is clicked, call the OnItemClicked method
                        OnFunctionItemClicked(controller);
                    });
                }
            }
        }
    }

    void OnFunctionItemClicked(FunctionItemController controller)
    {
        // Update the result using the ResultBehaviour
        if (resultBehaviour != null)
        {
            resultBehaviour.UpdateResult(controller.functionData);

            // Update the functionData output
            controller.functionData.output = resultBehaviour.GetOutput();

            // Update the functionResult text (if applicable)
            if (controller.functionResult)
            {
                controller.functionResult.text = controller.functionData.output;
            }
        }

        // Show the function panel using the FunctionPanelManager
        if (functionPanelManager != null)
        {
            functionPanelManager.ShowFunctionPanel(controller.functionData);
        }
    }
}