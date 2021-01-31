using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public GameObject[] banners;
    [SerializeField] int currentBanner = 0;
    int maxBanners = 6;

    [SerializeField] GameObject playButton;
    [SerializeField] GameObject nextButton;

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
            nextButton.SetActive(false);
        }
    }
}
