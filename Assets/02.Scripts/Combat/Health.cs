using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, ISaveable
{
	[SerializeField] TakeDamageEvent takeDamage;
	[SerializeField] UnityEvent onDie;

	[Serializable]
	public class TakeDamageEvent : UnityEvent<float>
	{

	}

	BaseStats baseStats;
	ActionScheduler actionScheduler;
	LazyValue<float> healthPoints;
	bool isDead = false;

	private void Awake()
	{
		healthPoints = new LazyValue<float>(GetInitialHealth);
		actionScheduler = GetComponent<ActionScheduler>();
		baseStats = GetComponent<BaseStats>();
	}

	private float GetInitialHealth()
	{
		return GetComponent<BaseStats>().GetStat(Stats.Health);
	}

	private void Start()
	{
		healthPoints.ForceInit();
	}

	public void TakeDamage(GameObject instigator, float damage)
	{
		healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
		Debug.Log("Health : " + healthPoints.value);
		if(healthPoints.value == 0)
		{
			onDie?.Invoke();
			Die();
			AwardExperience(instigator); // ����ġ �ֱ�
		}
		else
		{
			takeDamage.Invoke(damage);
		}
	}

    public float getHealthPoints()
    {
        return healthPoints.value;
    }

    public float GetMaxHealthPoints()
    {
		return baseStats.GetStat(Stats.Health);
    }

    public bool IsDead() => isDead;

    private void Die()
    {
		if (isDead) return;

		isDead = true;
		GameObjectExtention.GetComponentAroundOrAdd<Animator>(this.gameObject).SetTrigger("Death");
		actionScheduler?.CancleCurrentAction();
    }

    private void AwardExperience(GameObject instigator)
    {
        Experience experience = instigator.GetComponent<Experience>();

		if (experience == null) return;
		experience.GainExperience(GetComponent<BaseStats>().GetStat(Stats.ExperienceReward));

	}

    public object CaptureState()
    {
        return healthPoints.value;
    }

    public void RestoreState(object state)
    {
		healthPoints.value = (float)state;
		Debug.Log("RestoreState health : " + healthPoints.value);
    }

    
}
