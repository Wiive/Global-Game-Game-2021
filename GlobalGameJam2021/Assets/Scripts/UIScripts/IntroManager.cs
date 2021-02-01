using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    public GameObject[] banners;
    [SerializeField] int currentBanner = 0;
    int maxBanners = 6;

    [SerializeField] GameObject playButton;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject skipButton;

    [SerializeField] EventSystem eventSystem;

    public void ActivateBanner()
    {      
        if (currentBanner < maxBanners)
        {
            banners[currentBanner].SetActive(true);
            currentBanner++;
        }
        else
        {
            playButton.SetActive(true);
            eventSystem.SetSelectedGameObject(playButton);
            nextButton.SetActive(false);
            skipButton.SetActive(false);
        }
    }
}
