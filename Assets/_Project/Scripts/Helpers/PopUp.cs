using System.Collections;
using _Scripts.UI;
using TMPro;
using UnityEngine;

namespace _Scripts.Helpers
{
    public class PopUp : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI popUpText;
        [SerializeField] private float popUpDuration = 2f;

        private Coroutine _popUpCoroutine;

        private void Start()
        {
            canvasGroup.alpha = 0f;
        }

        public void ShowPopUp(string text)
        {
            popUpText.text = text;
            WindowsManager.CanvasGroupSwap(canvasGroup, true);

            if (_popUpCoroutine != null)
                StopCoroutine(_popUpCoroutine);
            
            _popUpCoroutine = StartCoroutine(HidePopUp());
        }
    
        private IEnumerator HidePopUp()
        {
            yield return new WaitForSeconds(popUpDuration);
            WindowsManager.CanvasGroupSwap(canvasGroup, false);
        }
    }
}