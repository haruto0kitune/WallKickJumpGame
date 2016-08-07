using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    GameObject leftButton;
    WKJGButton leftButtonWKJGButton;
    [SerializeField]
    GameObject rightButton;
    WKJGButton rightButtonWKJGButton;
    [SerializeField]
    GameObject jumpButton;
    WKJGButton jumpButtonWKJGButton;
    [SerializeField]
    GameObject hookShotButton;
    WKJGButton hookShotButtonWKJGButton;

    public ReactiveProperty<bool> isLeftButtonDown;
    public ReactiveProperty<bool> isRightButtonDown;
    public ReactiveProperty<bool> isJumpButtonDown;
    public ReactiveProperty<bool> isHookShotButtonDown;

    void Awake()
    {
        leftButtonWKJGButton = leftButton.GetComponent<WKJGButton>();
        rightButtonWKJGButton = rightButton.GetComponent<WKJGButton>();
        jumpButtonWKJGButton = jumpButton.GetComponent<WKJGButton>();
        hookShotButtonWKJGButton = hookShotButton.GetComponent<WKJGButton>();
    }

    void Start()
    {
        isLeftButtonDown = leftButtonWKJGButton.isButtonDown.ToReactiveProperty();
        isRightButtonDown = rightButtonWKJGButton.isButtonDown.ToReactiveProperty();
        isJumpButtonDown = jumpButtonWKJGButton.isButtonDown.ToReactiveProperty();
        isHookShotButtonDown = hookShotButtonWKJGButton.isButtonDown.ToReactiveProperty();
    }
}
