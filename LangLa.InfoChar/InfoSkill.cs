using LangLa.Data;
using LangLa.SupportOOP;
using LangLa.Template;

namespace LangLa.InfoChar
{
	public class InfoSkill
	{
		public Skill[]? Skills;

		public short TangPhanTramDameSkill;

		public short TangThoiGianTelietDichChuyenChiThuat;

		public short IdFocus;

		public InfoSkill(sbyte IdClass, sbyte GioiTinh)
		{
			Skills = CloneSkill(IdClass, GioiTinh);
		}

		public static Skill[] CloneSkill(sbyte IdClass, sbyte GioiTinh)
		{
			Skill[] skills = new Skill[6]
			{
				GetCloneSkill(SkillTemplate.KIEM_THUAT_CO_BAN, 1),
				null,
				null,
				null,
				null,
				null
			};
			switch (IdClass)
			{
			case 1:
				skills[1] = GetCloneSkill(SkillTemplate.TAM_TRUNG_OAN_THU_THUAT, 0);
				skills[2] = GetCloneSkill(SkillTemplate.KIEM_THUAT_TAM_PHAP, 0);
				skills[3] = GetCloneSkill(SkillTemplate.DICH_CHUYEN_CHI_THUAT, 0);
				skills[4] = GetCloneSkill(SkillTemplate.LOI_KIEM, 0);
				skills[5] = GetCloneSkill(SkillTemplate.TRIEU_HOI_CHIM_CHI_THUAT, 0);
				break;
			case 2:
				skills[1] = GetCloneSkill(SkillTemplate.SONG_THANG_LONG, 0);
				skills[2] = GetCloneSkill(SkillTemplate.TIEU_THUAT_TAM_PHAP, 0);
				skills[3] = GetCloneSkill(SkillTemplate.AN_CHU_CHI_THUAT, 0);
				skills[4] = GetCloneSkill(SkillTemplate.ANH_PHONG_XA, 0);
				skills[5] = GetCloneSkill(SkillTemplate.TRIEU_HOI_DOI_CHI_THUAT, 0);
				break;
			case 3:
				skills[1] = GetCloneSkill(SkillTemplate.THUY_LONG_AN_THUAT, 0);
				skills[2] = GetCloneSkill(SkillTemplate.GAY_THUAT_TAM_PHAP, 0);
				skills[3] = GetCloneSkill(SkillTemplate.TANG_SINH_CHI_THUAT, 0);
				skills[4] = GetCloneSkill(SkillTemplate.AI_BOC_BO_THUAT, 0);
				skills[5] = GetCloneSkill(SkillTemplate.THUY_LAO_THUAT, 0);
				break;
			case 4:
				skills[1] = GetCloneSkill(SkillTemplate.AM_SAT_THUAT, 0);
				skills[2] = GetCloneSkill(SkillTemplate.AO_THUAT_TAM_PHAP, 0);
				skills[3] = GetCloneSkill(SkillTemplate.AN_THAN_CHI_THUAT, 0);
				skills[4] = GetCloneSkill(SkillTemplate.HAO_HOA_CAU_THUAT, 0);
				skills[5] = GetCloneSkill(SkillTemplate.TRIEU_HOI_CHIM_YEU_CHI_THUAT, 0);
				break;
			case 5:
				skills[1] = GetCloneSkill(SkillTemplate.THIEN_SAT_THUY_PHI, 0);
				skills[2] = GetCloneSkill(SkillTemplate.DAO_THUAT_TAM_PHAP, 0);
				skills[3] = GetCloneSkill(SkillTemplate.ANH_PHAN_THAN_CHI_THUAT, 0);
				skills[4] = GetCloneSkill(SkillTemplate.PHI_LOI_THAN_THUAT, 0);
				skills[5] = GetCloneSkill(SkillTemplate.HIEN_NHAN_THUAT, 0);
				break;
			}
			return skills;
		}

		public static Skill GetCloneSkill(short id, sbyte level)
		{
			Skill skillClone = null;
			Skill[] arrSkill = DataServer.ArrSkill;
			foreach (Skill skill in arrSkill)
			{
				if (skill.IdTemplate == id && skill.Level == level)
				{
					skillClone = new Skill();
					skillClone.Id = skill.Id;
					skillClone.IdTemplate = skill.IdTemplate;
					skillClone.Level = level;
					skillClone.LevelNeed = skill.LevelNeed;
					skillClone.MaxTarget = skill.MaxTarget;
					skillClone.MpUse = skill.MpUse;
					skillClone.RangeDoc = skill.RangeDoc;
					skillClone.RangeNgang = skill.RangeNgang;
					skillClone.Options = skill.Options;
					skillClone.CoolDown = skill.CoolDown;
					skillClone.Index = skill.Index;
					break;
				}
			}
			return skillClone;
		}

		public static Skill GetSkill(short id)
		{
			Skill Skill = null;
			Skill[] arrSkill = DataServer.ArrSkill;
			foreach (Skill skill in arrSkill)
			{
				if (skill.IdTemplate == id)
				{
					Skill = skill;
					break;
				}
			}
			return Skill;
		}

		public static Skill GetSkillAndLevel(short id, sbyte level)
		{
			Skill Skill = null;
			Skill[] arrSkill = DataServer.ArrSkill;
			foreach (Skill skill in arrSkill)
			{
				if (skill.IdTemplate == id && skill.Level == level)
				{
					Skill = skill;
					break;
				}
			}
			return Skill;
		}
	}
}
