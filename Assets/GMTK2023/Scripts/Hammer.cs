using DG.Tweening;
using System;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField] private Transform hammerTransform;
    [SerializeField] private GameObject hitParticle;
    [SerializeField] private GameObject fogParticle;
    [SerializeField] private AudioSource hitAudio;
    public HammerAttributes attributes;
    private Action onCompleteMovement;
    private Action onCheckCollision;
    private Vector2 startPos;

    private bool isMoving;

    public bool IsMoving => isMoving;

    private void Start()
    {
        startPos = transform.position;
    }

    public void RegisterCheckCollision(Action callback)
    {
        onCheckCollision = callback;
    }

    public void RegisterCompleteMovement(Action callback)
    {
        onCompleteMovement += callback;
    }

    public void CheckHole(ICharacterAttributes data)
    {
        if (data.Character.localPosition.y <= -2.3f)
        {
            isMoving = false;
            onCompleteMovement?.Invoke();
            return;
        }

        isMoving = true;
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(data.HolePosition, attributes.toMove))
            .Append(hammerTransform.DOLocalMoveY(5.3f, attributes.toGoDown).OnComplete(() => 
            { 
                CheckCollision(data.Character.localPosition); 
                if (hitAudio.isPlaying) 
                    hitAudio.Stop(); 
                hitAudio.Play(); 
            }))
            .PrependInterval(0.5f)
            .Append(hammerTransform.DOLocalMoveY(63f, attributes.toGoUp)).OnComplete(OnCompleteGoToHole);
            //.Append(transform.DOMove(startPos, 1f).OnComplete(OnCompleteGoToHole));

        seq.Play();
    }

    private void CheckCollision(Vector3 pos)
    {
        if (pos.y > -1.48f)
        {
            if (hitParticle.activeInHierarchy)
                hitParticle.SetActive(false);

            hitParticle.SetActive(true);
            onCheckCollision?.Invoke();
        }
        else
        {
            if (fogParticle.activeInHierarchy)
                fogParticle.SetActive(false);

            fogParticle.SetActive(true);
        }
    }

    private void OnCompleteGoToHole()
    {
        isMoving = false;
        onCompleteMovement?.Invoke();
    }
}

[Serializable]
public class HammerAttributes
{
    public float toMove = 0.4f;
    public float toGoDown = 0.3f;
    public float toGoUp = 0.4f;
}
