using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.IO;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;


public class DialogueManager : MonoBehaviour, IPointerDownHandler
{

    public TMP_Text dialogueText;        //Story Text
    public TMP_Text dialogueName;        //Story Name
    public TMP_Text ChoiceUpText;        //Up Selection Text
    public TMP_Text ChoiceDownText;      //Down Selection Text

    public GameObject dialogueBox;       //전체 Canvas

    public GameObject Dialogue;          //대화창

    public GameObject ChoiceUp;          //위 선택지 표시
    public GameObject ChoiceDown;         //아래 선택지 표시

    public GameObject WaitCursor;        //다음 Text 대기 표시 커서
      
    public Image dialogueImage;          //일러스트
          
    public Sprite[] dialogueImages;      //일러스트 목록
      
    public string[] StoryText;           //Story Text 배열
    public string[] StoryName;           //Story Name 배열
      
    public int currentLine;              //현재 출력 중인 문자열 위치
      
    private bool isTyping = false;       //타이핑 효과 진행 여부 확인 변수
    private bool cancelTyping = false;   //

    private bool isSelection = false;    //선택지 진행 여부 확인 변수
    private bool isEnd = false;          //대화 종료 여부 확인 변수

    void Start()
    {
        dialogueBox.SetActive(false);    //시작 시 Canvas 전체 비활성화
        ChoiceUp.SetActive(false);      //시작 시 선택지 비활성화
        ChoiceDown.SetActive(false);    
        LoadDialogue();                  //Story Name,Text 불러오기
        ShowDialogue();                  //이미지와 전체 Canvas 표시
    }

    void Update()
    {
        if(!isEnd)
        {
            if(ChoiceUp.GetComponent<Choice>().isClicked){
                    Dialogue.SetActive(true);
                    ChoiceUp.SetActive(false);
                    ChoiceDown.SetActive(false);
                    StartCoroutine(TypeSentence("무사히 좀비를 피해 도망갔다."));
                    isEnd = true;
                }
            if(ChoiceDown.GetComponent<Choice>().isClicked){
                Dialogue.SetActive(true);
                ChoiceUp.SetActive(false);
                ChoiceDown.SetActive(false);
                StartCoroutine(TypeSentence("좀비와 맞서 싸우게 된다."));
                isEnd = true;
                }
        }
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       NextDialogue();
    }

    public void NextDialogue(){
        var IllustTable = new Dictionary<string,int>()
        {
            {"???",0},
            {"좀비",1}   
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
            isSelection = true;
            dialogueText.text = "....";
            dialogueName.text = "";
            Dialogue.SetActive(false);
            ChoiceUp.SetActive(true);
            ChoiceDown.SetActive(true);
            ChoiceUpText.text = StoryName[currentLine].Substring(1);
            ChoiceDownText.text = StoryText[currentLine].Substring(1);
        }
        else
        {
            // 이미지 변경
            dialogueImage.sprite = dialogueImages[IllustTable[dialogueName.text]];
            StartCoroutine(TypeSentence(StoryText[currentLine]));
        }

    }

    //Story Text에 타이핑 효과 추가하는 함수
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
        float textWidth = dialogueText.preferredWidth;
        Vector3 newPosition = dialogueText.transform.position + new Vector3(textWidth,0,0);
        WaitCursor.transform.position = newPosition;
        WaitCursor.SetActive(true);
        cancelTyping = false;
    }

    //초기 화면 표시
    public void ShowDialogue()
    {
        currentLine = -1;
        dialogueBox.SetActive(true);
        // 이미지 초기화
        dialogueImage.sprite = dialogueImages[0];
    }

    //TXT 파일에서 Story Text, Name 불러오는 함수
    void LoadDialogue()
    {
        //파일 저장 경로
        string FilePath = "Assets/Resources/StoryScript.txt";

        StreamReader reader = new StreamReader(FilePath);

        string fileContent = reader.ReadToEnd();

        StoryText = fileContent.Split('\n');

        StoryName = new string[StoryText.Length];

        for (int i = 0; i < StoryText.Length; i++)
        {
            string[] Temp = StoryText[i].Split('#');
            StoryText[i] = Temp[1]; // 대화 문장 저장
            StoryName[i] = Temp[0]; // 대화 이름 저장
        }

        reader.Close();
    }
}