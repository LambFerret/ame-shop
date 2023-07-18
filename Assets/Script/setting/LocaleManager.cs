using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
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
            Locale currentLocale = LocalizationSettings.SelectedLocale;
            var a = LocalizationSettings.StringDatabase.GetLocalizedString("L18nTable", "Customer-boy", currentLocale);
        }
    }
}