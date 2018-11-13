using System.Collections;
using System.Collections.Generic;

public class pt_update_find_friend_d710 : st.net.NetBase.Pt {
	public pt_update_find_friend_d710()
	{
		Id = 0xD710;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_find_friend_d710();
	}
	public int uid;
	public string name;
	public int lev;
	public int prof;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		uid = reader.Read_int();
		name = reader.Read_str();
		lev = reader.Read_int();
		prof = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(uid);
		writer.write_str(name);
		writer.write_int(lev);
		writer.write_int(prof);
		return writer.data;
	}

}
