using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;


public class MagicScript : MonoBehaviour
{
    //Singleton
    public static MagicScript S;
    [Header("Magic Meter Values")]
    public float playerMagic;
    public float magicIncrease = 0.0f;
    public Image magicBar;
    public Rigidbody2D rb, tempRB;
    public Transform player;
    private const float MAX_MAGIC = 100f;

    [Header("Hover Data Values")]
    public float hoveringSpeed;
    private float initialGravityScale;

    [Header("Hookshot Data Values")]
    public Camera cam;
    public LineRenderer lineRen;
    public LayerMask grappleLayer;
    public float moveSpeed = 50f;
    public float grappleLength = 25f;
    [Min(1)]
    public int maxPoints = 1;
    public bool IsGrappleModeActive = false;
    private bool IsLatchedOn = false;
    private List<Vector2> points = new List<Vector2>();

    [Header("Fireball Values")]
    public GameObject fireballPrefab;
    public float fireballSpeed;
    public float projectileCooldown;
    public float tempPCHolder;
    

    [Header("Enemy Entrapment Data")]
    public Transform entrapMagnet;
    public float magnetStrength;
    public float magnetRange;
    public float distanceToEnemy;
    public Vector2 pullForce;
    private bool activateMagnet = false;
    public Animator animator;



    private void Awake()
    {
        if (S)
        {
            Destroy(this.gameObject);
        }
        else
        {
            S = this;
        }
  
        //player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Start()
    {
       

        //Magic Bar Essentials
        playerMagic = MAX_MAGIC;
        magicBar = GetComponent<Image>();
        initialGravityScale = rb.gravityScale;

        //Temp Projectile Holder
        tempPCHolder = projectileCooldown;


        //Hovering Data
        hoveringSpeed = 2f;
        tempRB.velocity = rb.velocity;
        lineRen.positionCount = 0;

        //animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //Update Magic Bar
        magicBar.fillAmount = Mathf.Clamp(playerMagic / (MAX_MAGIC + magicIncrease), 0f, MAX_MAGIC + magicIncrease);

        //Update Animator
        UpdateAnimator();

        //Projectile Cooldown Timer
        projectileCooldown -= Time.deltaTime;

        if (GameManagerScript.S.currentState == GameState.Playing)
        {
            //Magic Strike
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    playerMagic -= 25f;

                }

            }
            //Hover Mechanic
            if (Input.GetKey(KeyCode.W) && !PlayerController.S.hasLanded() && !PlayerController.S.LatchedOntoWall() && HasMeter())
            {
                rb.gravityScale = 0f;
                rb.velocity = new Vector2(rb.velocity.x, 0.1f);
                playerMagic -= 0.1f;

            }
            if (Input.GetKeyUp(KeyCode.W) || PlayerController.S.hasLanded() || !PlayerController.S.LatchedOntoWall() || !HasMeter())
            {
                rb.gravityScale = initialGravityScale;
                rb.velocity = tempRB.velocity;

            }

            //Hookshot Mechanic
            if (Input.GetKeyDown(KeyCode.Q) && MagicScript.S.HasMeter() && !IsLatchedOn && !IsGrappleModeActive)
            {
                animator.SetBool("IsGrappleModeActive", true);

                //Disable Player Movement
                PlayerController.S.isGrappleActivated = true;

                //Activate Grapple Mode
                IsGrappleModeActive = true;

                //Reveal the Aim Pointer
                AimSightScript.S.aimPointer.SetActive(true);

            }
            //Fire Hookshot
            if (Input.GetMouseButtonDown(0) && IsGrappleModeActive)
            {
                //Disable the Aim Pointer
                AimSightScript.S.aimPointer.SetActive(false);

                animator.SetBool("IsGrappleModeActive", false);

                animator.SetBool("IsHooked", true);

                ActivateHookshot();

            }
            if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow)) && IsLatchedOn && IsGrappleModeActive)
            {
                DeactivateHookshot();
            }
            //Cancel Hookshot
            if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow)) && !IsLatchedOn && IsGrappleModeActive)
            {
                //Disable the Aim Pointer
                AimSightScript.S.aimPointer.SetActive(false);

                animator.SetBool("IsGrappleModeActive", false);

                //Disable Player Movement
                PlayerController.S.isGrappleActivated = false;

                //Activate Grapple Mode
                IsGrappleModeActive = false;
            }

            if(Input.GetKey(KeyCode.T) && HasMeter())
            {
                distanceToEnemy = Vector2.Distance(entrapMagnet.position, EnemyScript.S.transform.position);
                if(distanceToEnemy <= magnetRange)
                {
                    pullForce = (EnemyScript.S.transform.position - entrapMagnet.position).normalized / distanceToEnemy * magnetStrength;
                    EnemyScript.S.rb.AddForce(pullForce, ForceMode2D.Force);
                }
                activateMagnet = true;
                playerMagic -= 0.25f;
            }
            if (Input.GetKeyDown(KeyCode.T) || !HasMeter())
            {
                activateMagnet = false;
              
            }

            //Fireball Mechanic
            if (Input.GetKeyDown(KeyCode.X) && HasMeter())
            {
                //Fire the projectile
                ActivateProjectile();
                //Deplete Meter
                playerMagic -= 0.15f;
            }
        }

    }

    
    public bool HasMeter()
    {
        if (playerMagic <= 0)
        {
            return false;
        }
        return true;
    }

    private void ActivateHookshot()
    {
        playerMagic -= 0.35f;
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)rb.transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(rb.transform.position, direction, grappleLength, grappleLayer);

        if (hit.collider)
        {
            Vector2 hitPoint = hit.point;
            points.Add(hitPoint);
            if (points.Count > maxPoints)
            {
                points.RemoveAt(0);
            }
           
        }


        if (points.Count > 0)
        {
            Vector2 moveTo = centriod(points.ToArray());
            rb.MovePosition(Vector2.MoveTowards((Vector2)transform.position, moveTo, Time.unscaledDeltaTime * moveSpeed));
            
            lineRen.positionCount = 0;
            lineRen.positionCount = points.Count * 2;
            for (int n = 0, j = 0; n < points.Count * 2; n += 2, j++)
            {
                lineRen.SetPosition(n, transform.position);
                lineRen.SetPosition(n + 1, points[j]);
               
            }
           
        }

       
    }

    private void DeactivateHookshot()
    {
        lineRen.positionCount = 0;
        points.Clear();
        //Disable Player Movement
        PlayerController.S.isGrappleActivated = false;

        //Activate Grapple Mode
        IsGrappleModeActive = false;

        //No longer latched
        IsLatchedOn = false;
    }

   private Vector2 centriod(Vector2[] points)
    {
        Vector2 center = Vector2.zero;
        foreach (Vector2 point in points)
        {
            center += point;
        }
        center /= points.Length;
        return center;
    }



    private void OnDrawGizmos()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

        Gizmos.DrawLine(transform.position, (Vector2)transform.position + direction);

        foreach (Vector2 point in points)
        {
            Gizmos.DrawLine(transform.position, point);
        }
    }

    private void UpdateAnimator()
    {
        
        //animator.SetBool("IsLatchedOnWall", IsLatchedOn);
    }

    public void LatchedOn()
    {
        Debug.Log("Latched");
        animator.SetBool("IsHooked", false);
        IsLatchedOn = true;
    }

    private void ActivateProjectile()
    {
        //Check for cooldown
       if(projectileCooldown <= 0)
        {
            //Restart Cooldown
            projectileCooldown = tempPCHolder;
            //Instantiate projectile
            GameObject projectile = Instantiate(fireballPrefab, player.transform.position, Quaternion.identity);
            Vector3 projectileVelocity = Vector3.right * fireballSpeed;

            if (!PlayerController.S.isFacingRight)
            {
                //Flip the projectile
                Vector3 projectileScale = projectile.transform.localScale;
                projectileScale.x *= -1.0f;
                projectile.transform.localScale = projectileScale;
            }

            projectile.GetComponent<Rigidbody2D>().velocity = projectileVelocity;
        }
        
    }
}
