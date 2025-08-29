using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    public string characterName;
    [Space]
    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    [Space]
    [Header("Jump")]
    public float jumpForce;
    [Space]
    [Header("Health")]
    public float maxHealth;
}
