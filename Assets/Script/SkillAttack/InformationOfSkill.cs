using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ROC
{
    public class InformationOfSkill
    {
        public static SkillValue skill = new SkillValue();
        private static void SetSkillValue(int attack01, int attack02, int attack03, int timeValue01, int timeValue02, int timeValue03, int mp01, int mp02, int mp03)
        {
            // Attack
            skill.attack[0] = attack01;
            skill.attack[1] = attack02;
            skill.attack[2] = attack03;

            // Time
            skill.timeValue[0] = timeValue01;
            skill.timeValue[1] = timeValue02;
            skill.timeValue[2] = timeValue03;

            // Mp
            skill.mp[0] = mp01;
            skill.mp[1] = mp02;
            skill.mp[2] = mp03;
        }

        private static void SetSkillPercent(int percent01, int percent02, int percent03)
        {
            // Attack
            skill.percentValue[0] = percent01;
            skill.percentValue[1] = percent02;
            skill.percentValue[2] = percent03;
        }

        public static PlayerAttackSkill.skill SkillName(int skillNumber)
        {
            switch (skillNumber)
            {
                case 1:
                    return PlayerAttackSkill.skill.Angel_2;
                case 2: // Attack Skill
                    return PlayerAttackSkill.skill.EnergyStrike_1;
                case 3: // Attack Skill
                    return PlayerAttackSkill.skill.Energy_1;
                case 4: // Attack Skill
                    return PlayerAttackSkill.skill.FrontExplosion_2;
                case 5: // Attack Skill
                    return PlayerAttackSkill.skill.LightRay_1;
                case 6:
                    return PlayerAttackSkill.skill.Magic_1;
                case 7:
                    return PlayerAttackSkill.skill.Nature_1;
                case 8:
                    return PlayerAttackSkill.skill.Shine_1;
                case 9: // Attack Skill
                    return PlayerAttackSkill.skill.Shine_2;
            }
            return PlayerAttackSkill.skill.None;
        }

        public static SkillValue Information(int skillNum)
        {
            switch (skillNum) 
            {
                case 1:
                    skill.skillDescription = "[천사의 은총 / 특수 스킬] 죽을 시 단 한번 부활 가능하다.                             (절반의 Hp,Mp를 가지고 부활)";
                    break;

                case 2:
                    SetSkillValue(50,80,100,4,3,2,4,3,2);
                    skill.skillDescription = "[스파크 붐 / 공격 스킬 / 강화 가능] 적에게 타겟팅 후 공격한다.";
                    break;

                case 3:
                    SetSkillValue(60, 100, 200, 8, 8, 8, 5, 4, 3);
                    skill.skillDescription = "[뇌륜 / 공격 스킬 / 강화 가능] 범위 내부 적에게 데미지를 준다.";
                    break;

                case 4:
                    SetSkillValue(40, 70, 120, 10, 8, 6, 6, 5, 4);
                    skill.skillDescription = "[연폭 전격 / 공격 스킬 / 강화 가능] 전방 일정 거리 내 존재하는 적들에게 데미지를 준다.";
                    break;

                case 5:
                    SetSkillValue(70, 100, 300, 15, 12, 9, 5, 5, 5);
                    skill.skillDescription = "[신의 눈물 / 공격 스킬 / 광역기 / 강화 가능] 범위안의 몬스터들이 일정 간격으로 데미지를 입는다.";
                    break;

                case 6:
                    SetSkillPercent(15, 30, 40);
                    skill.skillDescription = "[잠재력 개방 / 패시브 / 강화 가능] 모든 스킬의 쿨타임이 감소된다.";
                    break;

                case 7:
                    SetSkillPercent(30, 50, 100);
                    SetSkillValue(0, 0, 0, 30, 30, 30, 0, 0, 0);
                    skill.skillDescription = "[자연 에너지 흡수 / 버프 / 강화 가능] 사용 시 체력이 회복된다.";
                    break;

                case 8:
                    SetSkillPercent(20, 40, 60);
                    skill.skillDescription = "[투신의 심장 / 패시브 / 버프 / 강화 가능] 공격 스킬의 공격력이 증가한다."; ;
                    break;

                case 9:
                    SetSkillValue(50, 150, 400, 180, 120, 80, Player.maxMp / 2, Player.maxMp / 2, Player.maxMp / 2);
                    skill.skillDescription = "[초신성 / 공격 스킬 / 강화 가능] 공중에서 랜덤적으로 선택된 위치로 공격을 시도한다.";
                    break;


                case 10:
                    SetSkillValue(20, 30, 50, 14, 12, 10, 3, 3, 3);
                    skill.skillDescription = "[기본 공격 / 강화 가능] 적을 자동 조준하여 공격할 수 있다. 대상을 직접 조준하는 공격 또한 가능하다.";
                    break;

            }
            return skill;
        }
    }

}
