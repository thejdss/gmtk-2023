using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUController : MonoBehaviour
{
    private Dictionary<int, Coroutine> coroutineDictionary = new Dictionary<int, Coroutine>();
    private Queue<Vector2> movedPositions = new Queue<Vector2>();

    [SerializeField] private GameObject BoardHolder;
    [SerializeField] private float checkDelay = 0.3f;

    private void Start()
    {
        var characters = BoardHolder.GetComponentsInChildren<MovementExposer>();
        foreach (MovementExposer item in characters)
        {
            item.RegisterEvents(null, CheckCharacterPosition);
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
        if (item.Character.localPosition.y > 0.5f)
        {
            var pos = item.Character.TransformDirection(item.Character.localPosition);
            movedPositions.Enqueue(pos);
        }
    }
}
