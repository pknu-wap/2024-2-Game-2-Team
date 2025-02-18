using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResolutionManager : MonoBehaviour
{
    // 싱글톤
    public static ResolutionManager Instance { get; private set; }

    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown frameRateDropdown;
    public Toggle fullscreenToggle;
    public Button applyButton;
    public Button cancelButton;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    private List<int> frameRates = new List<int> { 30, 60, 120, 144, 240 };
    private bool fullscreen = true;
    private int selectedResolutionIndex;
    private int selectedFrameRateIndex;

    private bool previousFullscreen;
    private int previousResolutionIndex;
    private int previousFrameRateIndex;

    void Awake()
    {
        Instance = this;

        // 해상도 리스트 가져오기
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();
        frameRateDropdown.ClearOptions();

        // 중복 제거
        HashSet<string> addedResolutions = new HashSet<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (!addedResolutions.Contains(resolutions[i].width + "x" + resolutions[i].height))
            {
                filteredResolutions.Add(resolutions[i]);
                addedResolutions.Add(resolutions[i].width + "x" + resolutions[i].height);
            }
        }

        // 해상도를 큰 것부터 작은 것 순서로 정렬
        filteredResolutions.Sort((res1, res2) => (res2.width * res2.height).CompareTo(res1.width * res1.height));
        resolutionDropdown.ClearOptions();

        // Dropdown에 해상도 옵션 추가하기
        List<string> resolutionOptions = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string option = filteredResolutions[i].width + " x " + filteredResolutions[i].height;
            resolutionOptions.Add(option);
        }
        resolutionDropdown.AddOptions(resolutionOptions);

        // Dropdown에 프레임 레이트 옵션 추가
        List<string> frameRateOptions = new List<string>();
        for (int i = 0; i < frameRates.Count; i++)
        {
            frameRateOptions.Add(frameRates[i] + " FPS");
        }
        frameRateDropdown.AddOptions(frameRateOptions);


        // 현재 해상도 선택
        selectedResolutionIndex = FindCurrentResolutionIndex();
        resolutionDropdown.value = selectedResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // 현재 프레임 레이트 선택
        selectedFrameRateIndex = FindCurrentFrameRateIndex();
        frameRateDropdown.value = selectedFrameRateIndex;
        frameRateDropdown.RefreshShownValue();

        // 토글 상태 설정
        fullscreenToggle.isOn = Screen.fullScreen;

        // 초기 설정값 저장
        previousResolutionIndex = selectedResolutionIndex;
        previousFrameRateIndex = selectedFrameRateIndex;
        previousFullscreen = Screen.fullScreen;

        // 버튼 클릭 시 적용
        applyButton.onClick.AddListener(ApplyResolutionSettings);
        applyButton.onClick.AddListener(ApplyFrameRateChange);

        // 취소 버튼 클릭 시 이전 설정으로 복원
        cancelButton.onClick.AddListener(CancelResolutionSettings);
        cancelButton.onClick.AddListener(CancelFrameRateChange);

        // Dropdown 값이 변경될 때 미리 해상도 적용
        resolutionDropdown.onValueChanged.AddListener(delegate { PreviewResolutionChange(); });
    }

    // 현재 해상도의 인덱스를 찾는 메서드
    private int FindCurrentResolutionIndex()
    {
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            if (filteredResolutions[i].width == Screen.currentResolution.width &&
                filteredResolutions[i].height == Screen.currentResolution.height)
            {
                return i;
            }
        }
        return 0;
    }

    // 현재 설정된 프레임 레이트에 가장 가까운 인덱스를 찾는 메서드
    private int FindCurrentFrameRateIndex()
    {
        int currentFrameRate = Application.targetFrameRate;
        for (int i = 0; i < frameRates.Count; i++)
        {
            if (frameRates[i] == currentFrameRate)
            {
                return i;
            }
        }

        // 설정된 프레임 레이트가 없을 경우 기본값 60 FPS 선택
        return frameRates.IndexOf(60);
    }

    // 설정 적용
    public void ApplyResolutionSettings()
    {
        selectedResolutionIndex = resolutionDropdown.value;
        Resolution resolution = filteredResolutions[selectedResolutionIndex];
        fullscreen = fullscreenToggle.isOn;

        // 해상도와 전체화면 설정 적용
        Screen.SetResolution(resolution.width, resolution.height, fullscreen);

        // 새로운 설정값을 이전 설정으로 저장
        previousResolutionIndex = selectedResolutionIndex;
        previousFullscreen = fullscreen;

        // 설정한 옵션을 로컬 파일로 저장
        SaveResolutionSettings();
        DataManager.Instance.SaveData();
    }

    // 프레임 레이트 설정 적용
    public void ApplyFrameRateChange()
    {
        selectedFrameRateIndex = frameRateDropdown.value;
        int targetFrameRate = frameRates[selectedFrameRateIndex];

        previousFrameRateIndex = selectedFrameRateIndex;

        // 프레임 레이트 제한 설정
        Application.targetFrameRate = targetFrameRate;
    }

    // 설정 취소
    public void CancelResolutionSettings()
    {
        // 이전 설정값으로 복원
        resolutionDropdown.value = previousResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        fullscreenToggle.isOn = previousFullscreen;

        // 해상도와 전체화면 설정 복원
        Resolution resolution = filteredResolutions[previousResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, previousFullscreen);
    }

    // 프레임 레이트 변경 취소
    public void CancelFrameRateChange()
    {
        // 이전 설정값으로 복원
        frameRateDropdown.value = previousFrameRateIndex;
        frameRateDropdown.RefreshShownValue();

        // 프레임 레이트를 이전 설정값으로 복원
        int targetFrameRate = frameRates[previousFrameRateIndex];
        Application.targetFrameRate = targetFrameRate;
    }

    public void SaveResolutionSettings()
    {
        // 현재 해상도, 전체화면 설정을 데이터에 저장
        DataManager.Instance.data.SelectedResolutionIndex = selectedResolutionIndex;
        DataManager.Instance.data.Fullscreen = fullscreen;
    }

    public void LoadResolutionSettings()
    {
        // 데이터에서 저장된 해상도, 전체화면 설정을 불러옴
        selectedResolutionIndex = DataManager.Instance.data.SelectedResolutionIndex;
        fullscreen = DataManager.Instance.data.Fullscreen;
            
        // 로드한 설정 적용
        Resolution resolution = filteredResolutions[selectedResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, fullscreen);

        // 드롭다운, 토글 업데이트
        resolutionDropdown.value = selectedResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        fullscreenToggle.isOn = fullscreen;
    }

    // Dropdown 값이 변경될 때 미리 해상도 적용
    private void PreviewResolutionChange()
    {
        int tempResolutionIndex = resolutionDropdown.value;
        Resolution resolution = filteredResolutions[tempResolutionIndex];
        bool tempFullscreen = fullscreenToggle.isOn;

        // 해상도와 전체화면 설정 미리 적용
        Screen.SetResolution(resolution.width, resolution.height, tempFullscreen);
    }
}