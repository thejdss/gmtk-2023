using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] private int maxMoves;
    [SerializeField] private MoveConfiguration moveConfiguration;
    [SerializeField] private CharacterPack[] characters;

    private int currentMove;

    private void Start()
    {
        foreach (var item in characters)
        {
            item.character.SetKey(item.key);
            item.character.RegisterEvents(MoveDown, MoveUp);
            item.character.RegisterEvents((x) => DecreaseCurrentMove(), (x) => IncreaseCurrentMove());
        }
    }

    private void MoveUp(ICharacterAttributes item)
    {
        if (currentMove >= maxMoves)
            return;

        item.Character.DOKill(false);
        item.Character.DOLocalMoveY(moveConfiguration.maxUpPosition, moveConfiguration.maxUpTime, false).SetAutoKill(false).OnComplete(() => HoldOnTop(item));
    }

    private void MoveDown(ICharacterAttributes item)
    {
        if (item.Hold)
            return;

        item.Character.DOKill(false);
        item.Character.DOLocalMoveY(moveConfiguration.maxDownPositions, moveConfiguration.maxDownTime, false).SetAutoKill(false);
    }

    private void DecreaseCurrentMove()
    {
        currentMove -= 1;
    }

    private void IncreaseCurrentMove()
    {
        currentMove += 1;
    }

    private void HoldOnTop(ICharacterAttributes item)
    {
        StartCoroutine(IHoldOnTop(item));
    }

    private IEnumerator IHoldOnTop(ICharacterAttributes item)
    {
        item.Hold = true;
        IncreaseCurrentMove();
        yield return new WaitForSeconds(moveConfiguration.holdTime);
        item.Hold = false;
        DecreaseCurrentMove();

        MoveDown(item);
    }
}

[Serializable]
public class CharacterPack
{
    public KeyCode key;
    public MovementExposer character;
}

[Serializable]
public struct MoveConfiguration
{
    public float maxUpPosition, maxUpTime;
    public float maxDownPositions, maxDownTime;
    public float holdTime;
}
