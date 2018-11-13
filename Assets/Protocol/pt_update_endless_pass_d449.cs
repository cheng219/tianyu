using System.Collections;
using System.Collections.Generic;

public class pt_update_endless_pass_d449 : st.net.NetBase.Pt {
	public pt_update_endless_pass_d449()
	{
		Id = 0xD449;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_endless_pass_d449();
	}
	public int chapter_id;
	public int pass_id;
	public int star_num;
	public int time;
	public int state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		chapter_id = reader.Read_int();
		pass_id = reader.Read_int();
		star_num = reader.Read_int();
		time = reader.Read_int();
		state = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(chapter_id);
		writer.write_int(pass_id);
		writer.write_int(star_num);
		writer.write_int(time);
		writer.write_int(state);
		return writer.data;
	}

}
