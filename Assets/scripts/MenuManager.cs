using UnityEngine;
using DFTGames.Localization;

public class MenuManager : MonoBehaviour
{
    #region Public Methods

    public void SetEnglish()
    {
        Localize.SetCurrentLanguage(SystemLanguage.English);
        LocalizeImage.SetCurrentLanguage();
    }

    public void SetGerman()
    {
        Localize.SetCurrentLanguage(SystemLanguage.German);
        LocalizeImage.SetCurrentLanguage();
    }

    public void SetFrench()
    {
        Localize.SetCurrentLanguage(SystemLanguage.French);
        LocalizeImage.SetCurrentLanguage();
    }


    #endregion Public Methods
}
