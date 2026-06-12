using UnityEngine;
using UnityEngine.UIElements;

public class AngleToPlayer : MonoBehaviour
{
    private Transform player;
    private Vector3 targetPos;
    private Vector3 targetDir;

    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] testIdles;


    public float angle;
    public int lastIndex;
    public bool walking;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        walking = true;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        player = ProgressionManager.Instance.GetPlayer().transform;
    }

    // Update is called once per frame
    void Update()
    {
        // get target position and direction
        targetPos = new Vector3(player.position.x,transform.position.y,player.position.z);
        targetDir = targetPos - transform.position;

        // get angle
        angle = Vector3.SignedAngle(targetDir,transform.forward,Vector3.up);

        //flip if needed
        Vector3 tempScale = Vector3.one;
        if (angle < 0)
        {
            tempScale.x *=-1;
        }
        spriteRenderer.transform.localScale = tempScale;

        lastIndex = GetIndex(angle);

        if(walking) SetWalk();
    }

    private int GetIndex(float angle)
    {
        //front
        if(angle>-22.5f && angle < 22.6f) return 0;
        if(angle >= 22.5f && angle < 67.5f) return 7;
        if(angle >= 67.5f && angle < 112.5f) return 6;
        if(angle >= 112.5f && angle < 157.5f) return 5;
        // back
        
        if(angle <= -157.4f && angle < -112.5f) return 3;
        if(angle >= -112.5f && angle < -67.5f) return 2;
        if(angle >= -67.5f && angle <= -22.5f) return 1;
        if(angle <= -157.5 || angle>=157.5f) return 4;
        
        
        
        return lastIndex;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position,transform.forward);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, targetPos);
    }
    public void SetWalk()
    {
        spriteRenderer.sprite = testIdles[lastIndex]; // will be swaped with more sofisticated animation blend tree when more sprites are avalible
    }
}
