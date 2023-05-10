using UnityEngine;

// Mimic 생성 시 사용할 스크립터블 오브젝트 데이터
[CreateAssetMenu(menuName = "Scriptable/MimicData", fileName = "Mimic Data")]
public class MimicData : ScriptableObject
{
    [Header("Mimic Stats")]
    public float health = 100f;
    public float damage = 5f;
    public float speed = 3f;
    public float height = 0.5f;

    [Header("Legs")]
    [Tooltip("Leg placement radius offset")]
    public float newLegRadius = 3f;
    [Tooltip("Minimum leg distance from center of mimic")]
    public float minLegDistance = 4.5f;
    [Tooltip("Width Curve of mimic's legs")]
    public AnimationCurve widthCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

    [Header("Collider")]
    [Tooltip("Radius of Sphere Collider applied to mimic")]
    public float radius = 0.8f;

    [Header("RigidBody")]
    public float mass = 0.1f;

    [Header("Sphere")]
    [Tooltip("Scale of mimic's sphere")]
    public Vector3 scale = new Vector3(0.2f, 0.2f, 0.2f);

    [Header("Material")]
    public Material mimicMaterial;
}
