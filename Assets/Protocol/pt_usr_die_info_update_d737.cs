using System.Collections;
using System.Collections.Generic;

public class pt_usr_die_info_update_d737 : st.net.NetBase.Pt {
	public pt_usr_die_info_update_d737()
	{
		Id = 0xD737;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_usr_die_info_update_d737();
	}
	public int type;
	public string kill_name;
	public int revive_num;
	public int count_down;
	public int drop_item;
	public int kill_uid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		type = reader.Read_int();
		kill_name = reader.Read_str();
		revive_num = reader.Read_int();
		count_down = reader.Read_int();
		drop_item = reader.Read_int();
		kill_uid = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(type);
		writer.write_str(kill_name);
		writer.write_int(revive_num);
		writer.write_int(count_down);
		writer.write_int(drop_item);
		writer.write_int(kill_uid);
		return writer.data;
	}

}
