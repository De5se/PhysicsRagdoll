using System;
using UnityEngine;
using UnityEngine.UI;

public class ResetSceneButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private float resetDuration;

    public event Action OnClick;

    public float ResetDuration => resetDuration;

    private void Start()
    {
        button.onClick.AddListener(ResetScene);
    }


    private void ResetScene()
    {
        OnClick?.Invoke();
    }
}
