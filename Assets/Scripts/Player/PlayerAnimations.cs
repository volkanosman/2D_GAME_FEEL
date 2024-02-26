using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private ParticleSystem _moveDustVFX;
    [SerializeField] private float _tiltAngle = 20f;
    [SerializeField] private float _tiltSpeed = 5f;
    [SerializeField] private float _cowboyHatTiltModifier = 2f;
    [SerializeField] private Transform _characterSpriteTransform;
    [SerializeField] private Transform _hatSpriteTransform;

    private void Update()
    {
        DetectMoveDust();
        ApplyTilt();
    }

    private void DetectMoveDust()
    {
        if (PlayerController.Instance.CheckGrounded())
        {
            if (PlayerController.Instance.CheckGrounded())
            {
                if (!_moveDustVFX.isPlaying)
                {
                    _moveDustVFX.Play();
                }
            }
            else
            {
                if (_moveDustVFX.isPlaying)
                {
                    _moveDustVFX.Stop();
                }
            }
        }
    }

    private void ApplyTilt()
    {
        float targetAngle;

        if(PlayerController.Instance.MoveInput.x < 0f)
        {
            targetAngle = _tiltAngle;
        }else if(PlayerController.Instance.MoveInput.x > 0f)
        {
            targetAngle = -_tiltAngle;
        }
        else
        {
            targetAngle = 0f;
        }
        //PlayerHat
        Quaternion currentCharacterRotation = _characterSpriteTransform.rotation;
        Quaternion targetCharacterRotation = 
            Quaternion.Euler(currentCharacterRotation.eulerAngles.x, currentCharacterRotation.eulerAngles.y, targetAngle); 

        _characterSpriteTransform.rotation = Quaternion.Lerp(currentCharacterRotation,targetCharacterRotation,_tiltSpeed*Time.deltaTime);

        //CowboyHat
        Quaternion currentHatRotation = _hatSpriteTransform.rotation;
        Quaternion targetHatRotation =
            Quaternion.Euler(currentHatRotation.eulerAngles.x, currentHatRotation.eulerAngles.y, -targetAngle / _cowboyHatTiltModifier);

        _hatSpriteTransform.rotation =
            Quaternion.Lerp(currentHatRotation, targetHatRotation, _tiltSpeed * _cowboyHatTiltModifier * Time.deltaTime);
    }
}
