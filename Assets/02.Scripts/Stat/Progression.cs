using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
public class Progression : ScriptableObject
{
	//characterClasses �迭�� ���� Ŭ������ �׿� ���� ���� ������ �����մϴ�
	[SerializeField] ProgressionCharacterClass[] characterClasses = null;

	//lookupTable�� ���� �� ���� Ŭ������ ���� ������ �̸� ����Ͽ� �����ϴ� Dictionary�Դϴ�.
	Dictionary<CharacterClass, Dictionary<Stats, float[]>> lookupTable = null;

	//Ư�� ������ Ư�� ���� Ŭ���� �� ������ �ش��ϴ� ���� ���� ��ȯ�� �մϴ�.
	public float GetStat(Stats stats, CharacterClass characterClass, int level)
	{
		BuildLookup();

		//lookupTable�� �ش� characterClass�� ���� ���� �����Ǿ� ���� ������ 0 ��ȯ
		if (!lookupTable[characterClass].ContainsKey(stats))
			return 0;

		float[] levels = lookupTable[characterClass][stats];

		//levels �迭�� 0 ���� ���� ��� 0 ��ȯ
		if (levels.Length <= 0)
			return 0;

		//���� ĳ������ ������ ������ �������� ���� ������ Ŭ ��� �ִ� ���� ������ ��ȯ
		if (levels.Length < level)
			return levels[levels.Length - 1];

		//���� ������ ���� ������ ��ȯ
		return levels[level - 1];
	}

	// Ư�� ������ Ư�� Ŭ������ �ִ� ������ ��ȯ�մϴ�.
    public int GetLevels(Stats stats, CharacterClass characterClass)
    {
		BuildLookup();

		// �̸� ������ ������ ������ �����ɴϴ�.
		float[] levels = lookupTable[characterClass][stats];

		// ������ �迭�� ����, �� �ִ� ������ ��ȯ�մϴ�.
		return levels.Length;
    }

    //lookupTable�� �����ϰ� ���� ������ �����մϴ�.
    private void BuildLookup()
	{
		// �̹� lookupTable�� �����Ǿ����� �Լ��� �����մϴ�.
		if (lookupTable != null) return;

		//lookupTable�� �ʱ�ȭ�մϴ�.
		lookupTable = new Dictionary<CharacterClass, Dictionary<Stats, float[]>>();

		//characterClasses �迭�� �ִ� ���� Ŭ���� �� ���� ������ ������ ���� ó���մϴ�.
		foreach (ProgressionCharacterClass progressionClass in characterClasses)
		{
			//statLookupTable�� �����մϴ�.
			var statLookupTable = new Dictionary<Stats, float[]>();

			//���� Ŭ������ ���� ���� ������ ������ ���� ó���մϴ�.
			foreach(ProgressionStat progressionStat in progressionClass.stats)
			{
				statLookupTable[progressionStat.stats] = progressionStat.levels;
			}

			//���� Ŭ������ ���� ������ lookupTable�� �����մϴ�.
			lookupTable[progressionClass.characterClass] = statLookupTable;
		}
	}

	//���� Ŭ���� ������ �����ϴ� ���� Ŭ����
	[Serializable]
	class ProgressionCharacterClass
	{
		public CharacterClass characterClass;//���� Ŭ����
		public ProgressionStat[] stats;//�ش� ���� Ŭ������ ���� ���� �迭
	}

	//���� ������ �����ϴ� ���� Ŭ����
    [Serializable]
    class ProgressionStat
    {
		public Stats stats;//����
		public float[] levels;//������ ���� �� �迭
    }
}
