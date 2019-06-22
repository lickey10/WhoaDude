using System;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

// Example script showing how to invoke the Google Mobile Ads Unity plugin.
public class GoogleMobileAdsDemoScript : MonoBehaviour
{

    private BannerView bannerView;
    private InterstitialAd interstitial;

	void Start()
	{
		string message = "";

		try {
			RequestBanner();
			bannerView.Show();
				} catch (Exception ex) {
			message = ex.Message;
				}

	}

    private void RequestBanner()
    {
        #if UNITY_EDITOR
            string adUnitIdTop = "unused";
			string adUnitIdBottom = "unused";
        #elif UNITY_ANDROID
		string adUnitIdTop = "ca-app-pub-2335297199953104/7079631471";
		string adUnitIdBottom = "ca-app-pub-2335297199953104/9636145072";
        #elif UNITY_IPHONE
			string adUnitId = "";
        #else 
			//windows?
		string adUnitIdTop = "ca-app-pub-2335297199953104/7079631471";
		string adUnitIdBottom = "ca-app-pub-2335297199953104/9636145072";
        #endif

        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitIdTop, AdSize.SmartBanner, AdPosition.Top);
        // Register for ad events.
        bannerView.AdLoaded += HandleAdLoaded;
        bannerView.AdFailedToLoad += HandleAdFailedToLoad;
        bannerView.AdOpened += HandleAdOpened;
        bannerView.AdClosing += HandleAdClosing;
        bannerView.AdClosed += HandleAdClosed;
        bannerView.AdLeftApplication += HandleAdLeftApplication;
        // Load a banner ad.
        bannerView.LoadAd(createAdRequest());

		// Create a 320x50 banner at the bottom of the screen.
		bannerView = new BannerView(adUnitIdBottom, AdSize.SmartBanner, AdPosition.Bottom);
		// Register for ad events.
		bannerView.AdLoaded += HandleAdLoaded;
		bannerView.AdFailedToLoad += HandleAdFailedToLoad;
		bannerView.AdOpened += HandleAdOpened;
		bannerView.AdClosing += HandleAdClosing;
		bannerView.AdClosed += HandleAdClosed;
		bannerView.AdLeftApplication += HandleAdLeftApplication;
		// Load a banner ad.
		bannerView.LoadAd(createAdRequest());
	}
	
	private void RequestInterstitial()
	{
		#if UNITY_EDITOR
            string adUnitId = "unused";
        #elif UNITY_ANDROID
		string adUnitId = "ca-app-pub-2335297199953104/9112653475";
        #elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-1463248652494374/1807902444";
        #else
		//windows?
		string adUnitId = "ca-app-pub-1463248652494374/3284635648";
        #endif

        // Create an interstitial.
        interstitial = new InterstitialAd(adUnitId);
        // Register for ad events.
        interstitial.AdLoaded += HandleInterstitialLoaded;
        interstitial.AdFailedToLoad += HandleInterstitialFailedToLoad;
        interstitial.AdOpened += HandleInterstitialOpened;
        interstitial.AdClosing += HandleInterstitialClosing;
        interstitial.AdClosed += HandleInterstitialClosed;
        interstitial.AdLeftApplication += HandleInterstitialLeftApplication;
        // Load an interstitial ad.
        interstitial.LoadAd(createAdRequest());
    }

    // Returns an ad request with custom ad targeting.
    private AdRequest createAdRequest()
    {
//        return new AdRequest.Builder()
//                .AddTestDevice(AdRequest.TestDeviceSimulator)
//                .AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
//                .AddKeyword("game")
//                .SetGender(Gender.Male)
//                .SetBirthday(new DateTime(1985, 1, 1))
//                .TagForChildDirectedTreatment(true)
//                .AddExtra("color_bg", "9B30FF")
//                .Build();

		return new AdRequest.Builder()
				.AddKeyword("game")
				.TagForChildDirectedTreatment(true)
				.AddExtra("color_bg", "9B30FF")
				.Build();
    }

    private void ShowInterstitial()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
        else
        {
            print("Interstitial is not ready yet.");
        }
    }

    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        print("HandleAdLoaded event received.");
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        print("HandleAdOpened event received");
    }

    void HandleAdClosing(object sender, EventArgs args)
    {
        print("HandleAdClosing event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        print("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        print("HandleAdLeftApplication event received");
    }

    #endregion

    #region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        print("HandleInterstitialLoaded event received.");
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleInterstitialFailedToLoad event received with message: " + args.Message);
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        print("HandleInterstitialOpened event received");
    }

    void HandleInterstitialClosing(object sender, EventArgs args)
    {
        print("HandleInterstitialClosing event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        print("HandleInterstitialClosed event received");
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        print("HandleInterstitialLeftApplication event received");
    }

    #endregion
}