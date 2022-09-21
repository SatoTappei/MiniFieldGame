using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

/// <summary>
/// ゲーム中のサウンドを管理する
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;

    /// <summary>再生するサウンドのデータ</summary>
    [Serializable]
    public struct SoundData
    {
        public string key;
        public AudioClip clip;
        float lastTime;
        public float volume;

        public float LastTime { get => lastTime; set => lastTime = value; }
    }

    /// <summary>再生するサウンドのデータをインスペクターから設定する</summary>
    [SerializeField] SoundData[] _soundDatas;
    /// <summary>同じ音を連続して鳴らせるまでの間隔</summary>
    [SerializeField] float _distance;
    /// <summary>複数同時に音を再生できるようにたくさんつけておく</summary>
    AudioSource[] _audioSources = new AudioSource[15];
    Dictionary<string, SoundData> _dic = new Dictionary<string, SoundData>();
    

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // 文字列でSoundDataを指定できるようにする
        _soundDatas.ToList().ForEach(s => _dic.Add(s.key, s));
        // AudioSourceをたくさん付ける
        for (int i = 0; i < _audioSources.Length; i++)
            _audioSources[i] = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>Keyを指定して音を再生</summary>
    public void Play(string key)
    {
        if (_dic.TryGetValue(key, out SoundData data))
        {
            if (Time.realtimeSinceStartup - data.LastTime < _distance) return;
            data.LastTime = Time.realtimeSinceStartup;

            if (key.IndexOf("BGM_") >= 0)
                PlayBGM(data);
            else
                PlaySE(data);
        }
        else
        {
            Debug.LogWarning("サウンドが登録されていません:" + key);
        }
    }

    /// <summary>SEを再生する</summary>
    void PlaySE(SoundData data)
    {
        AudioSource source = GetAudioSource();
        if (source == null)
        {
            Debug.LogWarning("音を鳴らせませんでした。AudioSourceが足りません");
            return;
        }
        source.clip = data.clip;
        source.volume = data.volume;
        source.Play();
    }

    /// <summary>BGMを再生する</summary>
    void PlayBGM(SoundData data)
    {
        AudioSource source = _audioSources[_audioSources.Length - 1];
        source.clip = data.clip;
        source.volume = data.volume;
        source.loop = true;
        source.Play();
    }

    /// <summary>再生中のBGMを止める</summary>
    public void StopBGM() => _audioSources[_audioSources.Length - 1].Stop();

    /// <summary>BGMをフェードアウトさせる</summary>
    public void FadeOutBGM(float duration = 0.5f) => _audioSources[_audioSources.Length - 1].DOFade(0, duration);

    /// <summary>SEを鳴らすためのAudioSourceを取得</summary>
    AudioSource GetAudioSource()
    {
        // 一番後ろのAudioSourceはBGM再生用に取っておく
        for (int i = 0; i < _audioSources.Length - 1; i++)
            if (!_audioSources[i].isPlaying)
                return _audioSources[i];

        return null;
    }
}
