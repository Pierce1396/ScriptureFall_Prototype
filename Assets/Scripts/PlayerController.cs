using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;
    public Transform spriteRenderer;
    public LayerMask whatStopsMovement;

    private Vector3 basePositionOffset;
    private bool isMoving = false;
    private bool isHorizontalMove = false;
    private float moveStartTime;

    public float frequency = .001f; // speed of the hop
    public float amplitude = 0.0015f; // height of the hop

    private bool justFinishedVerticalMove = false;
    private bool impactCooldownActive = false;

    void Start()
    {
        movePoint.parent = null;
        basePositionOffset = transform.localPosition;
    }

    void Update()
    {
        isMoving = Vector3.Distance(transform.position, movePoint.position) > 0.05f;

        float hopOffset = 0f;
        if (isMoving && isHorizontalMove)
        {
            if (moveStartTime == 0f)
                moveStartTime = Time.time;

            hopOffset = Mathf.Sin((Time.time - moveStartTime) * frequency) * amplitude;
        }
        else if (!isMoving)
        {
            moveStartTime = 0f;

            if (justFinishedVerticalMove && !impactCooldownActive)
            {
                justFinishedVerticalMove = false;
                StartCoroutine(VerticalImpact());
            }
        }

        Vector3 targetPosition = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        transform.position = new Vector3(targetPosition.x, targetPosition.y + hopOffset, targetPosition.z);

        if (!isMoving)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            if (Mathf.Abs(horizontalInput) == 1f)
            {
                isHorizontalMove = true;

                spriteRenderer.localScale = new Vector3(horizontalInput < 0 ? -1 : 1, 1, 1);

                Vector3 direction = new Vector3(horizontalInput, 0f, 0f);
                if (!Physics2D.OverlapCircle(movePoint.position + direction, 0.2f, whatStopsMovement))
                {
                    movePoint.position += direction;
                }
            }
            else if (Mathf.Abs(verticalInput) == 1f)
            {
                isHorizontalMove = false;

                Vector3 direction = new Vector3(0f, verticalInput, 0f);
                if (!Physics2D.OverlapCircle(movePoint.position + direction, 0.2f, whatStopsMovement))
                {
                    movePoint.position += direction;
                    justFinishedVerticalMove = true;
                }
            }
        }
    }

    private IEnumerator VerticalImpact()
    {
        impactCooldownActive = true;

        yield return new WaitForSeconds(0.1f); // Let movement fully finish

        Vector3 startPos = transform.position;
        Vector3 thudPos = startPos + new Vector3(0f, -0.04f, 0f);

        float duration = 0.08f;
        float timer = 0f;

        // Bump down
        while (timer < duration)
        {
            transform.position = Vector3.Lerp(startPos, thudPos, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        // Bump back up
        timer = 0f;
        while (timer < duration)
        {
            transform.position = Vector3.Lerp(thudPos, startPos, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = startPos;
        impactCooldownActive = false;
    }
}
