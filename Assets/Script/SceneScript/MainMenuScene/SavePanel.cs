using UnityEngine;

public class SavePanel : BaseMono
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnGoHeadButtonClick()
    {
        Debug.Log("OnGoHeadButtonClick");
        //gameObject.SetActive(true);
    }

    public void OnClosePanelClick()
    {
        Debug.Log("OnClosePanelClick");
        gameObject.SetActive(false);
    }

}
