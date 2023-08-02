using _Scripts.Helpers;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Scripts.Managers.MoneyManagement
{
    public class MoneyDisplay : MonoBehaviour
    {
        #region Variables
        [SerializeField] private TextMeshProUGUI moneyText;
        
        [Inject] private MoneyWallet _moneyWallet;
        #endregion
        
        #region Monobehaviour Callbacks
        private void Start()
        {
            _moneyWallet.MoneyCountChanged += Display;
            Display();
        }
        #endregion
        
        private void Display()
        {
            moneyText.text = TextFormatter.MoneyText(_moneyWallet.MoneyCount);
        }
    }
}
