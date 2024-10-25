using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingPanel : MonoBehaviour
{
    private TMP_Text endingText;
    private Image endingBG;
    private GameObject lobbyButton;

    [Header("상수")]
    [SerializeField] private string endText = "   <b>END</b>";
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private float typeTime = 0.2f;

    private void Awake()
    {
        endingText = transform.GetChild(1).GetComponent<TMP_Text>();
        endingBG = transform.GetComponent<Image>();
        lobbyButton = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        endingText.text = "";
        endingBG.color = new Color(0, 0, 0, 0);
        lobbyButton.SetActive(false);

        gameObject.SetActive(false);
    }

    public void Ending(string endingName)
    {
        gameObject.SetActive(true);
        StartCoroutine(EndGame(endingName));
    }

    private IEnumerator EndGame(string endingName)
    {
        yield return StartCoroutine(FadePanel(fadeTime));
        yield return StartCoroutine(TypeText(endingName, typeTime));

        lobbyButton.SetActive(true);
    }

    private IEnumerator FadePanel(float fadeTime)
    {
        TweenerCore<Color, Color, ColorOptions> sequence = endingBG.DOFade(1f, fadeTime);

        yield return sequence.WaitForCompletion();
    }

    private IEnumerator TypeText(string endingName, float typeTime = 0.1f)
    {
        // 대화창을 비운다.
        endingText.text = "";

        // 엔딩 이름을 띄운다.
        for (int i = 0; i < endingName.Length; ++i)
        {
            // 출력할 글자
            string letter = "";
            letter += endingName[i];

            // 만약 html 태그를 만난다면, 한 번에 출력을 위해 letter에 전부 담는다.
            if (letter == "<")
            {
                ++i;
                while (endingName[i] != '>')
                {
                    letter += endingName[i];
                    ++i;
                }
                if (endingName[i] == '>')
                {
                    letter += endingName[i];
                }
            }

            // 한 글자 추가 후 잠시 기다린다.
            endingText.text += letter;
            yield return new WaitForSeconds(typeTime);
        }

        yield return new WaitForSeconds(1f);

        // 마지막 END 글자를 띄운다.
        for (int i = 0; i < endText.Length; ++i)
        {
            // 출력할 글자
            string letter = "";
            letter += endText[i];

            // 만약 html 태그를 만난다면, 한 번에 출력을 위해 letter에 전부 담는다.
            if (letter == "<")
            {
                ++i;
                while (endText[i] != '>')
                {
                    letter += endText[i];
                    ++i;
                }
                if (endText[i] == '>')
                {
                    letter += endText[i];
                }
            }

            // 한 글자 추가 후 잠시 기다린다.
            endingText.text += letter;
            yield return new WaitForSeconds(typeTime);
        }
    }
}
