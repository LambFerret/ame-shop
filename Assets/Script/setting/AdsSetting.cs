using GoogleMobileAds.Api;
using UnityEngine;

namespace Script.setting
{
    public class AdsSetting : MonoBehaviour
    {
        public string adUnitId;
        public bool isTest;
        private RewardedAd _rewardedAd;

        public void Start()
        {
            // Quiz_Manager = GameObject.FindObjectOfType<Quiz_Manager>();
            // PlayScene_Manager = GameObject.FindObjectOfType<PlayScene_Manager>();
            if (isTest)
                adUnitId = "ca-app-pub-3940256099942544/1033173712";
            else
                adUnitId = "";

            // 모바일 광고 SDK를 초기화함.
            MobileAds.Initialize(initStatus => { });

            //adUnitId 설정
#if UNITY_ANDROID
            adUnitId = "ca-app-pub-3115045377477281/4539879882";
#endif
            LoadRewardedAd();
        }

        public void LoadRewardedAd() //광고 로드 하기
        {
            // Clean up the old ad before loading a new one.
            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }

            Debug.Log("Loading the rewarded ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest.Builder().Build();

            // send the request to load the ad.
            RewardedAd.Load(adUnitId, adRequest,
                (ad, error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                        return;
                    }

                    Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());
                    _rewardedAd = ad;
                });
        }

        public void ShowAd() //광고 보기
        {
            const string rewardMsg =
                "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

            if (_rewardedAd != null && _rewardedAd.CanShowAd())
                _rewardedAd.Show(reward =>
                {
                    //보상 획득하기
                    Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));
                });
            else
                LoadRewardedAd();
        }

        private void RegisterReloadHandler(RewardedAd ad) //광고 재로드
        {
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += null;
            {
                Debug.Log("Rewarded Ad full screen content closed.");

                // Reload the ad so that we can show another as soon as possible.
                LoadRewardedAd();
            }
            ;
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += error =>
            {
                Debug.LogError("Rewarded ad failed to open full screen content with error : " + error);
                LoadRewardedAd();
            };
        }
    }
}