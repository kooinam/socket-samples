using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorPopupView : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform = null;

    [SerializeField]
    private Text errorMessageText = null;

    [SerializeField]
    private float duration = 0.3f;

    public void Setup(string errorMessage) {
        this.errorMessageText.text= errorMessage;

        iTween.ValueTo(this.gameObject, new Hashtable() {
            { "from", 0 },
            { "to", 1 },
            { "time", this.duration },
            { "onupdate", "animateScale" },
        });
    }

    public void OnClickClose() {
        iTween.ValueTo(this.gameObject, new Hashtable() {
            { "from", 1 },
            { "to", 0 },
            { "time", this.duration },
            { "onupdate", "animateScale" },
            { "oncomplete", "close" },
        });
    }

    private void animateScale(float scale)
    {
        this.rectTransform.localScale = new Vector3(scale, scale, 1);
    }

    private void close() {
        GameObject.Destroy(this.gameObject);
    }
}
