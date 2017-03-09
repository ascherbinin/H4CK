using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteZone : MonoBehaviour {
    //The list of colliders currently inside the trigger
    private List<Collider2D> TriggerList = new List<Collider2D>();
    // Use this for initialization

    public AudioClip CompleteSound;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if the object is not already in the list
        if (!TriggerList.Contains(other))
        {
            //add the object to the list
            TriggerList.Add(other);
        }
    }

    //called when something exits the trigger
    void OnTriggerExit2D(Collider2D other)
    {
        //if the object is in the list
        if (TriggerList.Contains(other))
        {
            //remove it from the list
            TriggerList.Remove(other);
        }
    }

    public bool CheckItems ()
    {
        Debug.Log(TriggerList.Count);
        bool isComplete = false;
        foreach (var item in TriggerList)
        {
            var point = item.gameObject.GetComponent<CirclePoint>();
            if(!point.Type)
            {
                
                isComplete = false;
                break;
            }
            isComplete = true;
            
        }

        if (!isComplete)
        {
            Debug.Log("Inccorect");
            return false;
        }
        else
        {
            Debug.Log("Complete");
            SoundManager.instance.PlaySingle(CompleteSound);
            return true;
        }
    }

    public void Restart()
    {
        TriggerList.Clear();
    }
}
