using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Fade : MonoBehaviour
{

    enum Mode
    {
        FadeIn,
        FadeOut,
    }

    [SerializeField, Header("フェードの時間")]
    private float fadeTime;
    [SerializeField,Header("フェードの種類")]
    private Mode mode;

    private bool bFade;
    private float fadeCount;
    private Image Image;
    private UnityEvent onFadeComplete = new UnityEvent();


    // Start is called before the first frame update
    void Start()
    {
        Image = GetComponent<Image>();
        switch (mode)
        {
            case Mode.FadeIn: fadeCount = fadeTime; break;
            case Mode.FadeOut: fadeCount = 0; break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _Fade();
    }

    private void _Fade()
    {
        if (!bFade) return;

        switch (mode)
        {
            case Mode.FadeIn:FadeIn(); break;
            case Mode.FadeOut:FadeOut(); break;
        }
        float alpha = fadeCount / fadeTime;
        Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, alpha);
    }

    private void FadeIn()
    {
        fadeCount -= Time.deltaTime;
        if (fadeCount <= 0)
        {
            mode = Mode.FadeOut;
            bFade = false;
            onFadeComplete.Invoke();
        }
    }

    private void FadeOut()
    {
        fadeCount += Time.deltaTime;
        if (fadeCount >= 0)
        {
            mode = Mode.FadeIn;
            bFade = false;
            onFadeComplete.Invoke();
        }
    }

    public void FadeStart(UnityAction listener)
    {
        if (bFade) return;
        bFade = true;
        onFadeComplete.AddListener(listener);
    }

}
