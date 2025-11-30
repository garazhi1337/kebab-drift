using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _crashWet;
    [SerializeField] private AudioSource _crashHard;
    [SerializeField] private AudioSource _brake;
    [SerializeField] private AudioSource _motor;
    [SerializeField] private AudioSource _wrongMotor;
    [SerializeField] private AudioSource _peredachSwitch;
    
    public enum Sounds
    {
        CRASH_WET,
        CRASH_HARD,
        BRAKE,
        MOTOR,
        WRONG_MOTOR,
        PEREDACH_SWITCH
    }

    public void PlaySound(Sounds sound)
    {
        switch (sound)
        {
            case Sounds.CRASH_WET:
                if (!_crashWet.isPlaying) _crashWet.Play();
                break;
            case Sounds.CRASH_HARD:
                if (!_crashHard.isPlaying) _crashHard.Play();
                break;
            case Sounds.BRAKE:
                if (!_brake.isPlaying) _brake.Play();
                break;
            case Sounds.MOTOR:
                if (!_motor.isPlaying) _motor.Play();
                break;
            case Sounds.WRONG_MOTOR:
                if (!_wrongMotor.isPlaying) _wrongMotor.Play();
                break;
            case Sounds.PEREDACH_SWITCH:
                if (!_peredachSwitch.isPlaying) _peredachSwitch.Play();
                break;
        }
    }

    public void AdjustMotorSound(float intensity, float pitch)
    {
        _motor.volume = 0.3f + intensity;
        _motor.pitch = 0.9f + pitch;
    }

    public void AdjustBrakeSound(float intensity, float pitch)
    {
        _brake.volume = intensity;
    }
}
