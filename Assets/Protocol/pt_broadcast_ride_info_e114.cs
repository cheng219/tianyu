using System.Collections;
using System.Collections.Generic;

public class pt_broadcast_ride_info_e114 : st.net.NetBase.Pt {
	public pt_broadcast_ride_info_e114()
	{
		Id = 0xE114;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_broadcast_ride_info_e114();
	}
	public uint uid;
	public byte ride_state;
	public uint ride_type;
	public uint currskin;
	public uint eq1;
	public uint eq2;
	public uint eq3;
	public uint eq4;
	public uint eq5;
	public uint eq6;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		uid = reader.Read_uint();
		ride_state = reader.Read_byte();
		ride_type = reader.Read_uint();
		currskin = reader.Read_uint();
		eq1 = reader.Read_uint();
		eq2 = reader.Read_uint();
		eq3 = reader.Read_uint();
		eq4 = reader.Read_uint();
		eq5 = reader.Read_uint();
		eq6 = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(uid);
		writer.write_byte(ride_state);
		writer.write_int(ride_type);
		writer.write_int(currskin);
		writer.write_int(eq1);
		writer.write_int(eq2);
		writer.write_int(eq3);
		writer.write_int(eq4);
		writer.write_int(eq5);
		writer.write_int(eq6);
		return writer.data;
	}

}
