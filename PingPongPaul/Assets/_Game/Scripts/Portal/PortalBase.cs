using System.Collections;
using UnityEngine;

public abstract class PortalBase : MonoBehaviour {

    [SerializeField] protected Bounds originalBounds;
    [SerializeField] protected LayerMask wallLayermask;

    [Header("References")]
    [SerializeField] protected SpriteRenderer spriteRenderer;

    [Header("Settings")]
    [SerializeField] protected bool canBeReset = true;
    [SerializeField] protected Color portalColor;

    [Header("Debug")]
    [SerializeField] protected bool canTeleport = false;
    [SerializeField] protected bool useDebug;
    [SerializeField] protected bool isGhost = true;

    public float Opacity = 1.0f;

    public void ConfirmPortalPlacement( ) {

        OnConfirmPortalPlacement();
        StartCoroutine(CheckPositionDelayed());
        isGhost = false;
    }
    protected abstract void OnConfirmPortalPlacement();

    public void CancelPortalPlacement() {
        Destroy(this);
    }

    protected void CalculateExitPosition(PortalBase otherPortal, GameObject paul) {
        Rigidbody2D rigidbody = paul.GetComponent<Rigidbody2D>();

        Vector3 inPosition = this.transform.InverseTransformPoint(paul.transform.position);
        inPosition = -inPosition;
        Vector3 outPosition = otherPortal.transform.TransformPoint(inPosition);

        Vector3 inDirection = this.transform.InverseTransformDirection(rigidbody.velocity);
        Vector3 outDirection = otherPortal.transform.TransformDirection(inDirection);

        paul.transform.position = outPosition;
        rigidbody.velocity = -outDirection;

        otherPortal.SetCanTeleport(false);
        canTeleport = false;
    }

    protected void SetCanTeleport(bool newActive) {
        canTeleport = newActive;
    }

    public bool CanTeleport() {
        return canTeleport;
    }

    public void ResetPortal() {
        if (canBeReset == true) {
            OnReset();
        }
    }

    private void Update() {

        if (useDebug == true) {
            Debug_VisualizeCollisionDepth();
        }

    }


    protected IEnumerator CheckPositionDelayed() {
        spriteRenderer.color = Color.clear;
        yield return new WaitForSeconds(0.05f);
        Collider2D collider = GetComponent<Collider2D>();
        SetPositionOffset(CheckPosition(collider.bounds.center));
        portalColor.a = Opacity;
        spriteRenderer.color = portalColor;
    }

    public Vector3 CheckPosition(Vector3 centerBoundsLocation) {

        Vector3 offsetDirection = Vector3.zero;
        float distance = 0.0f;

        Vector3 centerPosition = centerBoundsLocation + transform.right * originalBounds.extents.x;
        Vector3 rightEndPosition = centerPosition + transform.up * originalBounds.extents.y;
        Vector3 leftEndPosition = centerPosition + (-transform.up) * originalBounds.extents.y;

        CheckCorners(ref offsetDirection, ref distance, centerPosition, rightEndPosition, leftEndPosition);

        CheckForEmptiness(ref offsetDirection, ref distance, centerPosition, rightEndPosition, leftEndPosition);

        return offsetDirection * distance;
        
    }

    public void SetPositionOffset(Vector3 positionOffset) {
        transform.position += positionOffset;
    }

    private void CheckForEmptiness(ref Vector3 offsetDirection, ref float distance, Vector3 centerPosition, Vector3 rightEndPosition, Vector3 leftEndPosition) {
        RaycastHit2D rightBackwards = Physics2D.Raycast(rightEndPosition, -transform.right, originalBounds.extents.x * 3, wallLayermask);
        RaycastHit2D leftBackwards = Physics2D.Raycast(leftEndPosition, -transform.right, originalBounds.extents.x * 3, wallLayermask);

        if (rightBackwards == false) {
            for (int i = 10; i > 0; --i) {
                float midToEndDistance = Vector3.Distance(rightEndPosition, centerPosition);
                float fraction = midToEndDistance / 10.0f;

                Vector3 positionDifference = centerPosition + (transform.up * (fraction * i));
                RaycastHit2D rightBackwards2 = Physics2D.Raycast(positionDifference, -transform.right, originalBounds.extents.x * 3, wallLayermask);

                if (rightBackwards2 == true) {
                    offsetDirection += -transform.up;
                    distance += Vector3.Distance(positionDifference, rightEndPosition) + 0.1f;
                    break;
                }
            }

        }

        if (leftBackwards == false) {

            for (int i = 10; i > 0; --i) {
                float midToEndDistance = Vector3.Distance(leftEndPosition, centerPosition);
                float fraction = midToEndDistance / 10.0f;

                Vector3 positionDifference = centerPosition - (transform.up * (fraction * i));
                RaycastHit2D leftBackwards2 = Physics2D.Raycast(positionDifference, -transform.right, originalBounds.extents.x * 3, wallLayermask);

                if (leftBackwards2 == true) {
                    offsetDirection += transform.up;
                    distance += Vector3.Distance(positionDifference, leftEndPosition) + 0.1f;
                    break;
                }
            }
        }
    }

    private void CheckCorners(ref Vector3 offsetDirection, ref float distance, Vector3 centerPosition, Vector3 rightEndPosition, Vector3 leftEndPosition) {
        RaycastHit2D raycastRight = Physics2D.Raycast(centerPosition, transform.up, originalBounds.extents.y, wallLayermask);
        RaycastHit2D raycastLeft = Physics2D.Raycast(centerPosition, -transform.up, originalBounds.extents.y, wallLayermask);
        if (raycastRight == true) {
            offsetDirection += -transform.up;
            //TODO: This might be werid if two corners are too tight
            distance += originalBounds.extents.y - raycastRight.fraction + 0.1f;
        }

        if (raycastLeft == true) {
            offsetDirection += transform.up;

            //TODO: This might be werid if two corners are too tight
            distance += originalBounds.extents.y - raycastLeft.fraction + 0.1f;
        }
    }

    public void SetOriginalBounds() {
        Collider2D collider = GetComponent<Collider2D>();
        originalBounds = collider.bounds;
    }

    protected abstract void OnReset();

    public abstract void ApplySettings(PortalSettings settings);

    private void Debug_VisualizeCollisionDepth() {
        Collider2D collider = GetComponent<Collider2D>();
        Vector3 centerPosition = collider.bounds.center + transform.right * originalBounds.extents.x;
        Vector3 rightEndPosition = centerPosition + transform.up * originalBounds.extents.y;
        Vector3 leftEndPosition = centerPosition + (-transform.up) * originalBounds.extents.y;

        RaycastHit2D raycastRight = Physics2D.Raycast(centerPosition, transform.up, originalBounds.extents.y, wallLayermask);
        RaycastHit2D raycastLeft = Physics2D.Raycast(centerPosition, -transform.up, originalBounds.extents.y, wallLayermask);


        if (raycastRight == true) {
            Debug.DrawLine(raycastRight.point, new Vector3(raycastRight.point.x, raycastRight.point.y) + transform.right);
            Debug.DrawLine(raycastRight.point, rightEndPosition, Color.red);
            Debug.DrawLine(new Vector3(raycastRight.point.x, raycastRight.point.y) + transform.right, rightEndPosition + transform.right, Color.green);
        }

        if (raycastLeft == true) {
            Debug.DrawLine(raycastLeft.point, new Vector3(raycastLeft.point.x, raycastLeft.point.y) + transform.right);
            Debug.DrawLine(raycastLeft.point, leftEndPosition, Color.red);
            Debug.DrawLine(new Vector3(raycastLeft.point.x, raycastLeft.point.y) + transform.right, leftEndPosition + transform.right, Color.green);
        }

        RaycastHit2D rightBackwards = Physics2D.Raycast(rightEndPosition, -transform.right, originalBounds.extents.x * 3, wallLayermask);
        RaycastHit2D leftBackwards = Physics2D.Raycast(leftEndPosition, -transform.right, originalBounds.extents.x * 3, wallLayermask);

        if (rightBackwards == false) {
            Debug.DrawLine(rightEndPosition, rightEndPosition + (-transform.right) * (originalBounds.extents.x * 3));

            for (int i = 10; i > 0; --i) {
                float midToEndDistance = Vector3.Distance(rightEndPosition, centerPosition);
                float fraction = midToEndDistance / 10.0f;

                Vector3 positionDifference = centerPosition + (transform.up * (fraction * i));
                RaycastHit2D rightBackwards2 = Physics2D.Raycast(positionDifference, -transform.right, originalBounds.extents.x * 3, wallLayermask);

                Debug.DrawLine(positionDifference, positionDifference + (-transform.right) * (originalBounds.extents.x * 3));

                if (rightBackwards2 == true) {
                    float theDistance = Vector3.Distance(positionDifference, rightEndPosition);
                    Debug.DrawLine(centerPosition, centerPosition + (transform.up * theDistance), Color.yellow);
                    break;
                }
            }

        }

        if (leftBackwards == false) {
            Debug.DrawLine(leftEndPosition, leftEndPosition + (-transform.right) * originalBounds.extents.x * 3);

            for (int i = 10; i > 0; --i) {
                float midToEndDistance = Vector3.Distance(leftEndPosition, centerPosition);
                float fraction = midToEndDistance / 10.0f;

                Vector3 positionDifference = centerPosition - (transform.up * (fraction * i));
                RaycastHit2D leftBackwards2 = Physics2D.Raycast(positionDifference, -transform.right, originalBounds.extents.x * 3, wallLayermask);

                Debug.DrawLine(positionDifference, positionDifference + (-transform.right) * (originalBounds.extents.x * 3));

                if (leftBackwards2 == true) {
                    float theDistance = Vector3.Distance(positionDifference, leftEndPosition);
                    Debug.DrawLine(centerPosition, centerPosition + (-transform.up * theDistance), Color.red);
                    break;
                }
            }
        }
    }

}

