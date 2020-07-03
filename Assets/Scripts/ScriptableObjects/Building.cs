using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Building")]
public class Building : ScriptableObject
{
    public string BuildingName;
    public string Description;
    public Vector2 Size;
    public GameObject Prefab;
    public Sprite Icon;
}
