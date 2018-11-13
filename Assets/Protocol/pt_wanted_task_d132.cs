using System.Collections;
using System.Collections.Generic;

public class pt_wanted_task_d132 : st.net.NetBase.Pt {
	public pt_wanted_task_d132()
	{
		Id = 0xD132;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_wanted_task_d132();
	}
	public uint wanted_task_id;
	public uint wanted_task_step;
	public uint wanted_task_star;
	public uint wanted_task_loop;
	public uint five_fold_state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		wanted_task_id = reader.Read_uint();
		wanted_task_step = reader.Read_uint();
		wanted_task_star = reader.Read_uint();
		wanted_task_loop = reader.Read_uint();
		five_fold_state = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(wanted_task_id);
		writer.write_int(wanted_task_step);
		writer.write_int(wanted_task_star);
		writer.write_int(wanted_task_loop);
		writer.write_int(five_fold_state);
		return writer.data;
	}

}
