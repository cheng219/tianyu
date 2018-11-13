using System.Collections;
using System.Collections.Generic;

public class pt_ret_ride_prop_e112 : st.net.NetBase.Pt {
	public pt_ret_ride_prop_e112()
	{
		Id = 0xE112;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_ret_ride_prop_e112();
	}
	public List<st.net.NetBase.prop_entry> props = new List<st.net.NetBase.prop_entry>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenprops = reader.Read_ushort();
		props = new List<st.net.NetBase.prop_entry>();
		for(int i_props = 0 ; i_props < lenprops ; i_props ++)
		{
			st.net.NetBase.prop_entry listData = new st.net.NetBase.prop_entry();
			listData.fromBinary(reader);
			props.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenprops = (ushort)props.Count;
		writer.write_short(lenprops);
		for(int i_props = 0 ; i_props < lenprops ; i_props ++)
		{
			st.net.NetBase.prop_entry listData = props[i_props];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
