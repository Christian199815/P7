﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RopeSystem : MonoBehaviour
{
    public GameObject ropeHingeAnchor;
    public DistanceJoint2D ropeJoint;
    public Transform crosshair;
    public SpriteRenderer crosshairSprite;
    //pm is niek zn movement script
    public Player PM;
    private bool ropeAttached;
    private Vector2 playerPosition;
    private Rigidbody2D ropeHingeAnchorRb;
    private SpriteRenderer ropeHingeAnchorSprite;
    private InputManager iMan;

    public LineRenderer ropeRenderer;
    public LayerMask ropeLayerMask;
    private float ropeMaxCastDistance = 20f;
    private List<Vector2> ropePositions = new List<Vector2>();

    private bool distanceSet;
    private Dictionary<Vector2, int> wrapPointsLookup = new Dictionary<Vector2, int>();

    public float climbSpeed = 3f;
    private bool isColliding;

    private void Start()
    {
        iMan = FindObjectOfType<InputManager>();
    }

    private void Awake()
    {
        ropeJoint.enabled = false;
        playerPosition = transform.position;
        ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D>();
        ropeHingeAnchorSprite = ropeHingeAnchor.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //mouse pos wordt schiet kant, shooting script niek
        var worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var facingDirection = worldMousePosition - transform.position;
        var aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        if(aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }

        var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
        playerPosition = transform.position;

        
        if (!ropeAttached)
        {
            SetCrosshairPosition(aimAngle);
            PM.isSwinging = false;
        }
        
        else
        {
            PM.isSwinging = true;
            PM.ropeHook = ropePositions.Last();
            crosshairSprite.enabled = false;
            if(ropePositions.Count > 0)
            {
                var lastRopePoint = ropePositions.Last();
                var playerToCurrentNextHit = Physics2D.Raycast(playerPosition, (lastRopePoint - playerPosition).normalized, Vector2.Distance(playerPosition, lastRopePoint) - 0.1f, ropeLayerMask);
                if (playerToCurrentNextHit)
                {
                    var colliderWithVertices = playerToCurrentNextHit.collider as PolygonCollider2D;
                    if(colliderWithVertices != null)
                    {
                        var closestPointToHit = GetClosesColliderPointFromRaycastHit(playerToCurrentNextHit, colliderWithVertices);

                        if (wrapPointsLookup.ContainsKey(closestPointToHit))
                        {
                            ResetRope();
                            return;
                        }

                        ropePositions.Add(closestPointToHit);
                        wrapPointsLookup.Add(closestPointToHit, 0);
                        distanceSet = false;
                    }
                }

            }
        }

        HandleInput(aimDirection);
        UpdateRopePositions();
        HandleRopeLength();
    }

    private void SetCrosshairPosition(float aimAngle)
    {
        if (!crosshairSprite.enabled)
        {
            crosshairSprite.enabled = true;
        }

        var x = transform.position.x + 1f * Mathf.Cos(aimAngle);
        var y = transform.position.y + 1f * Mathf.Sin(aimAngle);

        var crossHairPosition = new Vector3(x, y, 0);
        crosshair.transform.position = crossHairPosition;
    }

    private void HandleInput(Vector2 aimDirection)
    {
        //veranderen naar de knop in de mobile controller, niek shooting script
        if (Input.GetMouseButton(0))
        {
            if (ropeAttached) return;
            ropeRenderer.enabled = true;

            var hit = Physics2D.Raycast(playerPosition, aimDirection, ropeMaxCastDistance, ropeLayerMask);

            if(hit.collider != null)
            {
                ropeAttached = true;
                if (!ropePositions.Contains(hit.point))
                {
                    transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);
                    ropePositions.Add(hit.point);
                    ropeJoint.distance = Vector2.Distance(playerPosition, hit.point);
                    ropeJoint.enabled = true;
                    ropeHingeAnchorSprite.enabled = true;
                }
            }
            else
            {
                ropeRenderer.enabled = false;
                ropeAttached = false;
                ropeJoint.enabled = false;
            }
        }

        // hetzelfde maar dan anders
        if (Input.GetMouseButton(1))
        {
            ResetRope();
        }
    }

    private void ResetRope()
    {
        ropeJoint.enabled = false;
        ropeAttached = false;
        PM.isSwinging = false;
        ropeRenderer.positionCount = 2;
        ropeRenderer.SetPosition(0, transform.position);
        ropeRenderer.SetPosition(1, transform.position);
        ropePositions.Clear();
        ropeHingeAnchorSprite.enabled = false;
        wrapPointsLookup.Clear();
    }

    private void UpdateRopePositions()
    {
        if (!ropeAttached)
        {
            return;
        }

        ropeRenderer.positionCount = ropePositions.Count + 1;

        for(var i = ropeRenderer.positionCount - 1; i >= 0; i--)
        {
            if (i != ropeRenderer.positionCount - 1)
            {
                ropeRenderer.SetPosition(i, ropePositions[i]);

                if(i == ropePositions.Count -1 || ropePositions.Count == 1)
                {
                    var ropePosition = ropePositions[ropePositions.Count - 1];
                    if(ropePositions.Count == 1)
                    {
                        ropeHingeAnchorRb.transform.position = ropePosition;
                        if (!distanceSet)
                        {
                            ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                            distanceSet = true;
                        }
                    }
                    else
                    {
                        ropeHingeAnchorRb.transform.position = ropePosition;
                        if (!distanceSet)
                        {
                            ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                            distanceSet = true;
                        }
                    }
                }
                else if(i -1 == ropePositions.IndexOf(ropePositions.Last()))
                {
                    var ropePosition = ropePositions.Last();
                    ropeHingeAnchorRb.transform.position = ropePosition;
                    if (!distanceSet)
                    {
                        ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                        distanceSet = true;
                    }
                }
            }
            else
            {
                ropeRenderer.SetPosition(i, transform.position);
            }
        }
    }

    private Vector2 GetClosesColliderPointFromRaycastHit(RaycastHit2D hit, PolygonCollider2D polyCollider)
    {
        var distanceDictionary = polyCollider.points.ToDictionary<Vector2, float, Vector2>(position => Vector2.Distance(hit.point, polyCollider.transform.TransformPoint(position)), position => polyCollider.transform.TransformPoint(position));
        var orderedDictionary = distanceDictionary.OrderBy(e => e.Key);
        return orderedDictionary.Any() ? orderedDictionary.First().Value : Vector2.zero;
    }

    private void HandleRopeLength()
    {
        
        //niek zn vertical inputmanager
        if(iMan.axis.y >= 1f && ropeAttached && !isColliding)
        {
            ropeJoint.distance -= Time.deltaTime * climbSpeed;
        }
        else if(iMan.axis.y < 0f && ropeAttached)
        {
            ropeJoint.distance += Time.deltaTime * climbSpeed;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
    }
}
