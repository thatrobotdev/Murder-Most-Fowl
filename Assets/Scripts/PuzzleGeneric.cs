using UnityEngine;

public class PuzzleGeneric : MonoBehaviour
{
    [SerializeField] private GameObject puzzleScreenObj;
    
    public void DisablePuzzleScreen() {
        puzzleScreenObj.SetActive(false);
    }
}
