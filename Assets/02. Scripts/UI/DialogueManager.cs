using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.IO;
using TMPro;
using System.Collections.Generic;


public class DialogueManager : MonoBehaviour, IPointerDownHandler
{

    public TMP_Text dialogueText;        //Story Text
    public TMP_Text dialogueName;        //Story Name

    public TMP_Text ChoiceUpText;        //Up Selection Text
    public TMP_Text ChoiceDownText;      //Down Selection Text

    public TMP_Text ChoiceUpRequireText;       //Up Selection Text
    public TMP_Text ChoiceDownRequireText;     //Down Selection Text

    public GameObject dialogueBox;       //? μ²? Canvas

    public GameObject Dialogue;          //????μ°?

    public GameObject ChoiceUp;          //? ? ?μ§? ??
    public GameObject ChoiceDown;         //?? ? ?μ§? ??

    public GameObject WaitCursor;        //?€? Text ???κΈ? ?? μ»€μ
      
    public Image dialogueImage;          //?Ό?¬?€?Έ
          
    public Sprite[] dialogueImages;      //?Ό?¬?€?Έ λͺ©λ‘
      
    public string[] StoryText;           //Story Text λ°°μ΄
    public string[] StoryName;           //Story Name λ°°μ΄
      
    public int currentLine;              //??¬ μΆλ ₯ μ€μΈ λ¬Έμ?΄ ?μΉ?
      
    private bool isTyping = false;       //????΄? ?¨κ³? μ§ν ?¬λΆ? ??Έ λ³??
    private bool cancelTyping = false;   //

    void Start()
    {
        dialogueBox.SetActive(false);    //?? ? Canvas ? μ²? λΉν?±?
        ChoiceUp.SetActive(false);      //?? ? ? ?μ§? λΉν?±?
        ChoiceDown.SetActive(false);    
        LoadDialogue();                  //Story Name,Text λΆλ¬?€κΈ?
        ShowDialogue();                  //?΄λ―Έμ????? ? μ²? Canvas ??
    }

    void Update()
    {
   
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       NextDialogue();
    }

    public void NextDialogue(){
        var IllustTable = new Dictionary<string,int>()
        {
            {"???",0},
            {"Α»Ίρ",1}   
        };

        if(isTyping && !cancelTyping)
        {
            cancelTyping = true;
            return;
        }

        currentLine++;

        if (currentLine >= StoryText.Length)
        {
            dialogueBox.SetActive(false);
            return;
        }

        dialogueName.text = StoryName[currentLine];

        if(StoryName[currentLine][0] == '*'){
            Selection();
        }
        else
        {
            // ?΄λ―Έμ?? λ³?κ²?
            dialogueImage.sprite = dialogueImages[IllustTable[dialogueName.text]];
            StartCoroutine(TypeSentence(StoryText[currentLine]));
        }

    }

    //Story Text? ????΄? ?¨κ³? μΆκ???? ?¨?
    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        WaitCursor.SetActive(false);
        cancelTyping = false;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.03f);
            if (cancelTyping)
            {
                dialogueText.text = sentence;
                break;
            }
        }
        isTyping = false;
        WaitCursor.SetActive(true);
        cancelTyping = false;
    }

    void Selection()
    {
        dialogueText.text = "....";
        dialogueName.text = "";
        Dialogue.SetActive(false);
        ChoiceUp.SetActive(true);
        ChoiceDown.SetActive(true);
        ChoiceUpText.text = StoryName[currentLine].Substring(1);
        ChoiceDownText.text = StoryText[currentLine].Substring(1);
        if(Items.items.Find(x => x == "ΓΡ") != null){
        
        ChoiceDownRequireText.text = "ΗΚΏδΗΡ ΎΖΐΜΕΫ : <color=red>ΓΡ</color>";
        
        }
        else{

        ChoiceDownRequireText.text = "ΗΚΏδΗΡ ΎΖΐΜΕΫ : ΓΡ";

        }
    }

    //μ΄κΈ° ?λ©? ??
    public void ShowDialogue()
    {
        currentLine = -1;
        dialogueBox.SetActive(true);
        // ?΄λ―Έμ?? μ΄κΈ°?
        dialogueImage.sprite = dialogueImages[0];
    }

    //TXT ??Ό?? Story Text, Name λΆλ¬?€? ?¨?
    void LoadDialogue()
    {
        //??Ό ????₯ κ²½λ‘
        TextAsset asset = Resources.Load ("StoryScript")as TextAsset;

        string Story = asset.text;

        StringReader reader = new StringReader(Story);

        string fileContent = reader.ReadToEnd();

        StoryText = fileContent.Split('\n');

        StoryName = new string[StoryText.Length];

        for (int i = 0; i < StoryText.Length; i++)
        {
            string[] Temp = StoryText[i].Split('#');
            StoryText[i] = Temp[1]; // ???? λ¬Έμ₯ ????₯
            StoryName[i] = Temp[0]; // ???? ?΄λ¦? ????₯
        }

        reader.Close();
    }
}
