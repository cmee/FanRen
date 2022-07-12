using UnityEngine;

public class PanelTest : BaseMono
{
    // Start is called before the first frame update
    void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
