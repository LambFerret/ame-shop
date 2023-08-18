using System.Collections;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace title
{
    public class LoadingScreen : MonoBehaviour
    {
        public TextMeshProUGUI progressText;
        private Image _blackScreenImage; // Image component of your black screen
        private GameObject _icon;
        public static LoadingScreen Instance { get; private set; }

        private void Awake()
        {
            _blackScreenImage = transform.Find("BlackScreen").GetComponent<Image>();
            _icon = transform.Find("BlackScreen").Find("Icon").gameObject;
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void LoadScene(string sceneName)
        {
            gameObject.SetActive(true);
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            // Start the fade in
            _blackScreenImage.DOFade(1, 0.3f);
            yield return new WaitForSeconds(0.3f);

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f) * 100f;
                string dot = string.Concat(Enumerable.Repeat(".", (int)(progress % 4)));
                progressText.text = "LOADING" + dot + " " + progress + "%";
                yield return null;
            }

            // Start the fade out
            _icon.SetActive(false);
            _blackScreenImage.DOFade(0, 1f);
            yield return new WaitForSeconds(1f);

            gameObject.SetActive(false);
        }
    }
}