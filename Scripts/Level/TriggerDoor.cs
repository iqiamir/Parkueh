using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoor : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;

    [SerializeField] private bool FinalDoorTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (FinalDoorTrigger)
            {
                myDoor.Play("openDoor", 0, 0.0f);
                gameObject.SetActive(false);
            }
        }
    }
}

