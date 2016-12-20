using Entitas;
using Entitas.CodeGenerator;
using UnityEngine;

[Settings, SingleEntity]
public class PlayerOptionsComponent : IComponent
{
    public float MoveSpeed;
}
