using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private float speed = 2.5f;
    public Material blue;
    public Material green;
    public Material red;
    public Material yellow;

    public GameObject blueGem;
    public GameObject greenGem;
    public GameObject redGem;
    public GameObject yellowGem;

    public Transform tower;

    private bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        tower = GameObject.Find("WizardTowerPillar").transform;
        transform.forward = tower.transform.position - transform.position;
        transform.forward = new Vector3(transform.forward.x, 0, transform.forward.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, tower.position, speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "WizardTowerPillar" && !isDead)
        {
            tower.gameObject.GetComponent<TowerLife>().TakeDamage();
            Destroy(gameObject);
            isDead = true;
        }
    }

    public void Initialize(int height, WizardType color)
    {
        GameObject childObject = transform.GetChild(height).gameObject;
        childObject.SetActive(true);
        SetColor(childObject, color);
    }

    public void SetColor(GameObject childObject, WizardType color)
    {
        EnemyModel model = childObject.GetComponentInChildren<EnemyModel>();
        switch (color)
        {
            case WizardType.blueSquare:
                //blue
                foreach (MeshRenderer renderer in model.colorMeshes)
                    renderer.material = blue;
                Instantiate(blueGem, model.gemLocation);
                childObject.tag = "Blue";
                break;
            case WizardType.greenDiamond:
                //green
                foreach (MeshRenderer renderer in model.colorMeshes)
                    renderer.material = green;
                Instantiate(greenGem, model.gemLocation);
                childObject.tag = "Green";
                break;
            case WizardType.redCone:
                //red
                foreach (MeshRenderer renderer in model.colorMeshes)
                    renderer.material = red;
                Instantiate(redGem, model.gemLocation);
                childObject.tag = "Red";
                break;
            case WizardType.yellowRound:
                //yellow
                foreach (MeshRenderer renderer in model.colorMeshes)
                    renderer.material = yellow;
                Instantiate(yellowGem, model.gemLocation);
                childObject.tag = "Yellow";
                break;
        }
    }
}
