using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FunctionListManager : MonoBehaviour
{
    public GameObject functionItemPrefab;
    public List<FunctionData> functionDataList;
    public FunctionPanelManager functionPanelManager;

    void Start()
    {
        foreach (var data in functionDataList)
        {
            GameObject item = Instantiate(functionItemPrefab, transform);
            //item.transform.Find("Icon").GetComponent<Image>().sprite = data.icon;
            item.transform.Find("FunctionText").GetComponent<Text>().text = data.name;
            //item.transform.Find("BottomText").GetComponent<Text>().text = data.output;
            

            // Adding a click event
            item.GetComponent<Button>().onClick.AddListener(() => OnFunctionItemClicked(data));
        }
    }

    void OnFunctionItemClicked(FunctionData data)
    {
        // Implementing Click Functions
        functionPanelManager.ShowFunctionPanel(data);
    }
}

[System.Serializable]
public class FunctionData
{
    public string name;
    //public Sprite icon;
    //public string info;
    //public string output;
    //public int mode;
}