using FlowerShop.Tables;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowerShop.Ads
{
    public class LevelPlayAds : MonoBehaviour
    {
        [Inject] private readonly FertilizersTable fertilizersTable;
        [SerializeField] private Image adsImage;
        
        private void Start()
        {
            IronSource.Agent.init("1e0f975c5");
            IronSource.Agent.validateIntegration();
        }

        private void OnEnable()
        {
            IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
            
            IronSource.Agent.shouldTrackNetworkState(true);
            
            IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
            IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
            IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
            IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
            IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
            IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
            IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;
        }

        private void OnApplicationPause(bool isPaused) {                 
            IronSource.Agent.onApplicationPause(isPaused);
        }

        public void ShowRewardedAd()
        {
            if (IronSource.Agent.isRewardedVideoAvailable())
            {
                IronSource.Agent.showRewardedVideo();
            }
            else
            {
                Debug.Log("Rewarded Video Not Available");
            }
        }

        private void SdkInitializationCompletedEvent()
        {
            adsImage.enabled = true;
        }
        
        /************* RewardedVideo AdInfo Delegates *************/ 
        // Indicates that there’s an available ad.
        // The adInfo object includes information about the ad that was loaded successfully
        // This replaces the RewardedVideoAvailabilityChangedEvent(true) event
        void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo) 
        {
            
        } 
        
        // Indicates that no ads are available to be displayed
        // This replaces the RewardedVideoAvailabilityChangedEvent(false) event
        void RewardedVideoOnAdUnavailable()
        {
            
        } 
        
        // The Rewarded Video ad view has opened. Your activity will loose focus.
        void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
        {
            
        } 
        
        // The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
        void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
        {
            
        } 
        
        // The user completed to watch the video, and should be rewarded.
        // The placement parameter will include the reward data.
        // When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
        void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
        {
            fertilizersTable.IncreaseAvailableFertilizersUsesNumber();
        }
        
        // The rewarded video ad was failed to show.
        void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
        {
            
        } 
        
        // Invoked when the video ad was clicked.
        // This callback is not supported by all networks, and we recommend using it only if
        // it’s supported by all networks you included in your build.
        void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
        {
            
        }
    }
}
