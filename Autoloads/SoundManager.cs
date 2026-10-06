using Godot;
using Godot.Collections;
using System;

public partial class SoundManager : Node
{
    public Dictionary<int,AudioStream> soundDic = new()
    {
        {(int)E_Sound.Button, GD.Load<AudioStream>("res://Audio/Sounds/Menu/Move2.wav")},
        {(int)E_Sound.Import, GD.Load<AudioStream>("res://Audio/Sounds/Hit & Impact/Impact2.wav")},
        {(int)E_Sound.SkillBoost, GD.Load<AudioStream>("res://Audio/Sounds/Magic & Skill/Heal2.wav")},

    };
    [Export]
    public Array<AudioStreamPlayer> streamPlayers;

    public void Play(int type)
    {
        AudioStreamPlayer streamPlayer = GetFreeStreamPlayer();
        if(streamPlayer == null)
        {
            return;
        }
        AudioStream audio = soundDic[type];
        streamPlayer.Stream = audio;
        streamPlayer.PitchScale = (float)GD.RandRange(0.8, 1.2);
        streamPlayer.Play();
    }

    public AudioStreamPlayer GetFreeStreamPlayer()
    {
        foreach(AudioStreamPlayer stream in streamPlayers)
        {
            if (!stream.Playing)
            {
                return stream;
            }
        }
        return null;
    }

        // 单例实例
    private static SoundManager _instance;
    public static SoundManager Instance => _instance;
    public override void _EnterTree()
    {
        if (_instance == null)
        {
            _instance = this;  // ✅ 这里给 _instance 赋值！
        }
        else
        {
            QueueFree();  // 防止重复实例
        }
    }
}
