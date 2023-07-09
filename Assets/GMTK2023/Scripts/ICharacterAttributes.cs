using System;
using UnityEngine;

public interface ICharacterAttributes
{
    public bool Hold { get; set; }
    public Animator Animator { get; }
    public KeyCode Key { get; }
    public Vector3 HolePosition { get; }
    public Transform Character { get; }
}