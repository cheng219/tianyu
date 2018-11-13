using System.Collections;
using System.Collections.Generic;

public class pt_update_jingshi_hp_d769 : st.net.NetBase.Pt {
	public pt_update_jingshi_hp_d769()
	{
		Id = 0xD769;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_jingshi_hp_d769();
	}
	public int cur_jingshi_hp;
	public int max_jingshi_hp;
	public string cur_guild_name;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		cur_jingshi_hp = reader.Read_int();
		max_jingshi_hp = reader.Read_int();
		cur_guild_name = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(cur_jingshi_hp);
		writer.write_int(max_jingshi_hp);
		writer.write_str(cur_guild_name);
		return writer.data;
	}

}
