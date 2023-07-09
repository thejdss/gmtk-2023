using UnityEngine;
using System;

public class MovementExposer : MonoBehaviour, ICharacterAttributes
{
    [SerializeField] private GameObject character;

    private KeyCode key;
    private bool hold;

    private Action<ICharacterAttributes> onClickDown;
    private Action<ICharacterAttributes> onClickUp;

    public bool Hold { get => hold; set => hold = value; }
    public Transform Character { get => character.transform; }

    public void SetKey(KeyCode key)
    {
        this.key = key;
    }

    public void RegisterEvents(Action<ICharacterAttributes> onClickUp, Action<ICharacterAttributes> onClickDown)
    {
        this.onClickUp += onClickUp;
        this.onClickDown += onClickDown;
    }

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            onClickDown?.Invoke(this);
        }
        else if (Input.GetKeyUp(key))
        {
            onClickUp?.Invoke(this);
        }
    }
}
