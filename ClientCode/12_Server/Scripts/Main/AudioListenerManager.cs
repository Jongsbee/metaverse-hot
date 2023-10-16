using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioListenerManager : MonoBehaviour
{

    public Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        // 초기 슬라이더 값과 오디오 리스너 볼륨 값 동기화
        volumeSlider.value = AudioListener.volume;

        // 슬라이더 값이 변경되었을 때 이벤트 리스너 등록
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeVolume(float value)
    {
        AudioListener.volume = value;
    }
}
