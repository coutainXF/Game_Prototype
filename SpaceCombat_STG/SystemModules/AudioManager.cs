using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] AudioSource sFXPlayer;
    
    const float MIN_PITCH = .9f; 
    const float MAX_PITCH = 1.1f;
    
    //used for ui SFX
    //适用于播放ui音效
    public void PlaySFX(AudioData audioData)
    {
        //sFXPlayer.clip = audioClip;
        //sFXPlayer.volume = volume;
        // sFXPlayer.Play();该函数无法播放复数的音频，造成重复播放一部分而无法播放完整感觉被掐断。
        sFXPlayer.PlayOneShot(audioData.audioClip,audioData.volume);
    }
    
    //随机改变音高而制作出 相互区别的音频
    //used for repeat-play SFX
    //适用于播放重复播放的音效
    public void PlayRandomSFX(AudioData audioData)
    {
        sFXPlayer.pitch = Random.Range(MIN_PITCH,MAX_PITCH);
        sFXPlayer.PlayOneShot(audioData.audioClip,audioData.volume);
    }
    
    //随机播放某些
    public void PlayRandomSFX(AudioData[] audioData)
    {
        sFXPlayer.pitch = Random.Range(MIN_PITCH,MAX_PITCH);
        PlayRandomSFX(audioData[Random.Range(0,audioData.Length)]);
    }
}
[System.Serializable] public class AudioData
{
    public AudioClip audioClip;
    public float volume;
}