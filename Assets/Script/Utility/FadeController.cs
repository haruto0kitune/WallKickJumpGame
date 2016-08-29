using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public enum EFadeState
    {
        In,         // フェードイン完了.
        Out,        // フェードアウト完了.
        Process,    // フェード処理中.
    }


    private EFadeState m_state = default(EFadeState);
    private Image m_image = null;
    private Canvas m_canvas = null;

    // UIよりも手前に表示するために大きい値をCanvasの順に指定しています。
    // フェードスプライトより手前にものを表示する必要がある場合などは、値を調整する必要があります。
    private readonly int DEFAULT_CANVAS_ORDER = 1000;

    private readonly float DEFAULT_TIME = 0.5f;
    private readonly Color DEFAULT_COLOR = Color.black;


    public EFadeState State
    {
        get
        {
            return m_state;
        }
        private set
        {
            m_state = value;

            if (value == EFadeState.In)
            {
                FadeAlpha = 0.0f;
            }
            if (value == EFadeState.Out)
            {
                FadeAlpha = 1.0f;
            }
        }
    }

    /// <summary>
    /// キャンバスの描画順。
    /// フェードスプライトより手前にものを表示する必要がある場合などの調整用。
    /// </summary>
    public int CanvasOrder
    {
        get
        {
            return m_canvas.sortingOrder;
        }
        set
        {
            m_canvas.sortingOrder = value;
        }
    }

    private Color FadeColor
    {
        get
        {
            return m_image.color;
        }
        set
        {
            m_image.color = value;
        }

    }

    private float FadeAlpha
    {
        get
        {
            return FadeColor.a;
        }
        set
        {
            var color = FadeColor;
            color.a = value;
            FadeColor = color;
        }
    }

    void Awake()
    {
        gameObject.name = "FadeController";
        DontDestroyOnLoad(gameObject);

        // Canvasの作成。
        // Hierarchy上に別のCanvasがScreenSpaceOverlayである場合は、
        // sortingOrderの値に気を付ける必要があります。
        {
            var canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = DEFAULT_CANVAS_ORDER;

            m_canvas = canvas;
        }

        // CanvasScalerの作成。
        // モードをScaleWithScreenSizeにしてreferenceResolutionを(1,1)にすることで、
        // 解像度に関係なく画面全体に対応できる……はず。
        {
            var scaler = gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = Vector2.one;
        }

        // Fade用Spriteの作成。
        {
            var obj = new GameObject("FadeImage");
            obj.transform.SetParent(transform, false);
            var image = obj.AddComponent<Image>();

            m_image = image;
        }

        State = EFadeState.In;
    }

    /// <summary>
    /// フェードアウトを開始する。
    /// </summary>
    /// <param name="i_time">フェード処理時間</param>
    /// <param name="i_color">フェードの色</param>
    /// <returns>true:フェードの開始に成功 false:失敗</returns>
    public bool FadeOut(float i_time, Color i_color)
    {
        return StartFade(i_time, i_color, EFadeState.Out);
    }
    public bool FadeOut()
    {
        return FadeOut(DEFAULT_TIME, DEFAULT_COLOR);
    }
    public bool FadeOut(float i_time)
    {
        return FadeOut(i_time, DEFAULT_COLOR);
    }
    public bool FadeOut(Color i_color)
    {
        return FadeOut(DEFAULT_TIME, i_color);
    }

    /// <summary>
    /// フェードインを開始する。
    /// </summary>
    /// <param name="i_time">フェード処理時間</param>
    /// <returns>true:フェードの開始に成功 false:失敗</returns>
    public bool FadeIn(float i_time)
    {
        return StartFade(i_time, FadeColor, EFadeState.In);
    }
    public bool FadeIn()
    {
        return FadeIn(DEFAULT_TIME);
    }



    /// <summary>
    /// フェードを設定し、開始する。
    /// </summary>
    /// <param name="i_time">フェード処理時間</param>
    /// <param name="i_color">フェードの色</param>
    /// <param name="i_fade">フェードイン、フェードアウト</param>
    /// <returns>true:フェードの開始に成功 false:失敗</returns>
    private bool StartFade(float i_time, Color i_color, EFadeState i_fade)
    {
        // フェード処理中にはフェードの上書きは禁止する。
        // フェードの上書きを有効にした場合、先にフェード処理を呼出したコンポーネントの処理に影響が出る可能性があるため。
        if (State == EFadeState.Process)
        {
            return false;
        }

        // すでに指定したフェード状態になっている場合の上書きも禁止する。
        // こちらはフェード処理中ではないため、先に呼び出したコンポーネントの処理に影響が出る可能性は低いが、
        // フェード状況
        if (State == i_fade)
        {
            return false;
        }

        FadeColor = i_color;

        i_time = Mathf.Max(i_time, 0.0f);
        if (i_time <= Mathf.Epsilon)
        {
            State = i_fade;
            return true;
        }

        State = EFadeState.Process;
        StartCoroutine(FadeProcess(i_time, i_fade));
        return true;
    }

    /// <summary>
    /// フェード処理
    /// Update()もいいけど、現在の時間などのメンバ変数を作りたくなかったのでこの方式で。
    /// </summary>
    /// <param name="i_time">フェード処理時間</param>
    /// <param name="i_fade">フェードイン、フェードアウト</param>
    /// <returns></returns>
    private IEnumerator FadeProcess(float i_time, EFadeState i_fade)
    {
        float startAlpha = 0.0f;
        float targetAlpha = 0.0f;
        if (i_fade == EFadeState.In)
        {
            startAlpha = 1.0f;
            targetAlpha = 0.0f;
        }
        else
        {
            startAlpha = 0.0f;
            targetAlpha = 1.0f;
        }


        FadeAlpha = startAlpha;


        // FadeControllerがシーンをまたぐことも可能なため、
        // Time.timeSinceLevelLoadよりTime.timeの方がいい気がする。
        float startTime = Time.time;
        while (Time.time - startTime <= i_time)
        {
            float timeStep = (Time.time - startTime) / i_time;
            timeStep = Mathf.Clamp01(timeStep);
            FadeAlpha = Mathf.Lerp(startAlpha, targetAlpha, timeStep);
            yield return null;
        }

        State = i_fade;
    }

} // class FadeController