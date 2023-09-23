using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class TestBarrel : MonoBehaviour
{
    [Range(0, 5f)][SerializeField] private float _exploRadius = 1.0f;
    [Range(0, 1000f)][SerializeField] private float _exploForce;

    private int _maxHits = 20;
    private int _maxExploDamage = 100;
    private int _minExploDamage = 50;

    [Range(0, 5f)][SerializeField] private float _offset = 1.0f;

    private Collider[] _collidersHits;

    [SerializeField] private GameObject _brokeBarrelPrefab;

    [SerializeField] private ParticleSystem _exploParticle;

    void Awake()
    {
        _collidersHits = new Collider[_maxHits];
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Explode()
    {

        Debug.Log("Hello");
        this.gameObject.SetActive(false);

        Instantiate(_exploParticle, new Vector3(0, 1f, 0), Quaternion.identity);

        ExplosionDamage(transform.position, _exploRadius);

        GameObject barrel = Instantiate(_brokeBarrelPrefab, transform.position, Quaternion.identity);

        Rigidbody[] rigidbodies = barrel.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rigidbodies)
        {
            Vector3 pos = new Vector3(0.37f, 1f, 4.09f);
            rb.AddExplosionForce(_exploForce, pos, _exploRadius, 5f, ForceMode.Impulse);
            Destroy(barrel, 5f);
        }



        Destroy(this.gameObject);
    }

    void ExplosionDamage(Vector3 center, float radius)
    {
        int numColliders = Physics.OverlapSphereNonAlloc(center, radius, _collidersHits, 1 << 8);

        for (int i = 0; i < numColliders; i++)
        {

            BasicAI enemy = _collidersHits[i].GetComponent<BasicAI>();

            if (enemy != null)
                enemy.Damage(50);


            Rigidbody rb = _collidersHits[i].GetComponent<Rigidbody>();
            rb.AddExplosionForce(_exploForce, transform.position, _exploRadius, 5f, ForceMode.Impulse);

            Debug.Log($"Hit: {_collidersHits[i].name}");
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + _offset, transform.position.z);
        Gizmos.DrawWireSphere(pos, _exploRadius);
    }
}
