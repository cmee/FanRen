using UnityEngine;
using UnityEngine.UI;

public class TextDamageController : BaseMono
{

    RectTransform rtf;
    float targetY;

    // Start is called before the first frame update
    void Start()
    {
        rtf = GetComponent<RectTransform>();
        targetY = rtf.position.y + 80;

        //Debug.Log("rtf.rect.y " + rtf.rect.y);
        //Debug.Log("rtf.rect.position.y " + rtf.rect.position.y);
        //Debug.Log("rtf.position.y " + rtf.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * Time.deltaTime * 100, Space.Self);
        if (rtf.position.y > targetY)
        {
            Destroy(this.gameObject);
        }
    }

}
