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
public class EnemyAIActor : MonoBehaviour, Actor
{
    public float patrolDelay = 2.0f;

    public GameObject[] patrolPoints;
    public GameObject[] turrets;
    public GameObject stageThreePoint;


    public float meleeRadius;
	public float smallRadius;
    public bool shouldTurn;

    public float[] weaponReloadSpeed = { 1.0f, 1.0f, 1.0f };
    public float[] stageThreeReloadSpeed = { 6.0f, 6.0f, 6.0f };

    private bool facingRight;
	private Animator animator;
	private GameObject target;
    private Seek seek;
    private Image healthBar;
    private UnitResources res;
    private AudioSource m_Audio;

    private bool[] weaponStatus = { true, false, false };
    private bool[] weaponStatusStageThree = { true, true, true };

    private int patrolPoint = 0;
    private bool patrolReady = true;
    private bool stageThreeReady = false;

    bool bumped = false;
    bool bossDead = false;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
        seek = GetComponent<Seek>();
        res = GetComponent<UnitResources>();
        m_Audio = GetComponent<AudioSource>();
    }


    void Update()
    {
        if(res.health <= 0 && !bossDead)
        {  
            bossDead = true;
            m_Audio.Play(0);
            GameControl.control.PlayerVictory();
        }
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


	public bool IsTargetInRange() {
		return target != null;
	}

	public bool IsTargetInMeleeRange() {
		if (target != null && (target.transform.position - transform.position).magnitude <= meleeRadius) {
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

    public bool IsStageThreeInRange()
    {
        
        if (stageThreePoint != null && (stageThreePoint.transform.position - transform.position).magnitude <= smallRadius)
        {
            print(true);
            return true;
        }
        print(false);
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

    public bool IsStageWeaponReady()
    {
        return (weaponStatusStageThree[0] || weaponStatusStageThree[1] || weaponStatusStageThree[2]) && target;
    }

    public bool AreTurretsDead()
    {
        foreach(GameObject turr in turrets)
        {
            if (turr)
            {
                gameObject.GetComponent<SteeringAgent>().MaxVelocity = 0;
                return true;
            }
                
        }
        gameObject.GetComponent<SteeringAgent>().MaxVelocity = 3;
        return false;
    }

    public bool StageThreeMode()
    {
        return !stageThreeReady;
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
        if (stageThreePoint == null)
        {
            animator.SetBool("move", false);
            return BehaviourTree.State.FAILURE;
        }
        seek.TargetPoint = stageThreePoint.transform.position;
        animator.SetBool("move", true);
        if (IsStageThreeInRange())
        {
            seek.TargetPoint = transform.position;
            animator.SetBool("move", false);
            GetComponent<CapsuleCollider2D>().enabled = true;

            //Prevent player from geting stuck behind boss
            if(IsTargetInMeleeRange() && !bumped)
            {
                target.transform.position = transform.position + new Vector3(5,0,0);  
            }
            bumped = true;
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

    public BehaviourTree.State LaunchAttack(BehaviourTreeNode<Tuple<float, GameObject>> node)
    {
        animator.SetBool("range", true);
        animator.SetBool("prepare", false);
        if(weaponStatus[0])
        {
            GetComponent<ProjectileFire2D>().SetBullets(target);
            StartCoroutine(Reload(weaponStatus,weaponReloadSpeed, 0));
        }
        if(weaponStatus[1])
        {
            //GetComponent<HomingMissileLauncher>().Attack(target);
            GetComponent<ProjectileNova2D>().ManualFire();
            StartCoroutine(Reload(weaponStatus,weaponReloadSpeed, 1));
        }
        if(weaponStatus[2])
        {
            GetComponent<ProjectileSpray2D>().ManualFire(target);
            StartCoroutine(Reload(weaponStatus,weaponReloadSpeed, 2));
        }
        
        return BehaviourTree.State.SUCCESS;
    }

    public BehaviourTree.State LaunchStageAttack(BehaviourTreeNode<Tuple<float, GameObject>> node)
    {
        animator.SetBool("range", true);
        animator.SetBool("prepare", false);
        if (weaponStatusStageThree[0])
        {
            GetComponent<SpawnUnderPlayer>().SetAttributes(target, transform.position.y, 2);
            StartCoroutine(Reload(weaponStatusStageThree,stageThreeReloadSpeed, 0));
        }
        if (weaponStatusStageThree[1])
        {
            GetComponent<SpawnUnderPlayer>().ShadowAttack(gameObject, 20);
            StartCoroutine(Reload(weaponStatusStageThree,stageThreeReloadSpeed, 1));
        }
        if (weaponStatusStageThree[2])
        {
            
            StartCoroutine(Reload(weaponStatusStageThree,stageThreeReloadSpeed, 2));
        }

        return BehaviourTree.State.SUCCESS;
    }

    public BehaviourTree.State WithdrawAttack(BehaviourTreeNode<System.Object> node) {
		animator.SetBool("prepare", false);
		animator.SetBool("melee", false);
        animator.SetBool("range", false);
		return BehaviourTree.State.SUCCESS;
	}

    public BehaviourTree.State PrepareWeapons(BehaviourTreeNode<System.Object> node)
    {
        weaponStatus = new bool[] { true, false, false};
        return BehaviourTree.State.SUCCESS;
    }

    private float setLookAtX(Vector3 target)
    {
        
        float lookX = target.x - transform.position.x;
        facingRight = lookX > 0;
        animator.SetFloat("lookX", lookX > 0 ? 1 : -1);
        if(shouldTurn)
        {
            if (!facingRight)
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
        }

        return lookX;
    }

    private BehaviourTree.Node GetStageOneTree()
    {
        return new BinaryTreeNode(
            AreTurretsDead,
            new SequenceTreeNode(new BehaviourTree.Node[] {
                new BinaryTreeNode(
                    IsBombReady,
                    new ActionTreeNode<Tuple<float, GameObject>>(LaunchAttack),
                    new ActionTreeNode<System.Object>(WithdrawAttack)
                )
            }),
            new ActionTreeNode<System.Object>(node => BehaviourTree.State.FAILURE)  
        );
    }

    private BehaviourTree.Node GetStageTwoTree()
    {
        return new BinaryTreeNode(
            StageThreeMode,
            new BinaryTreeNode(
                IsTargetInRange,
                new SequenceTreeNode(new BehaviourTree.Node[] {
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
                new ActionTreeNode<System.Object>(WithdrawAttack)
            ),
            new ActionTreeNode<System.Object>(node => BehaviourTree.State.FAILURE)
        );
    }

    private BehaviourTree.Node GetStageThreeTree()
    {
        return new BinaryTreeNode(
            IsStageThreeInRange,
            new SequenceTreeNode(new BehaviourTree.Node[] {
                new BinaryTreeNode(
                    IsStageWeaponReady,
                    new ActionTreeNode<Tuple<float, GameObject>>(LaunchStageAttack),
                    new ActionTreeNode<System.Object>(WithdrawAttack)
                )
            }),
            new ActionTreeNode<System.Object>(MoveTowardsTarget)
        );
    }

    #region Actor implementation

    public BehaviourTree.Node GetBehaviourTree() {
        return new RepeatTreeNode(new BinaryTreeNode(
            IsTargetInRange,
            new SequenceTreeNode(new BehaviourTree.Node[] {
                new SelectorTreeNode(new BehaviourTree.Node[] {
                    GetStageOneTree(),
                    GetStageTwoTree(),
                    GetStageThreeTree()
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

    IEnumerator Reload(bool[] weaponInventory, float[] weaponSpeedType, int attackType)
    {
        weaponInventory[attackType] = false;
        yield return new WaitForSeconds(weaponSpeedType[attackType]);
        weaponInventory[attackType] = true;
    }

    public void SetWeaponStates(bool gun, bool nova, bool spray)
    {
        weaponStatus = new bool[] { gun, nova, spray };
    }

    public void SetStageThree()
    {
        weaponStatus = new bool[] { false, false, false };
        stageThreeReady = true;
    }
}
