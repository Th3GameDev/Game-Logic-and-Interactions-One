using UnityEngine;

public class ExploBarrel : MonoBehaviour
{
    [Range(0, 5f)][SerializeField] private float _exploRadius = 1.0f;
    [Range(0, 1000f)][SerializeField] private float _exploForce;

    //Max Number of explo enemy array size at once 
    private int _maxHits = 5;

    [Range(0, 5f)][SerializeField] private float _offset = 1.0f;

    private Collider[] _collidersHits;

    [SerializeField] private GameObject _brokeBarrelPrefab;

    [SerializeField] private ParticleSystem _exploParticle;

    void Awake()
    {
        //Set array size
        _collidersHits = new Collider[_maxHits];
    }

    public void Explode()
    {
        this.gameObject.SetActive(false);

        Instantiate(_exploParticle, new Vector3(transform.position.x, transform.position.y + _offset, transform.position.z), Quaternion.identity);

        ExplosionDamage(transform.position, _exploRadius);

        //Spawn Broken Barrel and grab the childrens rigidbodys and apply force
        GameObject barrel = Instantiate(_brokeBarrelPrefab, transform.position, Quaternion.identity);

        Rigidbody[] rigidbodies = barrel.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rigidbodies)
        {         
            rb.AddExplosionForce(_exploForce, this.transform.position, _exploRadius, 5f, ForceMode.Impulse);
            Destroy(barrel, 5f);
        }

        Destroy(this.gameObject, 1.5f);
    }

    void ExplosionDamage(Vector3 center, float radius)
    {
        int numColliders = Physics.OverlapSphereNonAlloc(center, radius, _collidersHits, 1 << 8);

        for (int i = 0; i < numColliders; i++)
        {
            BasicAI enemy = _collidersHits[i].GetComponent<BasicAI>();

            if (enemy != null)
            {
                enemy.Damage(100);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + _offset, transform.position.z);
        Gizmos.DrawWireSphere(pos, _exploRadius);
    }
}

