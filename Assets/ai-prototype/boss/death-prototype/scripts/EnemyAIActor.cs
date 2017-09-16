using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using BTree;
using Steer2D;

[RequireComponent(typeof(BTreeTickBehaviour))]
[RequireComponent(typeof (Animator))]
[RequireComponent(typeof (CircleCollider2D))]
[RequireComponent(typeof (Rigidbody2D))]
[RequireComponent(typeof (Seek))]
public class EnemyAIActor : MonoBehaviour, Actor {

    public int health;
    public int maxHealth;

    public float meleeDelay = 0.1f;
    public float meleeTime = 0.1f;

    public float patrolDelay = 2.0f;
    public float rangeDelay = 0.1f;
    public float rangeTime = 0.1f;
    public float rangeParticleHeight = 1f;
    public GameObject rangeProjectile;
    public GameObject CBTprefab;
    public GameObject debugsphere;
    public GameObject[] patrolPoints;

	public float mediumRadius;
	public float smallRadius;

    public float[] weaponReloadSpeed = { 1.0f, 1.0f, 1.0f };

    public GameObject core;

    private bool facingRight;
	private Animator animator;
	private GameObject target;
    private Seek seek;
    private Image healthBar;

    private bool machineGunReady = true;
    private bool novaReady = true;
    private bool coneReady = true;
    private bool[] weaponStatus = { true, true, true };

    private int patrolPoint = 0;
    private bool patrolReady = true;

    enum AttackTypes
    {
        machinegun,
        nova,
        cone
    }

	// Use this for initialization
	void Start () {
		animator = gameObject.GetComponent<Animator>();
        seek = gameObject.GetComponent<Seek>();
        //healthBar = GameObject.Find("BossCanvas").transform.Find("HealthBG").Find("HealthFG").GetComponent<Image>();
    }


    void Update()
    {

    }



    void OnTriggerStay2D(Collider2D collider) {
		if (collider.gameObject.tag == "Player") {
			target = collider.gameObject;
        }
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.gameObject.tag == "Player") {
			target = null;
        }
	}

    public void TakeDamage(int damage)
    {
        health -= damage;
        //healthBar.fillAmount = (float)health / (float)maxHealth;
        //initCBT(damage.ToString());
    }

    //public void initCBT(string text)
    //{
    //    GameObject temp = Instantiate(CBTprefab) as GameObject;
    //    RectTransform tempRect = temp.GetComponent<RectTransform>();
    //    temp.transform.SetParent(GameObject.Find("BossCanvas").transform);
        
    //    tempRect.transform.localPosition = CBTprefab.transform.localPosition;
    //    tempRect.transform.localScale = CBTprefab.transform.localScale;
    //    tempRect.transform.localRotation = CBTprefab.transform.localRotation;

    //    temp.GetComponent<Text>().text = text;
    //    temp.GetComponent<Animator>().SetTrigger("Hit");
    //    Destroy(temp.gameObject, 2.0f);
    //}

	public bool IsTargetInRange() {
		return target != null;
	}

	public bool IsTargetInMeleeRange() {
		if (target != null && (target.transform.position - transform.position).magnitude <= mediumRadius) {
			return true;
		}
		return false;
	}

	public bool IsTargetInMeleeAttackRange() {
		if (target != null && (target.transform.position - transform.position).magnitude <= smallRadius) {
			return true;
		}
		return false;
	}

    public bool IsPatrolPointInRange(int point)
    {
        if (patrolPoints[point] && (patrolPoints[point].transform.position - transform.position).magnitude <= smallRadius)
        {
            patrolReady = false;
            return true;
        }
        return false;
    }

    public bool PatrolMoveReady()
    {
        return patrolReady;
    }

    public bool IsBombReady()
    {
        return (weaponStatus[0] || weaponStatus[1] || weaponStatus[2]) && target;
    }

    public bool IsTargetInHorizontalProjectileRange()
    {
        if (target == null)
        {
            return false;
        }
        float y = target.transform.position.y - transform.position.y;
        float offset = rangeParticleHeight / 2;
        if ((-offset < y) && (y < offset))
        {
            //For now default to false
            return false;
            //return true;
        }
        return false;
    }

	public BehaviourTree.State Idle(BehaviourTreeNode<System.Object> node) {
		animator.SetBool("idle", true);
		return BehaviourTree.State.SUCCESS;
	}

	public BehaviourTree.State Wake(BehaviourTreeNode<System.Object> node) {
		animator.SetBool("idle", false);
		return BehaviourTree.State.SUCCESS;
	}

	public BehaviourTree.State LookAtTarget(BehaviourTreeNode<System.Object> node) {
        if (target == null)
        {
            return BehaviourTree.State.SUCCESS;
        }
        setLookAtX(target.transform.position);
        return BehaviourTree.State.SUCCESS;
	}
    
    public BehaviourTree.State MoveTowardsTarget(BehaviourTreeNode<System.Object> node)
    {
        if (target == null)
        {
            //seek.TargetPoint = transform.position;
            animator.SetBool("move", false);
            return BehaviourTree.State.FAILURE;
        }
        seek.TargetPoint = target.transform.position;
        animator.SetBool("move", true);
        setLookAtX(target.transform.position);
        if (IsTargetInMeleeAttackRange())
        {
            seek.TargetPoint = transform.position;
            animator.SetBool("move", false);
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    public BehaviourTree.State MoveTowardsPoint(BehaviourTreeNode<System.Object> node)
    {
        if (patrolPoints == null)
        {
            //seek.TargetPoint = transform.position;
            animator.SetBool("move", false);
            return BehaviourTree.State.FAILURE;
        }
        seek.TargetPoint = patrolPoints[patrolPoint].transform.position;
        animator.SetBool("move", true);
        setLookAtX(patrolPoints[patrolPoint].transform.position);
        if (IsPatrolPointInRange(patrolPoint))
        {
            print("reached");
            //seek.TargetPoint = transform.position;
            animator.SetBool("move", false);
            patrolPoint++;
            if (patrolPoint > patrolPoints.Length - 1)
                patrolPoint = 0;
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    public BehaviourTree.State WaitAtPoint(BehaviourTreeNode<float> node)
    {
        node.Result += Time.deltaTime;
        if (node.Result > patrolDelay)
        {
            node.Result = 0;
            patrolReady = true;
            return BehaviourTree.State.SUCCESS;
        }
        patrolReady = false;
        return BehaviourTree.State.RUNNING;
    }

    public BehaviourTree.State PrepareMeleeAttack(BehaviourTreeNode<float> node) {
        if (target == null)
        {
            animator.SetBool("melee", false);
            animator.SetBool("prepare", false);
            return BehaviourTree.State.FAILURE;
        }
        animator.SetBool("melee", true);
        animator.SetBool("prepare", true);
        setLookAtX(target.transform.position);
        node.Result += Time.deltaTime;
        if (node.Result > meleeDelay)
        {
            node.Result = 0;
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
	}

	public BehaviourTree.State MeleeAttack(BehaviourTreeNode<float> node) {
        animator.SetBool("melee", true);
        animator.SetBool("prepare", false);
        float lookx = animator.GetFloat("lookX");
        node.Result += Time.deltaTime;
        if (node.Result > meleeTime)
        {
            if (target)
            {
                Vector2 direction = target.transform.position - transform.position;
                Collider2D[] colliders = Physics2D.OverlapCircleAll(
                    transform.position + new Vector3(0.5f * direction.x, 0.5f * direction.y, 0), 1.0f);
                //GameObject dSphere = Instantiate(debugsphere) as GameObject;
                //dSphere.transform.position = transform.position + new Vector3(0.5f * direction.x, 0.5f * direction.y, 0);
                //Destroy(dSphere, 2.0f);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject.tag == "Player")
                    {
                        //colliders[i].gameObject.GetComponent<PlayerResources>().TakeDamage(10);
                    }
                }
            }
            //Debug.Log("Muahahaha!");
            node.Result = 0;
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
	}

    public BehaviourTree.State PrepareRangeAttack(BehaviourTreeNode<float> node)
    {
        if (target == null)
        {
            animator.SetBool("range", false);
            animator.SetBool("prepare", false);
            return BehaviourTree.State.FAILURE;
        }
        animator.SetBool("range", true);
        animator.SetBool("prepare", true);
        //setLookAtX(target.transform.position);
        node.Result += Time.deltaTime;
        if (node.Result > rangeDelay)
        {
            node.Result = 0;
            return BehaviourTree.State.SUCCESS;
        }
        return BehaviourTree.State.RUNNING;
    }

    public BehaviourTree.State LaunchAttack(BehaviourTreeNode<Tuple<float, GameObject>> node)
    {
        animator.SetBool("range", true);
        animator.SetBool("prepare", false);
        if(weaponStatus[0])
        {
            GetComponent<ProjectileFire2D>().SetBullets(target);
            StartCoroutine(Reload(0));
        }
        if(weaponStatus[1])
        {
            GetComponent<HomingMissileLauncher>().Attack(target);
            GetComponent<ProjectileNova2D>().ManualFire();
            StartCoroutine(Reload(1));
        }
        if(weaponStatus[2])
        {
            GetComponent<ProjectileSpray2D>().ManualFire(target);
            StartCoroutine(Reload(2));
        }
        
        return BehaviourTree.State.SUCCESS;
    }

	public BehaviourTree.State WithdrawAttack(BehaviourTreeNode<System.Object> node) {
		animator.SetBool("prepare", false);
		animator.SetBool("melee", false);
        animator.SetBool("range", false);
		return BehaviourTree.State.SUCCESS;
	}

    private float setLookAtX(Vector3 target)
    {
        //print("Looking");
        float lookX = target.x - transform.position.x;
        facingRight = lookX > 0;
        animator.SetFloat("lookX", lookX > 0 ? 1 : -1);
        if(!facingRight)
        {
            Vector3 theScale = gameObject.transform.localScale;
            theScale.x = -1;
            gameObject.transform.localScale = theScale;
        }
        else
        {
            Vector3 theScale = gameObject.transform.localScale;
            theScale.x = 1;
            gameObject.transform.localScale = theScale;

        }
        return lookX;
    }

    private BehaviourTree.Node GetMeleeTree()
    {
        return new BinaryTreeNode(
            IsTargetInMeleeRange,
            new SequenceTreeNode(new BehaviourTree.Node[] {
                new ActionTreeNode<System.Object>(Wake),
                new ActionTreeNode<System.Object>(MoveTowardsTarget),
                new BinaryTreeNode(
                    IsTargetInMeleeAttackRange,
                    new SequenceTreeNode(new BehaviourTree.Node[] {
                        new ActionTreeNode<float>(PrepareMeleeAttack),
                        new ActionTreeNode<float>(MeleeAttack)
                    }),
                    new ActionTreeNode<System.Object>(WithdrawAttack)
                )
            }),
            new SequenceTreeNode(new BehaviourTree.Node[] {
                new ActionTreeNode<System.Object>(WithdrawAttack),
                new ActionTreeNode<System.Object>(Idle),
                new ActionTreeNode<System.Object>(node => BehaviourTree.State.FAILURE)
            })
        );
    }

    private BehaviourTree.Node GetRangeTree()
    {
        return new BinaryTreeNode(
            IsTargetInRange,
            new SequenceTreeNode(new BehaviourTree.Node[] {
                new ActionTreeNode<System.Object>(Wake),
                new BinaryTreeNode(
                    PatrolMoveReady,
                    new ActionTreeNode<System.Object>(MoveTowardsPoint),
                    new ActionTreeNode<float>(WaitAtPoint)  
                ),
                new BinaryTreeNode(
                    IsBombReady,
                    new ActionTreeNode<Tuple<float, GameObject>>(LaunchAttack),
                    new ActionTreeNode<System.Object>(WithdrawAttack))             
            }),
            new SequenceTreeNode(new BehaviourTree.Node[] {
                new ActionTreeNode<System.Object>(WithdrawAttack)
            })
        );
    }

	#region Actor implementation

	public BehaviourTree.Node GetBehaviourTree() {
        return new RepeatTreeNode(new BinaryTreeNode(
            IsTargetInRange,
            new SequenceTreeNode(new BehaviourTree.Node[] {
                //new ActionTreeNode<System.Object>(LookAtTarget),
                new SelectorTreeNode(new BehaviourTree.Node[] {
                    //GetMeleeTree(),
                    GetRangeTree()
                })
            }),
            new SequenceTreeNode(new BehaviourTree.Node[]
            {
                new ActionTreeNode<System.Object>(WithdrawAttack),
                new ActionTreeNode<System.Object>(Idle)
            })
        ));
	}

    #endregion

    public bool getFacing()
    {
        return facingRight;
    }

    IEnumerator Reload(int attackType)
    {
        weaponStatus[attackType] = false;
        yield return new WaitForSeconds(weaponReloadSpeed[attackType]);
        weaponStatus[attackType] = true;
    }
}
