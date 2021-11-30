using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    public float speed { get; set; }
    public Transform transform { get; set; }
    void Start();
    void Update();
}
