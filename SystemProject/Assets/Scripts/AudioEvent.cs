using System;
using Unity.Collections;
using UnityEngine;

[SerializeField]
[CreateAssetMenu]

public class AudioEvent : ScriptableObject
{
    public event Action<string> OnPlay;

    public void Play(string name)
    {
        if (OnPlay != null)
            OnPlay.Invoke(name);
        //Invoke는 이벤트 실행용 함수입니다.
    }



}
