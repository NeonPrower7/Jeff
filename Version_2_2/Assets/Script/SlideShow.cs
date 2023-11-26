using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SlideShow : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private GameObject text;
    public List<Sprite> imagesList;
    public float fadeTime;
    public float longPause;
    private int imageNum = 0;
    private float fadeColor;
    private bool _isFading;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (_isFading) return;

            text.SetActive(false);
            if(imageNum == imagesList.Count) StartCoroutine(ExitStage());
            else
            {
                StartCoroutine(Change(imagesList[imageNum]));
                imageNum++;
            }
        }
    }

    private IEnumerator Change(Sprite newImage)
    {
        _isFading = true;
        while (fadeColor > 0)
        {
            image.color = new Color(fadeColor, fadeColor, fadeColor);
            fadeColor -= 0.1f;
            yield return new WaitForSeconds(fadeTime);
        }
        image.sprite = newImage;
        while (fadeColor < 1)
        {
            image.color = new Color(fadeColor, fadeColor, fadeColor);
            fadeColor += 0.1f;
            yield return new WaitForSeconds(fadeTime);
        }
        _isFading = false;
    }

    private IEnumerator ExitStage()
    {
        _isFading = true;
        while (fadeColor > 0)
        {
            image.color = new Color(fadeColor, fadeColor, fadeColor);
            fadeColor -= 0.1f;
            yield return new WaitForSeconds(fadeTime);
        }
        yield return new WaitForSeconds(longPause);
        SceneManager.LoadScene(1);
    }
}
