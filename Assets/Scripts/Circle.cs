using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    public GameObject CircleSpriteObject;

    public Circle NearCircle1;
    public Circle NearCircle2;

    public AudioClip DragStartSound;

    private bool dragging = false;

    private float startAngle = 0.0f;

    [SerializeField]
    private List<CirclePoint> points = new List<CirclePoint>(); 

    private float startCircleFactor = 0.4F;
    private float forwardSpeed;
    private float backwardSpeed;
    //0.79 radius 2 / 0.598 - radius 1.5 / 0.407 - radius 1
    private int numPoints;                      //number of points on radius to place prefabs

    public CirclePoint PointPrefab;
    private float radius;                  //radii for each x,y axes, respectively

    Vector3 pointPos;                                //position to place each prefab along the given circle/eliptoid
                                                     //*is set during each iteration of the loop
            
        
    void Start ()
    {
        forwardSpeed = Random.Range(1, 3);
        backwardSpeed = Random.Range(1, 3);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
            if (hit)
            {
                
                if(hit.collider == gameObject.GetComponent<PolygonCollider2D>())
                {
                    SoundManager.instance.PlaySingle(DragStartSound);
                    hit.transform.gameObject.GetComponent<Circle>().dragging = true;
                    var screenPos = Camera.main.WorldToScreenPoint(transform.position);
                    var vec = Input.mousePosition - screenPos;
                    hit.transform.gameObject.GetComponent<Circle>().startAngle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
                    hit.transform.gameObject.GetComponent<Circle>().startAngle -= Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;
                }
                
            }
            else {
               // Debug.Log("No colliders hit from mouse click");
            }
        }

        if (dragging)
        {
            var screenPos = Camera.main.WorldToScreenPoint(transform.position);
            var vec = Input.mousePosition - screenPos;
            var angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - startAngle, Vector3.forward);
            if (NearCircle1 != null)
            {
                NearCircle1.transform.Rotate(Vector3.back * backwardSpeed);
                //NearCircle1.transform.rotation = originalRot * Quaternion.AngleAxis(-angle, Vector3.forward);
            }
            if (NearCircle2 != null)
            {
                NearCircle2.transform.Rotate(Vector3.forward * forwardSpeed);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (dragging)
            {
                SoundManager.instance.PlaySingle(DragStartSound);
                dragging = !dragging;
                GameManager.instance.CheckCompleteZone();
            }
        }
    }

    public void Initialize(int numOfPoints, float rad)
    {
        numPoints = numOfPoints;
        radius = rad;

        CreateCircle();
    }

    public void CreateCircle()
    {        
        gameObject.transform.localScale = new Vector3(GetCircleScale(radius) + 0.03F, GetCircleScale(radius) + 0.03F, 0);
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
        int firstActivePoint = Random.Range(0, 3);
        int numPointsToNext = numPoints / 2;
        int lastActivePoint = firstActivePoint + numPointsToNext;
        //Debug.Log(gameObject.name + ":[" + firstActivePoint + ":" + lastActivePoint + "]");
        for (int i = 0; i < numPoints; i++)
        {
            //multiply 'i' by '1.0f' to ensure the result is a fraction
           
            float pointNum = (i * 1.0f) / numPoints;

            //angle along the unit circle for placing points
            float angle = pointNum * Mathf.PI * 2;
            float x = Mathf.Sin(angle) * radius;
            float y = Mathf.Cos(angle) * radius;

            pointPos = new Vector3(x, y) + gameObject.transform.position;
            //position for the point prefab
            var currentAngle = GetAngleToTarget(pointPos);
            //place the prefab at given position
            CirclePoint point = Instantiate(PointPrefab, pointPos, Quaternion.Euler(0, 0, currentAngle));
            bool activeType = false;
            if (i == firstActivePoint || i == lastActivePoint)
            {
                activeType = true;
            }
            else
            {
                activeType = Random.value > 0.7; // RAndom active points 30% 
            }
            point.Initialize(activeType);
            point.transform.parent = gameObject.transform;
            points.Add(point);
        }
        var cir = Instantiate(CircleSpriteObject, transform.position, Quaternion.identity);
		cir.transform.parent = gameObject.transform;
        CircleSpriteObject.transform.localScale = new Vector3(GetCircleScale(radius), GetCircleScale(radius), 0);
    }

    private float GetAngleToTarget(Vector3 targetPos)
    {
        Vector3 v3 = targetPos - transform.position;
        return Mathf.Atan2(v3.y, v3.x) * Mathf.Rad2Deg;
    }

    private float GetCircleScale(float radius)
    {
        return radius * startCircleFactor;
    }
}
