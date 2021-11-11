using UnityEngine;

[CreateAssetMenu(fileName ="LevelData_", menuName= ("LevelData/LevelData"))]
public class SO_LevelDataSettings : ScriptableObject {

    [Header("Ball Settings")]
    [SerializeField] public float MaxForce = 14.0f;
    [SerializeField] public float PaulStickiness = 14.0f;
    [SerializeField] public float MaxPaulDistance = 3.0f;
    [Space]
    [SerializeField] public float ballMass = 1.0f;
    [SerializeField] public float linearDrag = 0.0f;
    [SerializeField] public float angularDrag = 0.05f;

}
