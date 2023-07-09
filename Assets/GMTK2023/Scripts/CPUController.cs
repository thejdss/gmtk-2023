using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUController : MonoBehaviour
{
    private Dictionary<int, Coroutine> coroutineDictionary = new Dictionary<int, Coroutine>();
    private Queue<ICharacterAttributes> movedPositions = new Queue<ICharacterAttributes>();

    [SerializeField] private GameObject BoardHolder;
    [SerializeField] private Hammer hammer;
    [SerializeField] private float checkDelay = 0.3f;

    public bool useQueue;

    private void Start()
    {
        hammer.RegisterCompleteMovement(ActiveUseQueue);
        var characters = BoardHolder.GetComponentsInChildren<MovementExposer>();
        foreach (MovementExposer item in characters)
        {
            item.RegisterEvents(null, CheckCharacterPosition);
        }
    }

    public void RegisterHammerCheckCollision(Action callback)
    {
        hammer.RegisterCheckCollision(callback);
    }

    private void Update()
    {
        if (useQueue)
        {
            if (movedPositions.Count > 0)
            {
                hammer.CheckHole(movedPositions.Dequeue());
                useQueue = false;
            }
        }
    }

    private void CheckCharacterPosition(ICharacterAttributes item)
    {
        if (coroutineDictionary.ContainsKey(item.Character.GetInstanceID()))
        {
            var anim = coroutineDictionary[item.Character.GetInstanceID()];
            if (anim != null)
                StopCoroutine(anim);

            coroutineDictionary[item.Character.GetInstanceID()] = StartCoroutine(ICheckCharacterPosition(item));
            return;
        }

        coroutineDictionary.Add(item.Character.GetInstanceID(), StartCoroutine(ICheckCharacterPosition(item)));
    }

    private IEnumerator ICheckCharacterPosition(ICharacterAttributes item)
    {
        yield return new WaitForSeconds(checkDelay);
        if (item.Character.localPosition.y > -0.80f)
        {
            movedPositions.Enqueue(item);
            if (!hammer.IsMoving)
                useQueue = true;
        }
    }

    private void ActiveUseQueue()
    {
        useQueue = true;
    }
}
