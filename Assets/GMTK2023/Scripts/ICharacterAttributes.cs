using System;
using UnityEngine;

public interface ICharacterAttributes
{
    public bool Hold { get; set; }
    public Transform Character { get; }
}