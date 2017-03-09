using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float StartRadius;                   //radius for each x,y axes, respectively
    public Circle CircleObject;
    public int NumOfCircles = 3;
    public int NumOfItems = 12;

    private List<Circle> circles = new List<Circle>(); 

    // Use this for initialization
    void Start()
    {
      
    }

    void FindNeighbor()
    {
        for (int i = 0; i < NumOfCircles; i++)
        {
            var cir = circles[i];
            if (circles.ElementAtOrDefault(i - 1) != null) cir.NearCircle1 = circles.ElementAtOrDefault(i - 1);
            if (circles.ElementAtOrDefault(i + 1) != null) cir.NearCircle2 = circles.ElementAtOrDefault(i + 1);
        }
    }

    public void InitCircles()
    {
        var tempRadius = StartRadius;
        var tempNumItems = NumOfItems;
        for (int i = 0; i < NumOfCircles; i++)
        {
            
            Circle cir = Instantiate(CircleObject, Vector3.zero, Quaternion.identity);
            cir.name = "Circle-" + i;
            cir.Initialize(tempNumItems, tempRadius);
            circles.Add(cir);
            tempRadius -= 0.5F;
            tempNumItems -= 2;
        }
        FindNeighbor();
    }

    public void Restart()
    {
        foreach (var item in circles)
        {
            Destroy(item.gameObject);
        }
        circles.Clear();
        InitCircles();
    }

}
