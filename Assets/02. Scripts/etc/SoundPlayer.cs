using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip m_Clip;
    [SerializeField] private SoundType m_Type;

    public void PlaySound()
    {
        if(SoundManager.Instance == null)
        {
            // Debug.LogError("SoundManager가 없습니다.");
            return;
        }

        SoundManager.Instance.Play(m_Clip, m_Type);
    }
}
