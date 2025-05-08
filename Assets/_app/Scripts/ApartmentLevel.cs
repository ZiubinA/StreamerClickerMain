using UnityEngine;

[CreateAssetMenu(fileName = "New Apartment Level", menuName = "Streamer Clicker/Apartment Level")]
public class ApartmentLevel : ScriptableObject
{
    public string apartmentName;
    public Sprite apartmentSprite; // Reference to sprites in Level Upgrade Images folder
    [TextArea(3, 5)]
    public string unlockMessage = "You've upgraded your apartment!";
}