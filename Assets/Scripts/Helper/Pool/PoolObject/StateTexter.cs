//===============================================
//作者：吴江
//日期：2015/5/30
//用途：状态漂浮文字的对象（派生自对象池对象)  包括伤害数字，buff等
//===================================================




using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateTexter : PoolObject
{
    /// <summary>
    /// 总共能持续的时间
    /// </summary>
    public float totalTime = 0.4f;

    public static Vector2 fixedFriendRotation = new Vector2(0, 1);
    public static Vector2 fixedEnemyRotation = new Vector2(4, 1);

    public class TextJumpPath
    {
        /// <summary>
        /// 本阶段比率
        /// </summary>
        public float durationRate = 0;

        /// <summary>
        /// 目标坐标
        /// </summary>
        public Vector3 toPos = Vector3.zero;

        /// <summary>
        /// 目标大小
        /// </summary>
        public Vector3 toScale = Vector3.zero;


        public TextJumpPath(float _durationRate, Vector3 _toPos, Vector3 _toScale)
        {
            durationRate = _durationRate;
            toPos = _toPos;
            toScale = _toScale;
        }
    }

    /// <summary>
    /// 本次取出以后的表现路径列表
    /// </summary>
    List<TextJumpPath> textJumpPathList = new List<TextJumpPath>();

    public void OnSpawned(AbilityResultInfo _info)
    {
        textJumpPathList.Clear();
        if (_info.UserActor == null || _info.TargetActor == null)
        {
            ReturnBySelf();
            return;
        }
        HeadTextCtrl textCtrl = _info.TargetActor.GetHeadTextCtrl();
        if (textCtrl == null || textCtrl.TextParent == null)
        {
            ReturnBySelf();
            return;
        }
        //是否自己受伤
		//RelationType relation = ConfigMng.Instance.GetRelationType(GameCenter.curMainPlayer.Camp, _info.TargetActor.Camp, GameCenter.curGameStage.SceneType);
		bool isFriend = _info.TargetActor.id == GameCenter.curMainPlayer.id;// || relation == RelationType.NO_ATTAK;
		//是否是自己宠物攻击的
		bool isPetAttack = (_info.UserActor == null || GameCenter.curMainEntourage == null)?false:(_info.UserActor.id == GameCenter.curMainEntourage.id);

        int nextDepth = NGUITools.CalculateNextDepth(textCtrl.TextParent);
        Vector3 fromPos = textCtrl.TextParent.transform.position;
        Vector3 vol = fromPos - _info.UserActor.transform.position;
        vol = isFriend ? new Vector3(vol.x * fixedFriendRotation.x, vol.y * fixedFriendRotation.y + vol.z * fixedFriendRotation.x)
            : new Vector3(vol.x * fixedEnemyRotation.x, vol.y * fixedEnemyRotation.y + vol.z * fixedEnemyRotation.x);
        Vector3 toPos = fromPos + vol.normalized * 2.0f;


        UILabel lb = this.gameObject.GetComponent<UILabel>();
        string text = string.Empty;
        UIFont font = null;
 //       Color color = Color.white;
        Vector3 localScale = Vector3.zero;
        if (lb != null)
        {
            lb.transform.parent = textCtrl.TextParent.transform;
            lb.gameObject.SetMaskLayer(textCtrl.TextParent.layer);
            lb.transform.localPosition = Vector3.zero;
            switch (_info.AttackType)
            {
                case AttackResultType.ATT_SORT_DODGE:
                    text = "s";
					font = isFriend ? textCtrl.font_get_dmg : textCtrl.font_dmg;
					if(isPetAttack)font = textCtrl.font_pet_dmg;
                    break;
                case AttackResultType.ATT_SORT_HIGHDEF:
                    //text = _info.curDamage > 0 ? "d-" + _info.curDamage.ToString() : "m";
					text = "d-" + _info.curDamage.ToString();
					font = isFriend ? textCtrl.font_get_dmg : textCtrl.font_dmg;
					if(isPetAttack)font = textCtrl.font_pet_dmg;
                    break;
                case AttackResultType.ATT_SORT_CRIT:
                    if (_info.DefType == DefResultType.DEF_SORT_TREAT)
                    {
                        //text = _info.curDamage > 0 ? "b+" + _info.curDamage.ToString() : "m";
						text = "b+" + _info.curDamage.ToString();
						font = isFriend ? textCtrl.zl_font : textCtrl.zl_font;
						if(isPetAttack)font = textCtrl.font_pet_dmg;
                    }
                    else
                    {
                        //text = _info.curDamage > 0 ? "b-"+_info.curDamage.ToString() : "m";
						text = "b-" + _info.curDamage.ToString();
						font = isFriend ? textCtrl.font_get_dmg : textCtrl.bj_font;
						if(isPetAttack)font = textCtrl.font_pet_dmg;
                        if (isFriend)
                        {
                            textJumpPathList.Add(new TextJumpPath(0.05f, Vector3.Lerp(fromPos, toPos, 0.2f), new Vector3(0.5f, 0.4f, 0f)));
						    textJumpPathList.Add(new TextJumpPath(0.1f, Vector3.Lerp(fromPos, toPos, 0.4f), new Vector3(1f, 0.9f, 0f)));
						    textJumpPathList.Add(new TextJumpPath(0.8f, Vector3.Lerp(fromPos, toPos, 0.6f), new Vector3(1f, 0.9f, 0f)));
                        }
                        else
                        {
                            textJumpPathList.Add(new TextJumpPath(0.15f, Vector3.Lerp(fromPos, toPos, 0.2f), new Vector3(0.8f, 1.2f, 0f)));
                            textJumpPathList.Add(new TextJumpPath(0.25f, Vector3.Lerp(fromPos, toPos, 0.4f), new Vector3(0.8f, 0.7f, 0f)));
						    textJumpPathList.Add(new TextJumpPath(0.9f, Vector3.Lerp(fromPos, toPos, 0.6f), new Vector3(0.7f, 0.5f, 0f)));
                        }
                        textJumpPathList.Add(new TextJumpPath(1.0f, Vector3.Lerp(fromPos, toPos, 1.0f), new Vector3(0.1f, 0.08f, 0f)));
                    }
                    break;
				case AttackResultType.ATT_SORT_LUCKY_HIT:
					text = isFriend?("bxy-"+_info.curDamage):("xy-"+_info.curDamage);
					font = isFriend ? textCtrl.font_get_dmg : textCtrl.xy_font;
					if(isPetAttack)font = textCtrl.font_pet_dmg;
					if (isFriend)
					{
						textJumpPathList.Add(new TextJumpPath(0.05f, Vector3.Lerp(fromPos, toPos, 0.2f), new Vector3(0.4f, 0.3f, 0f)));
						textJumpPathList.Add(new TextJumpPath(0.1f, Vector3.Lerp(fromPos, toPos, 0.4f), new Vector3(0.8f, 0.6f, 0f)));
						textJumpPathList.Add(new TextJumpPath(0.8f, Vector3.Lerp(fromPos, toPos, 0.6f), new Vector3(0.8f, 0.6f, 0f)));
					}
					else
					{
						textJumpPathList.Add(new TextJumpPath(0.15f, Vector3.Lerp(fromPos, toPos, 0.2f), new Vector3(1.4f, 1.1f, 0f)));
						textJumpPathList.Add(new TextJumpPath(0.25f, Vector3.Lerp(fromPos, toPos, 0.4f), new Vector3(0.7f, 0.6f, 0f)));
						textJumpPathList.Add(new TextJumpPath(0.9f, Vector3.Lerp(fromPos, toPos, 0.6f), new Vector3(0.6f, 0.4f, 0f)));
					}
					textJumpPathList.Add(new TextJumpPath(1.0f, Vector3.Lerp(fromPos, toPos, 1.0f), new Vector3(0.1f, 0.08f, 0f)));
					break;
                default:
                    switch (_info.DefType)
                    {
                        case DefResultType.DEF_SORT_TREAT:
                            if (_info.curDamage == 0)
                                text = string.Empty;
                            else
                                text = "+ " + _info.curDamage.ToString();
                            font = textCtrl.zl_font;
                            break;
                        case DefResultType.DEF_SORT_STIFLE:
                            text = "-" + _info.curDamage.ToString();
							//font = textCtrl.font_dmg;
							font = isFriend ? textCtrl.font_get_dmg : textCtrl.font_dmg;
                            break;
                        case DefResultType.DEF_SORT_NOKICK:
                            text = "-"+_info.curDamage.ToString();
							//font = textCtrl.font_dmg;
							font = isFriend ? textCtrl.font_get_dmg : textCtrl.font_dmg;
                            fromPos = toPos;
                            textJumpPathList.Add(new TextJumpPath(0.2f, Vector3.Lerp(fromPos, toPos, 0.8f), new Vector3(0.6f, 0.4f, 0f)));
                            textJumpPathList.Add(new TextJumpPath(0.6f, Vector3.Lerp(fromPos, toPos, 0.8f), new Vector3(0.8f, 0.6f, 0f)));
                            textJumpPathList.Add(new TextJumpPath(0.2f, toPos, new Vector3(0.80f, 0.8f, 0f)));
                            break;
                        case DefResultType.DEF_SORT_NOKICKDOWN:
                            text = "-"+_info.curDamage.ToString();
							//font = textCtrl.font_dmg;
							font = isFriend ? textCtrl.font_get_dmg : textCtrl.font_dmg;
                            fromPos = toPos;
                            textJumpPathList.Add(new TextJumpPath(0.2f, Vector3.Lerp(fromPos, toPos, 0.8f), new Vector3(0.6f, 0.4f, 0f)));
                            textJumpPathList.Add(new TextJumpPath(0.6f, Vector3.Lerp(fromPos, toPos, 0.8f), new Vector3(0.8f, 0.6f, 0f)));
                            textJumpPathList.Add(new TextJumpPath(0.2f, toPos, new Vector3(0.8f, 0.8f, 0f)));
                            break;
                        default:
                            //text = _info.curDamage > 0 ? "-"+_info.curDamage.ToString() : "m";
							text = "-"+_info.curDamage.ToString();
							font = isFriend ? textCtrl.font_get_dmg : textCtrl.font_dmg;
							if(isPetAttack)font = textCtrl.font_pet_dmg;
                            break;
                    }
                    break;
            }
            if (isFriend)
            {
                //color = Color.red;
            }
            if (textJumpPathList.Count == 0)
            {
				if (isFriend)
				{
					textJumpPathList.Add(new TextJumpPath(0.05f, Vector3.Lerp(fromPos, toPos, 0.2f), new Vector3(0.3f, 0.25f, 0f)));
					textJumpPathList.Add(new TextJumpPath(0.1f, Vector3.Lerp(fromPos, toPos, 0.4f), new Vector3(0.9f, 0.7f, 0f)));
					textJumpPathList.Add(new TextJumpPath(0.8f, Vector3.Lerp(fromPos, toPos, 0.6f), new Vector3(0.9f, 0.7f, 0f)));
				}
				else
				{
					textJumpPathList.Add(new TextJumpPath(0.15f, Vector3.Lerp(fromPos, toPos, 0.2f), new Vector3(1.5f, 1.2f, 0f)));
					textJumpPathList.Add(new TextJumpPath(0.25f, Vector3.Lerp(fromPos, toPos, 0.4f), new Vector3(0.8f, 0.65f, 0f)));
					textJumpPathList.Add(new TextJumpPath(0.9f, Vector3.Lerp(fromPos, toPos, 0.6f), new Vector3(0.7f, 0.55f, 0f)));
				}
				textJumpPathList.Add(new TextJumpPath(1.0f, Vector3.Lerp(fromPos, toPos, 1.0f), new Vector3(0.1f, 0.08f, 0f)));
                //textJumpPathList.Add(new TextJumpPath(0.1f, toPos, new Vector3(0.5f, 0.5f, 0.6f)));
            }
            this.gameObject.transform.localScale = localScale;
            if (font != null)
            {
                lb.bitmapFont = font;
                // lb.transform.localScale = new Vector3(font.defaultSize, font.defaultSize, 1f);
            }
            else
            {
                GameSys.LogError(_info.AttackType + " , " + _info.DefType + ConfigMng.Instance.GetUItext(192));
            }
            lb.text = text;
            lb.depth = nextDepth;
            lb.pivot = UILabel.Pivot.Center;
            lb.supportEncoding = true;
            lb.symbolStyle = NGUIText.SymbolStyle.Normal;
            lb.fontSize = 1;

            //lb.color = color;  字集自带颜色,则不设置颜色 by邓成

        }
        NewStepInit();

        base.OnSpawned();


    }
/// <summary>
/// 生命恢复 数字 by 何明军
/// </summary>
	public void OnSpawned(int _value)
	{
		textJumpPathList.Clear();
		HeadTextCtrl textCtrl = GameCenter.curMainPlayer.GetHeadTextCtrl();
		if (textCtrl == null || textCtrl.TextParent == null)
		{
			ReturnBySelf();
			return;
		}
		int nextDepth = NGUITools.CalculateNextDepth(textCtrl.TextParent);
		Vector3 fromPos = textCtrl.TextParent.transform.position;
		Vector3 toPos = fromPos + new Vector3(0,2f,0);
		UILabel lb = this.gameObject.GetComponent<UILabel>();
		if (lb != null)
		{
			lb.transform.parent = textCtrl.TextParent.transform;
			lb.gameObject.SetMaskLayer(textCtrl.TextParent.layer);
			lb.transform.localPosition = Vector3.zero;
			
			lb.text = "+" +_value;
			lb.bitmapFont = textCtrl.zl_font;
			
			lb.depth = nextDepth;
			lb.pivot = UILabel.Pivot.Center;
			lb.supportEncoding = true;
			lb.symbolStyle = NGUIText.SymbolStyle.Normal;
			lb.fontSize = 1;

			lb.color = Color.green;;
			
			if(textJumpPathList.Count == 0){
//				textJumpPathList.Add(new TextJumpPath(0.15f, Vector3.Lerp(fromPos, toPos, 2f), new Vector3(1.5f, 1.2f, 0f)));
				textJumpPathList.Add(new TextJumpPath(1.5f, Vector3.Lerp(fromPos, toPos, 0.6f), new Vector3(0.7f, 0.7f, 0f)));
			}
		}
		NewStepInit();

		base.OnSpawned();
	}
	
    float deltTime = 0;
    float startTime = 0;
    float curDuration = 0;
    Vector3 curFrom = Vector3.zero;
    Vector3 curScale = Vector3.zero;
    TextJumpPath curTextJumpPath = null;

    /// <summary>
    /// 更新到下一步  by吴江
    /// </summary>
    void NewStepInit()
    {
        if (textJumpPathList.Count <= 0)
        {
            GameSys.LogError("文字面板变化路径为空！无法初始化！");
            return;
        }
        startTime = Time.time;
        curTextJumpPath = textJumpPathList[0];
        curDuration = curTextJumpPath.durationRate * totalTime;
        curFrom = transform.position;
        curScale = transform.localScale;
    }


    void Update()
    {
        if (!beSpawnedNow) return;
        deltTime = Time.time - startTime;
        if (deltTime >= curDuration)
        {
            textJumpPathList.RemoveAt(0);
            if (textJumpPathList.Count > 0)
            {
                NewStepInit();
                deltTime = Time.time - startTime;
            }
            else
            {
                ReturnBySelf();
                return;
            }
        }
        float curRate = deltTime / curDuration;
        transform.position = Vector3.Lerp(curFrom, curTextJumpPath.toPos, curRate);
        transform.localScale = Vector3.Lerp(curScale, curTextJumpPath.toScale, curRate);
    }

    /// <summary>
    /// 对象自主归还对象池  by吴江
    /// </summary>
    public void ReturnBySelf()
    {
        if (GameCenter.spawner != null)
        {
            GameCenter.spawner.DespawnStateTexter(this);
        }
    }

}
