using DG.Tweening;
using System;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField] private Transform hammerTransform;
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
        onCheckCollision += callback;
    }

    public void RegisterCompleteMovement(Action callback)
    {
        onCompleteMovement += callback;
    }

    public void CheckHole(ICharacterAttributes data)
    {
        isMoving = true;
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(data.HolePosition, 1f))
            .Append(hammerTransform.DOLocalMoveY(0f, 0.5f).OnComplete(() => CheckCollision(data.Character.localPosition)))
            .PrependInterval(0.5f)
            .Append(hammerTransform.DOLocalMoveY(63f, 0.8f)).OnComplete(OnCompleteGoToHole);
            //.Append(transform.DOMove(startPos, 1f).OnComplete(OnCompleteGoToHole));

        seq.Play();
    }

    private void CheckCollision(Vector3 pos)
    {
        if (pos.y > 0.2f)
        {
            //colidiu
            /*tocar particula*/
            Debug.Log(pos);
            onCheckCollision?.Invoke();
        }
        else
        {
            //n colidiu
        }
    }

    private void OnCompleteGoToHole()
    {
        isMoving = false;
        onCompleteMovement?.Invoke();
    }
}
