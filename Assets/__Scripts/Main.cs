using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Set in inpector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f;
    public WeaponDefinition[] weaponDefinitions;
    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequency = new WeaponType[]
    {
        WeaponType.laser,
        WeaponType.laser, 
        WeaponType.blaster, 
        WeaponType.shield 
    };

    private BoundsCheck bndCheck;
    
    private void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);                     //a
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (var def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
    }

    public void SpawnEnemy()
    {
        int ndx = Random.Range(0, prefabEnemies.Length);                    //b

        GameObject go = Instantiate(prefabEnemies[ndx]);                    //c

        float enemyPadding = enemyDefaultPadding;                           //d
        if (go.GetComponent<BoundsCheck>() != null)                         //e
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        Vector3 pos = Vector3.zero;                                         //f
        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;

        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);                     //g
    }

    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }

    public void Restart()
    {
        SceneManager.LoadScene("_Scene_0");
    }

    /// <summary>
    /// Статическая функция, возвращающая из статического защищенного поля WEAP_DICT класса Main
    /// </summary>
    /// <param name="wt"> Тип оружия WeaponType, для которого требуется получить WeaponDefinition</param>
    /// <returns> Экземпляр WeaponDefinition или, если нет такого определения
    /// для указанного WeaponType возвращает новый экземпляр WeaponDefinition
    /// c типом none.</returns>
    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        // Проверить наличие указанного ключа в словаре
        if (WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);
        }
        return (new WeaponDefinition());
    }

    public void ShipDestroyed(Enemy e)
    {
        //Сгенерировать бонус с заданной вероятностью
        if (Random.value <= e.powerUpDropChance)
        {
            //Выбрать тип бонуса
            //Выбрать один из элементов в powerUpFrequencty
            int ndx = Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[ndx];

            //Создать экземпляр PowerUp
            GameObject go = Instantiate(prefabPowerUp);
            PowerUp pu = go.GetComponent<PowerUp>();

            //Установить соостветствующий тип WeaponType
            pu.SetType(puType);

            //Поместить его в место, где находился разрушенный корабль
            pu.transform.position = e.transform.position;
        }
        if (e.gameObject.name == "Enemy_0(Clone)")
        {
            ScoreCounter.score += 100;
        }
        if (e.gameObject.name == "Enemy_1(Clone)")
        {
            ScoreCounter.score += 200;
        }
        if (e.gameObject.name == "Enemy_2(Clone)")
        {
            ScoreCounter.score += 200;
        }
        if (e.gameObject.name == "Enemy_3(Clone)")
        {
            ScoreCounter.score += 300;
        }
        if (e.gameObject.name == "Enemy_4(Clone)")
        {
            ScoreCounter.score += 500;
        }
    }
}
