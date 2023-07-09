using UnityEngine;
using System;

public class MovementExposer : MonoBehaviour, ICharacterAttributes
{
    [SerializeField] private GameObject character;

    private KeyCode key;
    private bool hold;

    private Action<ICharacterAttributes> onClickDown;
    private Action<ICharacterAttributes> onClickUp;

    public Transform Character => character.transform;
    public KeyCode Key => key;
    public Vector3 HolePosition => transform.position;
    public bool Hold { get => hold; set => hold = value; }

    public void SetKey(KeyCode key)
    {
        this.key = key;
    }

    public void RegisterEvents(Action<ICharacterAttributes> onClickUp = null, Action<ICharacterAttributes> onClickDown = null)
    {
        if(onClickUp != null)
            this.onClickUp += onClickUp;
        if (onClickDown != null)
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
