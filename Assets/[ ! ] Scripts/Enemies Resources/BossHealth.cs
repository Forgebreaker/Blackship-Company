using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [Header("Boss Type")]

        [SerializeField] private bool EasyBoss;
        [SerializeField] private bool MediumBoss;
        [SerializeField] private bool HardBoss;

    [Header("Health Data")]

        [SerializeField] private int MaximumHealth;
        private int CurrentHealth;
        private Collider2D Boss_Collider;
        private GameObject BossHealth_UI;
        private Text BossHealth_Text;
        private Image BossHealth_Bar;

        private AudioSource _audioSource;
        [SerializeField] private AudioClip TakeDamageEffect_AudioClip;
        [SerializeField] private ParticleSystem TakeDamageEffect;
        [SerializeField] private AudioClip BossExplodeEffect_AudioClip;
        [SerializeField] private ParticleSystem BossExplodeEffect;

    [Header("Coin")]

        public int IncreasedCoin;
        private bool IsIncreased;

    private void Start()
    {
        IsIncreased = false;
        CurrentHealth = MaximumHealth;
        Boss_Collider = GetComponent<Collider2D>();
        BossHealth_UI = GameObject.FindGameObjectWithTag("Boss Health Bar");
        
        if (BossHealth_UI != null)
        {
            BossHealth_Text = BossHealth_UI.GetComponentInChildren<Text>();
            Transform healthBarTransform = BossHealth_UI.transform.Find("HealthBar");
            if (healthBarTransform != null)
            {
                BossHealth_Bar = healthBarTransform.GetComponent<Image>();
            }
        }
        
        _audioSource = GetComponent<AudioSource>();
        
        UpdateHealthUI();
    }

    private void Update()
    {
        HealthSystem();
    }

    private void HealthSystem()
    {
        //if (CurrentHealth > 0)
        //{
        //    this.gameObject.transform.localScale = Vector3.one;
        //}
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaximumHealth);

        if (CurrentHealth <= 0)
        {
            Boss_Collider.enabled = false;
            
            if (EasyBoss && IsIncreased == false) 
            {
                IncreasedCoin = Random.Range(5000, 10000);
                GameDataManager.AddCoins(IncreasedCoin);
                IsIncreased = true;
            }
            if (MediumBoss && IsIncreased == false) 
            {
                IncreasedCoin = Random.Range(25, 35);
                GameDataManager.AddCoins(IncreasedCoin);
                IsIncreased = true;
            }

            if (HardBoss && IsIncreased == false)
            {
                IncreasedCoin = Random.Range(70, 100);
                GameDataManager.AddCoins(IncreasedCoin);
                IsIncreased = true;
            }
        }
    }

    public void TakeDamage(int Damage)
    {
        CurrentHealth -= Damage;
        
        UpdateHealthUI();

        if (CurrentHealth <= 0)
        {
            ParticleSystem DieEffect = Instantiate(BossExplodeEffect, this.gameObject.transform.position, this.gameObject.transform.rotation);
            DieEffect.Play();
            _audioSource.PlayOneShot(BossExplodeEffect_AudioClip);
            this.gameObject.transform.localScale = Vector3.zero;
        }

        if (CurrentHealth > 0)
        {
            ParticleSystem HitEffect = Instantiate(TakeDamageEffect, this.gameObject.transform.position, this.gameObject.transform.rotation);
            HitEffect.Play();
            Destroy(HitEffect, 1.5f);
            _audioSource.PlayOneShot(TakeDamageEffect_AudioClip);
        }
    }

    private void UpdateHealthUI()
    {
        BossHealth_Text.text = $"{CurrentHealth}";
        BossHealth_Bar.fillAmount = (float)CurrentHealth / MaximumHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SpaceshipController player = collision.GetComponent<SpaceshipController>();
            
            if (player != null)
            {
                player.TakeDamage(CurrentHealth);
                TakeDamage(player.CurrentHealth);
            }
        }
    }

}
