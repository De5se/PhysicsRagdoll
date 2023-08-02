using DG.Tweening;
using UnityEngine;

namespace _Scripts.UI
{
    public class WindowsManager : MonoBehaviour
    {
        #region Variables
        private const float SwapDuration = 0.25f;
        #endregion
        
        public static void CanvasGroupSwap(CanvasGroup canvasGroup, bool isEnabled)
        {
            canvasGroup.DOFade(isEnabled? 1 : 0, SwapDuration);

            canvasGroup.interactable = isEnabled;
            canvasGroup.blocksRaycasts = isEnabled;
        }
    }
}