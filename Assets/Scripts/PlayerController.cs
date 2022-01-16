using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask solidObjectsLayer;
    Animator animator;
    bool isMoving;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isMoving == false)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            if (x != 0)
            {
                y = 0;
            }

            if (x != 0 || y != 0)
            {
                animator.SetFloat("InputX", x);
                animator.SetFloat("InputY", y);

                StartCoroutine(Move(new Vector2(x, y)));
            }
        }
        animator.SetBool("IsMoving", isMoving);
    }

    IEnumerator Move(Vector3 direction)
    {
        isMoving = true;
        Vector3 targetPos = transform.position + direction;
        if (IsWalkable(targetPos) == false)
        {
            isMoving = false;
            yield break;
        }
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 5f*Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;

        CheckForEncounts();
    }

    void CheckForEncounts()
    {
        if (Random.Range(0,100) < 10)
        {
            Debug.Log("!");
        }   
    }

    bool IsWalkable(Vector3 targetPos)
    {
        return Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) == false;
    }
}
