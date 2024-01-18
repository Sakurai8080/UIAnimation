using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public abstract class TweenBase : MonoBehaviour
{
    [Header("Variable")]
    [Tooltip("遅らせる時間")]
    [SerializeField]
    protected float _delayTime = 2f;

    [Tooltip("所要時間")]
    [SerializeField]
    protected float _requiredTime = 1f;

    [Tooltip("ターゲット値")]
    [SerializeField]
    protected float _targetAmout = 1f;

    [SerializeField]
    protected Image _targetImage = default;

    [SerializeField]
    protected Button _tweensButton = default;

    /// <summary>現在起動しているScaleに関わるTween操作用</summary>
    protected Tween _currentScaleTween = null;
    /// <summary>現在起動しているFadeに関わるTween操作用</summary>
    protected Tween _currentFadeTween = null;

    protected abstract void PlayAnimation();
    protected abstract void UiLoopAnimation();

    protected void Start()
    {
        _tweensButton.OnClickAsObservable()
                     .TakeUntilDestroy(this)
                     .Subscribe(_ =>
                     {
                         ImageAlphaController(_targetImage, 1);
                         KillTweens();
                         SelectedAnimation();
                     });
                     
    }

    //WARNING:引数はミリ秒
    protected async UniTask AnimationDelay(double delayTime)
    {
        await UniTask.Delay(TimeSpan.FromMilliseconds(delayTime));
    }

    protected void ImageAlphaController(Image targetImage,float alphaAmount)
    {
        Color color = targetImage.color;
        color.a = alphaAmount;
        targetImage.color = color;
    }

    protected void KillTweens()
    {
        _currentFadeTween?.Kill();
        _currentScaleTween?.Kill();
    }

    protected void SelectedAnimation()
    {
        _currentScaleTween = transform.DOScale(1, 0.25f)
                                      .SetEase(Ease.OutQuint)
                                      .SetDelay(0.1f)
                                      .OnComplete(() => transform.DOPunchRotation(new Vector3(180f, 270, -45), 2f, 5, 1f));
    }
}