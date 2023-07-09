using System;
using UnityEngine;

public interface ICharacterAttributes
{
    public bool Hold { get; set; }
    public KeyCode Key { get; }
    public Transform Character { get; }
}