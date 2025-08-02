using UnityEngine;
using System.Collections.Generic;
using TheSentinel.Skills;
using TMPro;
using UnityEngine.UI;

namespace TheSentinel.Cores
{
    public class TowerScript : Singleton<TowerScript>,IHpManager
    {

        [SerializeField] private List<GameObject>  turrets = new List<GameObject>();
        public static bool PlayerInRange = false;

        [SerializeField] private LayerMask layermask;

        public bool HaveAnyTurret {  get; private set; }
        public TowerHPManager hpManager { get; private set; }

        [SerializeField] private Slider _hpSlider;
        [SerializeField] private TMP_Text _hpText;


        private void Start()
        {
            hpManager = new TowerHPManager();
            hpManager.Initialize(100, _hpSlider, _hpText);
            
            foreach (GameObject turret in turrets)
                turret.SetActive(false);
        }
        public void Update()
        {
            hpManager.HPUI();
            hpManager.HPLogic(null);
            TransparentTower();
        }
        private void OnTriggerEnter(Collider other) => TogglePlayerInRange(other);
        private void OnTriggerStay(Collider other) => TogglePlayerInRange(other);



        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                PlayerInRange = false;
        }
        private void TransparentTower()
        {
            var distance = Vector2.Distance(Camera.main.transform.position, PlayerScript.Instance.transform.position);
            var dir = PlayerScript.Instance.transform.position - Camera.main.transform.position;

            if (Physics.Raycast(Camera.main.transform.position, dir, Mathf.Infinity,layermask,QueryTriggerInteraction.Ignore))
            {
                
                Fade(0.3f);
            }
            else
            {
                Fade(1);
            }

        }
        private void Fade(float alpha)
        {
            var orgColor = GetComponent<Renderer>().material.color;
            GetComponent<Renderer>().material.color = Color.Lerp(orgColor, new Color(orgColor.r, orgColor.g, orgColor.b, alpha), 0.05f);
        }
        private static void TogglePlayerInRange(Collider other)
        {
            if (other.CompareTag("Player") && (SkillManager.GetSkill<HpTransfer>()?.HaveSkill ?? false))
                PlayerInRange = true;
        }
        public void ActiveTurret(int index)
        {
            HaveAnyTurret = true;
            turrets[index].SetActive(true);
        }
        public void ModifyFireRate(float value)
        {
            foreach (GameObject turret in turrets)
                turret.GetComponent<TurretScript>().ModifyFireRate(value);
        }
        public void ModifyDamage(float value)
        {
            foreach (GameObject turret in turrets)
                turret.GetComponent<TurretScript>().ModifyDamage(value);
        }
        public HPManager GetHPManager() => hpManager;


        public void OnDrawGizmosSelected()
        {
            Ray ray = new(Camera.main.transform.position, PlayerScript.Instance.transform.position - Camera.main.transform.position);

            Gizmos.DrawLine(Camera.main.transform.position, PlayerScript.Instance.transform.position);
            //Gizmos.DrawRay(ray);
        }
    }
}