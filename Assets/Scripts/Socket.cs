using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Socket : MonoBehaviour
{
    GameManager gameManager;
    SpriteRenderer spriteRenderer;
    Color mainColor;
    public GameObject attackPath;

    public LayerMask targetLayerMask;

    public bool full;
    private bool isMouseDown = false;
    private bool isInside = false;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainColor = spriteRenderer.color;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        full = false;
    }

    

    private void OnMouseEnter()
    {
        spriteRenderer.color = gameManager.zoneColor;
        isInside = true;
    }

    private void OnMouseExit()
    {
        spriteRenderer.color = mainColor;
        isInside = false;

        if (isMouseDown)
        {
            ResetOnMouseDown();
        }
    }

    private void OnMouseDown()
    {
        isMouseDown = true;

        if (!gameManager.gameOver && gameManager.pathAssist)
        {
            if (gameManager.indexPrefabs == 0)
            {
                AttackPath(transform.up + transform.right);
                AttackPath(transform.up - transform.right);
                AttackPath(-transform.up + transform.right);
                AttackPath(-transform.up - transform.right);
            }
            else if (gameManager.indexPrefabs == 1)
            {
                AttackPath(transform.position + new Vector3(1, 0));
                AttackPath(transform.position + new Vector3(-1, 0));
                AttackPath(transform.position + new Vector3(0, 1));
                AttackPath(transform.position + new Vector3(0, -1));
                AttackPath(transform.position + new Vector3(1, 1));
                AttackPath(transform.position + new Vector3(1, -1));
                AttackPath(transform.position + new Vector3(-1, 1));
                AttackPath(transform.position + new Vector3(-1, -1));
            }
            else if (gameManager.indexPrefabs == 2)
            {
                AttackPath(transform.position + new Vector3(2, 1));
                AttackPath(transform.position + new Vector3(2, -1));
                AttackPath(transform.position + new Vector3(-2, 1));
                AttackPath(transform.position + new Vector3(-2, -1));
                AttackPath(transform.position + new Vector3(1, 2));
                AttackPath(transform.position + new Vector3(1, -2));
                AttackPath(transform.position + new Vector3(-1, 2));
                AttackPath(transform.position + new Vector3(-1, -2));
            }
            else if (gameManager.indexPrefabs == 3)
            {
                AttackPath(transform.right);
                AttackPath(-transform.right);
                AttackPath(transform.up);
                AttackPath(-transform.up);
            }
        }
    }

    private void OnMouseUp()
    {
        isMouseDown = false;

        if (!gameManager.gameOver && isInside)
        {
            // Deploy block if zone is empty
            if (transform.childCount == 0)
            {
                Instantiate(gameManager.blocks[gameManager.indexPrefabs], transform);
                gameManager.deployed = false;
                gameManager.timerRunning = true;
                gameManager.audioSource.clip = gameManager.audioClips[0];
                gameManager.audioSource.Play();
            }
        }
    }

    void AttackPath(Vector2 path)
    {
        if (gameManager.indexPrefabs == 1 || gameManager.indexPrefabs == 2)
        {
            RaycastHit2D hit = Physics2D.Raycast(path, Vector2.zero, Mathf.Infinity, targetLayerMask);
            if (hit.collider != null)
            {
                SpriteRenderer spriteRenderer = hit.collider.GetComponent<SpriteRenderer>();
                SpriteRenderer attackSpriteRenderer = attackPath.gameObject.GetComponent<SpriteRenderer>();

                if (spriteRenderer != null && attackSpriteRenderer != null)
                {
                    spriteRenderer.sprite = attackSpriteRenderer.sprite;
                    spriteRenderer.sortingOrder = 1;

                    Color spriteColor = spriteRenderer.color;
                    spriteColor.a = 0.5f;
                    spriteRenderer.color = spriteColor;
                }
            }
        }
        else
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, path, Mathf.Infinity, targetLayerMask);
            bool firstTouch = true;

            foreach (var hit in hits)
            {
                if (firstTouch)
                {
                    firstTouch = false;
                    continue;
                }

                SpriteRenderer spriteRenderer = hit.collider.GetComponent<SpriteRenderer>();
                SpriteRenderer attackSpriteRenderer = attackPath.gameObject.GetComponent<SpriteRenderer>();

                if (spriteRenderer != null && attackSpriteRenderer != null)
                {
                    spriteRenderer.sprite = attackSpriteRenderer.sprite;
                    spriteRenderer.sortingOrder = 1;

                    Color spriteColor = spriteRenderer.color;
                    spriteColor.a = 0.5f;
                    spriteRenderer.color = spriteColor;
                }

                if (hit.collider == null)
                {
                    break;
                }
            }
        }
    }

    void CancelAttackPath(Vector2 path)
    {
        if (gameManager.indexPrefabs == 1 || gameManager.indexPrefabs == 2)
        {
            RaycastHit2D hit = Physics2D.Raycast(path, Vector2.zero, Mathf.Infinity, targetLayerMask);
            if (hit.collider != null)
            {
                SpriteRenderer spriteRenderer = hit.collider.GetComponent<SpriteRenderer>();
                SpriteRenderer attackSpriteRenderer = gameManager.zone.gameObject.GetComponent<SpriteRenderer>();

                if (spriteRenderer != null && attackSpriteRenderer != null)
                {
                    spriteRenderer.sprite = attackSpriteRenderer.sprite;
                    spriteRenderer.sortingOrder = -1;

                    Color spriteColor = spriteRenderer.color;
                    spriteColor.a = 1f;
                    spriteRenderer.color = spriteColor;
                }
            }
        }
        else
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, path, Mathf.Infinity, targetLayerMask);
            bool firstTouch = true;

            foreach (var hit in hits)
            {
                if (firstTouch)
                {
                    firstTouch = false;
                    continue;
                }

                SpriteRenderer spriteRenderer = hit.collider.GetComponent<SpriteRenderer>();
                SpriteRenderer attackSpriteRenderer = gameManager.zone.gameObject.GetComponent<SpriteRenderer>();

                if (spriteRenderer != null && attackSpriteRenderer != null)
                {
                    spriteRenderer.sprite = attackSpriteRenderer.sprite;
                    spriteRenderer.sortingOrder = -1;

                    Color spriteColor = spriteRenderer.color;
                    spriteColor.a = 1f;
                    spriteRenderer.color = spriteColor;
                }

                if (hit.collider == null)
                {
                    break;
                }
            }
        }
    }

    void ResetOnMouseDown()
    {
        if (!gameManager.gameOver)
        {
            if (gameManager.indexPrefabs == 0)
            {
                CancelAttackPath(transform.up + transform.right);
                CancelAttackPath(transform.up - transform.right);
                CancelAttackPath(-transform.up + transform.right);
                CancelAttackPath(-transform.up - transform.right);
            }
            else if (gameManager.indexPrefabs == 1)
            {
                CancelAttackPath(transform.position + new Vector3(1, 0));
                CancelAttackPath(transform.position + new Vector3(-1, 0));
                CancelAttackPath(transform.position + new Vector3(0, 1));
                CancelAttackPath(transform.position + new Vector3(0, -1));
                CancelAttackPath(transform.position + new Vector3(1, 1));
                CancelAttackPath(transform.position + new Vector3(1, -1));
                CancelAttackPath(transform.position + new Vector3(-1, 1));
                CancelAttackPath(transform.position + new Vector3(-1, -1));
            }
            else if (gameManager.indexPrefabs == 2)
            {
                CancelAttackPath(transform.position + new Vector3(2, 1));
                CancelAttackPath(transform.position + new Vector3(2, -1));
                CancelAttackPath(transform.position + new Vector3(-2, 1));
                CancelAttackPath(transform.position + new Vector3(-2, -1));
                CancelAttackPath(transform.position + new Vector3(1, 2));
                CancelAttackPath(transform.position + new Vector3(1, -2));
                CancelAttackPath(transform.position + new Vector3(-1, 2));
                CancelAttackPath(transform.position + new Vector3(-1, -2));
            }
            else if (gameManager.indexPrefabs == 3)
            {
                CancelAttackPath(transform.right);
                CancelAttackPath(-transform.right);
                CancelAttackPath(transform.up);
                CancelAttackPath(-transform.up);
            }
        }
    }

}
