using System.Collections;
using System.Collections.Generic;

public class pt_usr_info_property_d124 : st.net.NetBase.Pt {
	public pt_usr_info_property_d124()
	{
		Id = 0xD124;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_usr_info_property_d124();
	}
	public uint cur_hp;
	public uint cur_mp;
	public uint exp;
	public uint limit_hp;
	public uint limit_mp;
	public uint str;
	public uint agi;
	public uint sta;
	public uint wis;
	public uint spi;
	public uint atk;
	public uint def;
	public uint defIgnore;
	public uint cri;
	public uint criDmg;
	public uint tough;
	public uint hit;
	public uint dod;
	public uint cd;
	public uint dmgRate;
	public uint dmgDownRate;
	public uint blockRate;
	public uint blockDownRate;
	public uint realDmg;
	public uint stifle;
	public uint longSuffering;
	public uint moveSpd;
	public uint fighting;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		cur_hp = reader.Read_uint();
		cur_mp = reader.Read_uint();
		exp = reader.Read_uint();
		limit_hp = reader.Read_uint();
		limit_mp = reader.Read_uint();
		str = reader.Read_uint();
		agi = reader.Read_uint();
		sta = reader.Read_uint();
		wis = reader.Read_uint();
		spi = reader.Read_uint();
		atk = reader.Read_uint();
		def = reader.Read_uint();
		defIgnore = reader.Read_uint();
		cri = reader.Read_uint();
		criDmg = reader.Read_uint();
		tough = reader.Read_uint();
		hit = reader.Read_uint();
		dod = reader.Read_uint();
		cd = reader.Read_uint();
		dmgRate = reader.Read_uint();
		dmgDownRate = reader.Read_uint();
		blockRate = reader.Read_uint();
		blockDownRate = reader.Read_uint();
		realDmg = reader.Read_uint();
		stifle = reader.Read_uint();
		longSuffering = reader.Read_uint();
		moveSpd = reader.Read_uint();
		fighting = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(cur_hp);
		writer.write_int(cur_mp);
		writer.write_int(exp);
		writer.write_int(limit_hp);
		writer.write_int(limit_mp);
		writer.write_int(str);
		writer.write_int(agi);
		writer.write_int(sta);
		writer.write_int(wis);
		writer.write_int(spi);
		writer.write_int(atk);
		writer.write_int(def);
		writer.write_int(defIgnore);
		writer.write_int(cri);
		writer.write_int(criDmg);
		writer.write_int(tough);
		writer.write_int(hit);
		writer.write_int(dod);
		writer.write_int(cd);
		writer.write_int(dmgRate);
		writer.write_int(dmgDownRate);
		writer.write_int(blockRate);
		writer.write_int(blockDownRate);
		writer.write_int(realDmg);
		writer.write_int(stifle);
		writer.write_int(longSuffering);
		writer.write_int(moveSpd);
		writer.write_int(fighting);
		return writer.data;
	}

}
