using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace setting
{
    public class SoundManager : MonoBehaviour
    {
        public enum SFX
        {
            BillIn,
            BillOut,
            ChangeCashier,
            Click,
            CloseStore,
            Cooling,
            DoorBell,
            Entrance,
            Fuel,
            Money,
            Package1,
            Package2,
            PackageRing,
            Pool,
            Sugar,
            TimeIsRunningOut,
            UIClick,
            Water,
            PanelClose,
            Fruit
        }

        public static SoundManager Instance;

        public AudioClip[] bgm, sfx, fruit;
        private AudioSource _bgmSource, _sfxSource, _fruitSource;

        public float bgmVolume = 1.0f;
        public float sfxVolume = 1.0f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _bgmSource = gameObject.AddComponent<AudioSource>();
            _bgmSource.loop = true;
            _sfxSource = gameObject.AddComponent<AudioSource>();
            _fruitSource = gameObject.AddComponent<AudioSource>();

            SceneManager.sceneLoaded += OnSceneLoaded;

            PlayMusic("bgm");
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            switch (scene.name)
            {
                case "TitleScene":
                    PlayMusic("Title");
                    break;
                case "NewsScene":
                    PlayMusic("News");
                    break;
                case "ResultScene":
                    PlayMusic("Result");
                    break;
                case "CashierScene":
                    PlayMusic("CashierDay");
                    break;
            }
        }

        public void PlayMusic(string bgmName)
        {
            Debug.Log(bgmName + " is playing with this volume " + bgmVolume);
            foreach (var clip in bgm)
            {
                if (clip.name == bgmName)
                {
                    _bgmSource.Stop();
                    _bgmSource.clip = clip;
                    _bgmSource.volume = bgmVolume;
                    _bgmSource.Play();
                }
            }
        }

        public void PlaySFX(SFX sfxEnum, float? volume = null)
        {
            string sfxName = sfxEnum.ToString();
            foreach (var clip in sfx)
            {
                if (clip.name == sfxName)
                {
                    _sfxSource.PlayOneShot(clip, volume ?? sfxVolume);
                    break;
                }
            }
        }

        public void FruitSound()
        {
            _fruitSource.PlayOneShot(fruit[UnityEngine.Random.Range(0, fruit.Length)], sfxVolume);
        }

        public void MusicVolume(float volume)
        {
            bgmVolume = volume;
            _bgmSource.volume = bgmVolume;
        }

        public void SFXVolume(float volume)
        {
            sfxVolume = volume;
            _sfxSource.volume = sfxVolume;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlaySFX(SFX.Click, 0.1F);
            }
        }
    }
}