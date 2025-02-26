using UnityEngine.UI;
using TMPro;
using UnityEngine;

namespace TheSentinel.Cores
{
    public class PlayerScript : Singleton<PlayerScript>, IHpManager
    {
        public static float MoveSpeed => _moveSpeed;

        [SerializeField] private Color RageColor;
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private Transform _gunPoint;
        [SerializeField] private TMP_Text _hpText;
        [SerializeField] private Image _hpFillImage;

        private static float _moveSpeed;
        private float _initialMoveSpeed, _tempMoveSpeedChange, _hpDropValue = 7; 
        private int _ammoDropValue = 5, camRayLength = 100;
        private bool _rageMode = false;
        private Rigidbody _rb;
        private Vector3 _movement,target;
        private Color originalColor;
        private LayerMask floorMask;
        private PlayerHPManager hpManager;

        private void Awake()
        {
            hpManager = new PlayerHPManager();
            _rb = GetComponent<Rigidbody>();
            floorMask = LayerMask.GetMask("floor");

            _initialMoveSpeed = 5;
            _moveSpeed = _initialMoveSpeed;

            hpManager.Initialize(100);

        }
        private void Update()
        {
            MovementInput();
            Turning();
            hpManager.HPUI();
            hpManager.HPLogic(null);
            //Rage mode color change
        }

        private void FixedUpdate() => Movement();
        private void MovementInput()
        {
            float h = Input.GetAxisRaw("Horizontal"), v = Input.GetAxisRaw("Vertical");
            _movement = new Vector3(h, 0f, v);
        }
        private void Movement()
        {
            _movement = _movement.normalized * (_moveSpeed + _tempMoveSpeedChange) * Time.deltaTime;
            _rb.MovePosition(transform.position + _movement);
        }
        private void Turning()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, camRayLength, floorMask))
            {
                Vector3 pos = hit.point - transform.position;
                pos.y = 0;
                Quaternion rot = Quaternion.LookRotation(pos);
                var r = rot.eulerAngles;
                r.Set(r.x, r.y - 90, r.z);

                if (!GameManager.OnPause)
                    _rb.MoveRotation(Quaternion.Euler(r));
            }
        }
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.CompareTag("HP"))
            {
                hpManager.ModifyHP(_hpDropValue);
                Destroy(collision.gameObject);
            }
            if (collision.CompareTag("AMMO"))
            {
                GunScript.Instance.AddAmmo(_ammoDropValue);
                Destroy(collision.gameObject);
            }
        }
        private void SetMoveSpeedToOriginal() => _tempMoveSpeedChange = 0;
        public void AddMoveSpeed(float value) => _moveSpeed += value;
        public void ModifyMoveSpeedTemporarily(float value, float time)
        {
            _tempMoveSpeedChange = value;
            Invoke(nameof(SetMoveSpeedToOriginal), time);
        }
        public void ToggleRageMode(bool value) => _rageMode = value;
        public HPManager GetHPManager() => hpManager;
    }
}