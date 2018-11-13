using System.Collections;
using System.Collections.Generic;

public class pt_return_gem_update_d103 : st.net.NetBase.Pt {
	public pt_return_gem_update_d103()
	{
		Id = 0xD103;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_return_gem_update_d103();
	}
	public List<st.net.NetBase.gem_list> gem_list = new List<st.net.NetBase.gem_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lengem_list = reader.Read_ushort();
		gem_list = new List<st.net.NetBase.gem_list>();
		for(int i_gem_list = 0 ; i_gem_list < lengem_list ; i_gem_list ++)
		{
			st.net.NetBase.gem_list listData = new st.net.NetBase.gem_list();
			listData.fromBinary(reader);
			gem_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lengem_list = (ushort)gem_list.Count;
		writer.write_short(lengem_list);
		for(int i_gem_list = 0 ; i_gem_list < lengem_list ; i_gem_list ++)
		{
			st.net.NetBase.gem_list listData = gem_list[i_gem_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
