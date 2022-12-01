using System.Collections;
using UnityEngine;

public class MonstersPet : MonstersInteractionWay
{
    private MonsterBehaviour _monsterBehaviour;
    [SerializeField] private GameObject _heartPrefab;
    private SpriteRenderer _heart;

    private void Awake()
    {
        _monsterBehaviour = GetComponentInParent<MonsterBehaviour>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_heart)
        {
            
            _heart.transform.position += Vector3.up * 12 * Time.deltaTime;
            //_heart.color.a -= Color.white.a
        }

    }

    public override void Execute()
    {
        var monster = _monsterBehaviour.Monster.gameObject.transform;
        _heart = Instantiate(_heartPrefab, monster.position + new Vector3(0, 1), Quaternion.identity, monster).GetComponent<SpriteRenderer>();

        Invoke(nameof(DestroyHeart), 4f);

        gameObject.SetActive(false);
    }

    public override bool GetDisplayCondition()
    {
        return true;
    }

    private void DestroyHeart()
    {
        Destroy(_heart.gameObject);
    }
}
