using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRandomNum : MonoBehaviour
{
    public Transform[] elements; // Array of transforms to pick from
    private List<int> shuffledIndices; // Shuffled indices of elements
    private int currentIndex = 0; // Current index in the shuffled indices list

    void Start()
    {
        ShuffleIndices();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Transform randomElement = GetNextRandomElement();
            Debug.Log("Random Element: " + randomElement.name);
        }
    }

    // Shuffles the indices of the elements array using Fisher-Yates algorithm
    void ShuffleIndices()
    {
        shuffledIndices = new List<int>();

        // Initialize the list with indices
        for (int i = 0; i < elements.Length; i++)
        {
            shuffledIndices.Add(i);
        }

        // Fisher-Yates shuffle algorithm
        int n = shuffledIndices.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1); // Generate a random index from 0 to n (inclusive)
            int value = shuffledIndices[k]; // Get the value at index k
            shuffledIndices[k] = shuffledIndices[n]; // Swap the value at index k with the value at index n
            shuffledIndices[n] = value; // Place the original value at index k in the position of index n
        }

        currentIndex = 0; // Reset current index to the start
    }

    // Retrieves the next random element in the shuffled order
    Transform GetNextRandomElement()
    {
        if (currentIndex >= shuffledIndices.Count)
        {
            // All elements have been picked, shuffle indices again
            ShuffleIndices();
        }

        int nextIndex = shuffledIndices[currentIndex];
        currentIndex++;

        return elements[nextIndex];
    }
}
