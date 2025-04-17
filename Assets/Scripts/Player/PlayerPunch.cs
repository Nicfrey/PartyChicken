using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    private PlayerStatistics playerStatistics;
    [SerializeField] private int punchDamage = 15;
    [SerializeField] private Transform avatar;
    private Animator animator;
    private bool isActive = true;

    private void Start()
    {
        playerStatistics = GetComponentInParent<PlayerStatistics>();
        animator = GetComponentInParent<Animator>();
        Invoke(nameof(SetLayerMask),0.5f);
    }

    public void SetLayerMask()
    {
        GetComponent<Collider>().excludeLayers = 1 << playerStatistics.gameObject.layer;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive)
            return;
        
        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(1);
        if (!animatorStateInfo.IsName("Punch"))
            return;
        if (other.CompareTag("Player"))
        {
            Target target = other.GetComponent<Target>();
            target.TakeDamage(punchDamage,playerStatistics);
            other.GetComponent<Rigidbody>().AddForce(avatar.forward * 3f, ForceMode.Impulse);
            isActive = false;
            Invoke(nameof(SetActive),0.5f);
        }
    }

    private void SetActive()
    {
        isActive = true;
    }
}
