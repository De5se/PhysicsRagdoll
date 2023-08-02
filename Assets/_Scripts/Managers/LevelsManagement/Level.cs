using UnityEngine;

namespace _Scripts.Managers.LevelsManagement
{
    [CreateAssetMenu(fileName ="Level", menuName = "Data/Level")]
    public class Level : ScriptableObject
    {
        #region Variables
        [HideInInspector] public int index;

        #endregion

    }
}