using UnityEngine;

public class BlockMovements : MonoBehaviour
{
    GameManager gameManager;
    SpriteRenderer zone;

    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        zone = gameManager.zone.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        DrawBlock();
    }

    void DrawBlock()
    {
        if(name == "Rook(Clone)")
        {
            AttackPath(transform.right);
            AttackPath(-transform.right);
            AttackPath(transform.up);
            AttackPath(-transform.up);
            GameManager gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            gameManager.totalScore += gameManager.rookScore;
            gameManager.rookCount++;
        }
        else if(name == "Bishop(Clone)")
        {
            AttackPath(transform.up + transform.right);
            AttackPath(transform.up - transform.right);
            AttackPath(-transform.up + transform.right);
            AttackPath(-transform.up - transform.right);
            GameManager gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            gameManager.totalScore += gameManager.bishopScore;
            gameManager.bishopCount++;
        }
        else if(name == "Knight(Clone)")
        {
            AttackPath(transform.position + new Vector3(2, 1));
            AttackPath(transform.position + new Vector3(2, -1));
            AttackPath(transform.position + new Vector3(-2, 1));
            AttackPath(transform.position + new Vector3(-2, -1));
            AttackPath(transform.position + new Vector3(1, 2));
            AttackPath(transform.position + new Vector3(1, -2));
            AttackPath(transform.position + new Vector3(-1, 2));
            AttackPath(transform.position + new Vector3(-1, -2));
            GameManager gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            gameManager.totalScore += gameManager.knightScore;
            gameManager.knightCount++;
        }
        else if(name == "Dragon(Clone)")
        {
            AttackPath(transform.position + new Vector3(1, 0));
            AttackPath(transform.position + new Vector3(-1, 0));
            AttackPath(transform.position + new Vector3(0, 1));
            AttackPath(transform.position + new Vector3(0, -1));
            AttackPath(transform.position + new Vector3(1, 1));
            AttackPath(transform.position + new Vector3(1, -1));
            AttackPath(transform.position + new Vector3(-1, 1));
            AttackPath(transform.position + new Vector3(-1, -1));
            GameManager gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            gameManager.totalScore += gameManager.dragonScore;
            gameManager.dragonCount++;
        }
    }

    void AttackPath(Vector2 path)
    {
        if (gameObject.name == "Knight(Clone)" || gameObject.name == "Dragon(Clone)")
        {
            RaycastHit2D hit = Physics2D.Raycast(path, Vector2.zero);
            if (hit.transform != null && hit.transform.childCount != 0)
            {
                gameManager.gameOver = true;
            }

            SpriteRenderer spriteRenderer = hit.collider?.gameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = zone.sprite;
                spriteRenderer.sortingOrder = -1;
                Color spriteColor = spriteRenderer.color;
                spriteColor.a = 1f;
                spriteRenderer.color = spriteColor;
            }
        }
        else
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, path, Mathf.Infinity);
            bool firstTouch = true;

            foreach (RaycastHit2D hit in hits)
            {
                if (firstTouch)
                {
                    firstTouch = false;
                    continue;
                }

                if (hit.transform.childCount != 0)
                {
                    gameManager.gameOver = true;
                }

                SpriteRenderer spriteRenderer = hit.collider?.gameObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sprite = zone.sprite;
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
}
