using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Script.setting
{
    public class LocaleManager : MonoBehaviour
    {
        private bool _isChanging;

        public void ChangeLocale(int index)
        {
            if (_isChanging) return;
            StartCoroutine(ChangeRoutine(index));
        }

        private IEnumerator ChangeRoutine(int index)
        {
            _isChanging = true;
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
            _isChanging = false;
        }

        public void GetLocalization()
        {
            var currentLocale = LocalizationSettings.SelectedLocale;
            var a = LocalizationSettings.StringDatabase.GetLocalizedString("L18nTable", "Customer-boy", currentLocale);
        }
    }
}