using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

/// <summary>
/// ボタンゲームのUIプレゼンター
/// </summary>
public class TBGameUIPresenter : PresenterBase
{
    [Tooltip("各セレクトボタン")]
    [SerializeField]
    private TBGameSelectBtn[] _button = default;

    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < _button.Length; i++)
        {
            _button[i].SelectedObsever
                      .TakeUntilDestroy(this)
                      .Subscribe(button =>
                      {
                          TBGameManager.Instance.MissButtonChecker(button);
                      });
        }

        _activeSwitchButton.OnClickObserver
                      .Subscribe(_ =>
                      {
                          TBGameManager.Instance.ButtonRandomHide();
                      }).AddTo(this);
    }
}
