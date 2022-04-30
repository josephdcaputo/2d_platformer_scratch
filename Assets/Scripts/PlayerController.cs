using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /*
                                        +-----------------+                                                                                                            
                                        |                 |                                                                                                            
                                        | PlayerContoller |                                                                                                            
                                        |                 |                                                                                                            
                                        +-----------------+                                                                                                            
                                               ^ ^ ^                                                                                                                                      
                                               | | |                                                                                                                                      
                      +------------------------+ | +------------------------+                                                                                                             
                      |                          |                          |                                                                                                             
                      V                          V                          V                                                                                                             
             +----------------+         +----------------+         +----------------+                                                                                                            
             |                |         |                |         |                |                                                                                                            
             |   InputScrpt   |         | MovementScript |         | CollisionScript|                                                                                                            
             |                |         |                |         |                |                                                                                                            
             +----------------+         +----------------+         +----------------+                                                                                                            
    */

    //--------------------------------------
    //  Player Components 
    //--------------------------------------
    public Rigidbody2D body;
    public Collider2D collider;

    //--------------------------------------
    //  Inspector: SerializedFields
    //--------------------------------------
    [SerializeField] internal PlayerInputScript inputScript;
    [SerializeField] internal PlayerMovementScript movementScript;
    [SerializeField] internal PlayerCollisionScript collisionScript;

    public PlayerState_e playerState;

    public enum PlayerState_e
    {
        Grounded,
        PreparingToJump,
        InFlight,
        //PreparingToSlide,
        //Sliding,
    };

    // Start is called before the first frame update
    private void Awake()
    {
        playerState = PlayerState_e.Grounded;
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        // distToGround = collider.bounds.extents.y;
    }

    //  Player input detection goes into Update()
    private void Update()
    {
        UpdateStateMachine();
    }

    //  All PHYSICS go in FixedUpdate()
    private void FixedUpdate()
    {
        movementScript.ComputeVelocity();
    }


    void UpdateStateMachine()
    {
        //  The state machine will be the main driver of all mechanism related to the movement of the character
        /*
                        +----------+                           +-----------+                                                                                                                                             
                        |          |    DetectJumpingEvent()   | Preparing |                                                                                                                                             
                        | Grounded |-------------------------->|     To    |                                                                                                                                             
                        |          |                           |    Jump   |                                                                                                                                             
                        +----------+                           +-----------+                                                                                                                                             
                              ^                                      |                                                                                                           
                              |                                      |   DetectHasTakenOff()                                                                                                            
                              |                                      V                                                                                                           
                              |                                +----------+                                                                                                                                             
                              |         DetectHasLanded()      |          |                                                                                                                                             
                              +--------------------------------| InFlight |                                                                                                                                             
                                                               |          |                                                                                                                                             
                                                               +----------+                                                                                                                                             

        */
        //  Grounded is our default state
        switch (playerState)
        {
            case PlayerState_e.Grounded:

                if (inputScript.DetectJumpingEvent())
                {
                    Debug.Log("Grounded -> PreparingToJump");
                    playerState = PlayerState_e.PreparingToJump;
                }
                else
                {
                    playerState = PlayerState_e.Grounded;
                }
                break;

            case PlayerState_e.PreparingToJump:

                //  Setup all parameters to take flight
                if (collisionScript.DetectHasTakenOff())
                {
                    Debug.Log("PreparingToJump -> InFlight");
                    playerState = PlayerState_e.InFlight;
                }
                break;

            case PlayerState_e.InFlight:

                //  Continuously check if we have hit the ground yet.
                //  and apply all modifiers to get "normal" kinematic movement
                if (collisionScript.DetectHasLanded())
                {
                    Debug.Log("InFlight -> Grounded");
                    playerState = PlayerState_e.Grounded;
                }
                else
                {
                    playerState = PlayerState_e.InFlight;
                }
                break;

            default:
                playerState = PlayerState_e.Grounded;
                break;

        }
    }
}
