using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;



public class FirstPersonController : MonoBehaviour
{
    public bool CanMove { get; set; } = true;
    public bool IsSprinting => canSprint && Input.GetKey(sprintKey);
    public bool IsBreathing => canBreath && Input.GetKey(holdBreath);
    private bool ShouldJump => Input.GetKeyDown(jumpKey) && CharacCtrl.isGrounded;
    private bool ShouldCrouch => Input.GetKeyDown(crouchKey) && !inCrouchAnim && CharacCtrl.isGrounded;

    [Header("Function Options")]
    public bool canMouseLook = true;
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canHeadBob = true;
    [SerializeField] private bool canInteract = true;
    [SerializeField] private bool canBreath = true;
    [SerializeField] private bool useFootsteps = true;
    [SerializeField] private bool useStamina = true;
    [SerializeField] private bool useOxygen = true;

    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.C;
    [SerializeField] private KeyCode interactKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode holdBreath = KeyCode.E;
    [SerializeField] public KeyCode returnKey = KeyCode.R;

    [Header("Movement Parameters")]
    [SerializeField] private float WalkSpeed = 3.0f;
    [SerializeField] private float SprintSpeed = 6.0f;
    [SerializeField] private float crouchSpeed = 0.25f;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float LookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float LookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float UpperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float LowerLookLimit = 80.0f;

    [Header("Jumping Parameters")]
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 30.0f;

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standHeight = 2f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);

    private bool isCrouching;
    private bool inCrouchAnim;

    [Header("Stamina Parameters")]
    [SerializeField] private float maxStamina = 100;
    [SerializeField] private float staminaDrain = 5;
    [SerializeField] private float timeBeforeStart = 5;
    [SerializeField] private float staminaRegenValue = 2;
    [SerializeField] private float staminaRegenTime = 0.1f;
    [SerializeField] private float currentStamina;
    private Coroutine regeneratingStamina;

    [Header("HoldBreath")]
    [SerializeField] private float maxOxygen = 100;
    [SerializeField] private float oxygenDrain = 5;
    [SerializeField] private float timeBeforeOxygenStart = 5;
    [SerializeField] private float oxygenRegenValue = 2;
    [SerializeField] private float oxygenRegenTime = 0.1f;
    [SerializeField] private float currentOxygen;
    private Coroutine regeneratingOxygen;

    [Header("Headbob Parameters")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float SprintBobSpeed = 18f;
    [SerializeField] private float SprintBobAmount = 0.11f;
    [SerializeField] private float crouchBobSpeed = 7f;
    [SerializeField] private float crouchBobAmount = 0.02f;
    private float defaultYPos = 0;
    private float timer = 20;

    [Header("Footstep Parameters")]
    [SerializeField] private float baseStepSpeed = 0.1f;
    [SerializeField] private float crouchStepMultiplier = 1.5f;
    [SerializeField] private float SprintStepMultiplier = 0.5f;
    [SerializeField] private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioClip[] woodClips = default;
    [SerializeField] private AudioClip[] stoneClips = default;
    [SerializeField] private AudioClip[] tileClips = default;
    [SerializeField] private AudioClip[] metalClips = default;
    [SerializeField] private float footstepTimer = 0;
    private float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultiplier : IsSprinting ? baseStepSpeed * SprintStepMultiplier : baseStepSpeed;

    [Header("Damage Overlay")]
    [SerializeField] private Image damageScreen;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private float duration;
    [SerializeField] private float durationTimer;

    [Header("Interaction")]
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayer = default;
    private Interactable currentInteractable;
    private Interactable previousInteractable;

    [Header("Examine Settings")]
    [SerializeField] private Transform examinePoint;
    [SerializeField] private float rotationSpeed = 100f;

    [SerializeField] GameObject examineText;

    private GameObject currentExaminedItem;
    private bool isExamining = false;
    private bool isMandatoryPickup = false;
    private Vector3 originalItemPosition;
    private Quaternion originalItemRotation;
    private Transform originalItemParent;

    [Header("Additional Settings")]

    [SerializeField] private Camera playerCamera;
    private CharacterController CharacCtrl;
    [SerializeField] private Enemy enemy;

    private Vector3 MoveDir;
    public Vector2 CurrentInput;

    private float rotationX = 0;

    public static FirstPersonController instance;

    void Awake()
    {
        instance = this;

        playerCamera = GetComponentInChildren<Camera>();
        CharacCtrl = GetComponent<CharacterController>();
        defaultYPos = playerCamera.transform.localPosition.y;
        damageScreen.color = new Color(damageScreen.color.r, damageScreen.color.g, damageScreen.color.b, 0);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if(canMouseLook)
        {
            MouseLook();
        }


        if (useOxygen)
        {
            HandleOxygen();
        }
            
        if (canInteract)
        {
            HandleInteractionCheck();
            HandleInteractionInput();
        }

        if (isExamining)
        {
            RotateExaminedItem();
            HandleExamineInput();
        }


        if (CanMove)
        {
            MovementInput();

            ApplyMovement();

            if (canJump)
                HandleJump();

            if (canCrouch)
                HandleCrouch();

            if (canHeadBob)
                HandleHeadBob();

            if (useFootsteps)
                HandleFootstep();

            if (useStamina)
                HandleStamina();
        }

        if (damageScreen.color.a > 0)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                float regenTempAlpha = damageScreen.color.a;
                regenTempAlpha -= Mathf.Clamp(Time.deltaTime * fadeSpeed, 0, 0.5f);
                damageScreen.color = new Color(damageScreen.color.r, damageScreen.color.g, damageScreen.color.b, regenTempAlpha);
            }
        }
    }

    // ***** PLAYER MOVEMENT *****

    private void MovementInput()
    {
        CurrentInput = new Vector2((isCrouching ? crouchSpeed : IsSprinting ? SprintSpeed : WalkSpeed) * Input.GetAxis("Vertical"), (isCrouching ? crouchSpeed : IsSprinting ? SprintSpeed : WalkSpeed) * Input.GetAxis("Horizontal"));

        float moveDirectionY = MoveDir.y;
        MoveDir = (transform.TransformDirection(Vector3.forward) * CurrentInput.x) + (transform.TransformDirection(Vector3.right) * CurrentInput.y);
        MoveDir.y = moveDirectionY;
    }

    // ***** PLAYER LOOK *****

    private void MouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * LookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -UpperLookLimit, LowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * LookSpeedX, 0);
    }

    public void ForceLookAtObject(Transform objectPosition)
    {
        playerCamera.transform.DOLookAt(objectPosition.position, 2).onComplete= () =>
        {
            float convertedRotation = ConvertToValidRotation(playerCamera.transform.localEulerAngles.x);
            rotationX = convertedRotation;
        };
    }
    
    float ConvertToValidRotation(float value)
    {
        if (value > 180)
        {
            value -= 360; // Convert values above 180 to negative range
        }

        return value;
    }

    // ***** Jumping *****

    private void HandleJump()
    {
        if (ShouldJump)
            MoveDir.y = jumpForce;
    }

    // ***** Jumping *****

    private void HandleCrouch()
    {
        if (ShouldCrouch)
            StartCoroutine(CrouchStand());
    }

    // ***** PLAYER HEADBOB *****

    private void HandleHeadBob()
    {
        if (!CharacCtrl.isGrounded) return;

        if (Mathf.Abs(MoveDir.x) > 0.1f || Mathf.Abs(MoveDir.z) > 0.1f)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : IsSprinting ? SprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer)* (isCrouching ? crouchBobAmount : IsSprinting ? SprintBobAmount : walkBobAmount),
                playerCamera.transform.localPosition.z);
        }
    }

    // ***** PLAYER STAMINA *****

    private void HandleStamina()
    {
        if(IsSprinting && CurrentInput != Vector2.zero)
        {
            if(regeneratingStamina != null)
            {
                StopCoroutine(regeneratingStamina);
                regeneratingStamina = null;
            }

            currentStamina -= staminaDrain * Time.deltaTime;

            if (currentStamina < 0)
                currentStamina = 0;
            if (currentStamina <= 0)
                canSprint = false;
        }
        if(!IsSprinting && currentStamina < maxStamina && regeneratingStamina == null)
        {
            regeneratingStamina = StartCoroutine(RegenStamina());
        }
    }

    // ***** PLAYER OXYGEN *****
    private void HandleOxygen()
    {

        if (Input.GetKeyDown(holdBreath))
        {
            FindObjectOfType<SoundManager>().Play("BreathIn");
        }

        if (IsBreathing && currentOxygen != 0)
        {          
            if (regeneratingOxygen != null)
            {                
                StopCoroutine(regeneratingOxygen);
                regeneratingOxygen = null;
            }

            currentOxygen -= oxygenDrain * Time.deltaTime;

            float tempAlpha = damageScreen.color.a;
            tempAlpha += Mathf.Clamp(Time.deltaTime * fadeSpeed, 0, 0.5f);
            damageScreen.color = new Color(damageScreen.color.r, damageScreen.color.g, damageScreen.color.b, tempAlpha);
            durationTimer = 0;


            CanMove = false;
            enemy.hearRadius = 0;
            enemy.chaseDuration = 2;


            if (currentOxygen < 0)
                currentOxygen = 0;
            if (currentOxygen <= 0)
                canBreath = false;
        }
        if (!IsBreathing && currentOxygen < maxOxygen && regeneratingOxygen == null)
        {   
            FindObjectOfType<SoundManager>().Play("BreathOut");
            regeneratingOxygen = StartCoroutine(RegenOxygen());
        }
    }
    // ***** PLAYER FOOTSTEP SOUNDS *****

    private void HandleFootstep()
    {
        if (!CharacCtrl.isGrounded) return;
        if (CurrentInput == Vector2.zero) return;

        footstepTimer -= Time.deltaTime;

        if(footstepTimer <= 0)
        {
            if(Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 30 , LayerMask.GetMask("Floor")))
            {
                switch (hit.collider.tag)
                {
                    case "Footstep/WOOD":
                        footstepAudioSource.PlayOneShot(woodClips[Random.Range(0, woodClips.Length - 1)]);
                        break;
                    case "Footstep/STONE":
                        footstepAudioSource.PlayOneShot(stoneClips[Random.Range(0, stoneClips.Length - 1)]);
                        break;
                    case "Footstep/TILE":
                        footstepAudioSource.PlayOneShot(tileClips[Random.Range(0, tileClips.Length - 1)]);
                        break;
                    case "Footstep/METAL":
                        footstepAudioSource.PlayOneShot(metalClips[Random.Range(0, metalClips.Length - 1)]);
                        break;
                    default:
                        break;
                }
            }

            footstepTimer = GetCurrentOffset;
        }
    }

    private void HandleInteractionCheck()
    {
        previousInteractable = currentInteractable;

        if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
        {
            if (hit.collider.gameObject.layer == 6 && (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID()))
            {
                hit.collider.TryGetComponent(out currentInteractable);

                if (currentInteractable)
                    currentInteractable.OnFocus();
            }
        }
        else
        {
            currentInteractable = null;
        }

        if (previousInteractable != currentInteractable)
        {
            if (previousInteractable != null)
            {
                previousInteractable.OnLoseFocus();
            }
        }
    }

    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(interactKey) && currentInteractable != null)
        {
            if (currentInteractable is PickupItem pickupItem)
            {
                StartExamination(pickupItem.gameObject);
            }
            else
            {
                currentInteractable.OnInteract();
            }
        }
    }

    private void StartExamination(GameObject item)
    {
        if (isExamining) return;

        isExamining = true;
        currentExaminedItem = item;
        CanMove = false;
        canMouseLook = false;

        originalItemParent = item.transform.parent;
        originalItemPosition = item.transform.position;
        originalItemRotation = item.transform.rotation;

        item.transform.SetParent(examinePoint);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        Collider itemCollider = item.GetComponent<Collider>();
        if (itemCollider != null)
        {
            itemCollider.enabled = false;
        }

        examineText.SetActive(true);
    }

    private void HandleExamineInput()
    {
        PickupItem pickupScript = currentExaminedItem.GetComponent<PickupItem>();
        // Pick up item
        if (Input.GetKeyDown(KeyCode.E) && pickupScript.canBePickedUp)
        {
            PickUpExaminedItem();
        }

        // Return item if not important
        if (!pickupScript.isMandatoryPickup && Input.GetKeyDown(returnKey))
        {
            ReturnExaminedItem();
        }
    }
    private void PickUpExaminedItem()
    {
        if (currentExaminedItem != null)
        {
            PlayerInventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
            PickupItem pickupScript = currentExaminedItem.GetComponent<PickupItem>();

            if (pickupScript != null && inventory != null)
            {
                inventory.AddItem(pickupScript.ItemID);
                FindObjectOfType<SoundManager>().Play("pickup");
                Destroy(currentExaminedItem);
                Debug.Log($"Picked up item: {pickupScript.ItemID}");
            }
        }
        
        EndExamination();
    }

    private void RotateExaminedItem()
    {
        if (currentExaminedItem == null) return;

        float rotationX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        Vector3 rotation = new Vector3(rotationY, -rotationX, 0);
        currentExaminedItem.transform.Rotate(rotation, Space.World);
    }

    private void ReturnExaminedItem()
    {
        if (currentExaminedItem != null)
        {
            currentExaminedItem.transform.SetParent(originalItemParent);
            currentExaminedItem.transform.position = originalItemPosition;
            currentExaminedItem.transform.rotation = originalItemRotation;

            // turn on item collider here.
            Collider itemCollider = currentExaminedItem.GetComponent<Collider>();
            if (itemCollider != null)
            {
                itemCollider.enabled = true;
            }
        }
        EndExamination();
    }

    private void EndExamination()
    {
        isExamining = false;
        CanMove = true;
        canMouseLook = true;
        currentExaminedItem = null;
        examineText.SetActive(false);
    }

    private void ApplyMovement()
    {
        if (!CharacCtrl.isGrounded)
            MoveDir.y -= gravity * Time.deltaTime;

        CharacCtrl.Move(MoveDir * Time.deltaTime);
    }

    // ***** Coroutines *****

    private IEnumerator CrouchStand()
    {

        if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
            yield break;

        inCrouchAnim = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standHeight : crouchHeight;
        float currentHeight = CharacCtrl.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = CharacCtrl.center;

        while(timeElapsed < timeToCrouch)
        {
            CharacCtrl.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            CharacCtrl.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        CharacCtrl.height = targetHeight;
        CharacCtrl.center = targetCenter;

        isCrouching = !isCrouching;

        inCrouchAnim = false;
    }

    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(timeBeforeStart);
        WaitForSeconds timeToWait = new WaitForSeconds(staminaRegenTime);

        while(currentStamina < maxStamina)
        {
            if (currentStamina > 0)
                canSprint = true;

            currentStamina += staminaRegenValue;

            if (currentStamina > maxStamina)
                currentStamina = maxStamina;

            yield return timeToWait;
        }

        regeneratingStamina = null;
    }
    private IEnumerator RegenOxygen()
    {
        yield return new WaitForSeconds(timeBeforeOxygenStart);
        WaitForSeconds timeToWait = new WaitForSeconds(oxygenRegenTime);


        while (currentOxygen < maxOxygen)
        {
            if (currentOxygen > 0)
                canBreath = true;


            currentOxygen += oxygenRegenValue;


            enemy.hearRadius = 3;
            enemy.chaseDuration = 10;
            CanMove = true;


            if (currentOxygen > maxOxygen)
                currentOxygen = maxOxygen;


            yield return timeToWait;
        }


        regeneratingOxygen = null;
    }
}