using UnityEngine;
using UnityEngine.UI;

public class FunctionItemController : MonoBehaviour
{
    public FunctionData functionData;

    public Text functionText;

    //public Image functionImage;
    //public Text functionInfo;
    public Text functionResult;

    void Start()
    {
        if (functionText)
        {
            functionText.text = functionData.name;
        }
    }
}