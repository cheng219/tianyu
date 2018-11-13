using System.Collections;
using System.Collections.Generic;

public class pt_companion_d591 : st.net.NetBase.Pt {
	public pt_companion_d591()
	{
		Id = 0xD591;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_companion_d591();
	}
	public int uid;
	public int target_id;
	public int keepsake_id;
	public int keepsake_type;
	public int create_time;
	public int keepsake_lev;
	public int keepsake_exp;
	public int designation;
	public int target_lev;
	public string target_name;
	public int target_prof;
	public int target_online_state;
	public int intimacy;
	public int res_copy_num;
	public int marry;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		uid = reader.Read_int();
		target_id = reader.Read_int();
		keepsake_id = reader.Read_int();
		keepsake_type = reader.Read_int();
		create_time = reader.Read_int();
		keepsake_lev = reader.Read_int();
		keepsake_exp = reader.Read_int();
		designation = reader.Read_int();
		target_lev = reader.Read_int();
		target_name = reader.Read_str();
		target_prof = reader.Read_int();
		target_online_state = reader.Read_int();
		intimacy = reader.Read_int();
		res_copy_num = reader.Read_int();
		marry = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(uid);
		writer.write_int(target_id);
		writer.write_int(keepsake_id);
		writer.write_int(keepsake_type);
		writer.write_int(create_time);
		writer.write_int(keepsake_lev);
		writer.write_int(keepsake_exp);
		writer.write_int(designation);
		writer.write_int(target_lev);
		writer.write_str(target_name);
		writer.write_int(target_prof);
		writer.write_int(target_online_state);
		writer.write_int(intimacy);
		writer.write_int(res_copy_num);
		writer.write_int(marry);
		return writer.data;
	}

}
