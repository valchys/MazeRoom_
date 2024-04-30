using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishDetector : MonoBehaviour
{
    private void OnColissionEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // End the level here
            Debug.Log("Level completed!");
            // Add your level completion logic here
        }
    }
}