using System.Collections;
using System.Collections.Generic;

public class pt_chat_content_d325 : st.net.NetBase.Pt {
	public pt_chat_content_d325()
	{
		Id = 0xD325;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_chat_content_d325();
	}
	public uint type;
	public int send_uid;
	public string name;
	public string receive_name;
	public int vip_lev;
	public int prof;
	public string content;
	public int scene;
	public int x;
	public int y;
	public int z;
	public int sort;
	public int time;
	public int item_type;
	public List<st.net.NetBase.item_des> item = new List<st.net.NetBase.item_des>();
	public List<int> content_info = new List<int>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		type = reader.Read_uint();
		send_uid = reader.Read_int();
		name = reader.Read_str();
		receive_name = reader.Read_str();
		vip_lev = reader.Read_int();
		prof = reader.Read_int();
		content = reader.Read_str();
		scene = reader.Read_int();
		x = reader.Read_int();
		y = reader.Read_int();
		z = reader.Read_int();
		sort = reader.Read_int();
		time = reader.Read_int();
		item_type = reader.Read_int();
		ushort lenitem = reader.Read_ushort();
		item = new List<st.net.NetBase.item_des>();
		for(int i_item = 0 ; i_item < lenitem ; i_item ++)
		{
			st.net.NetBase.item_des listData = new st.net.NetBase.item_des();
			listData.fromBinary(reader);
			item.Add(listData);
		}
		ushort lencontent_info = reader.Read_ushort();
		content_info = new List<int>();
		for(int i_content_info = 0 ; i_content_info < lencontent_info ; i_content_info ++)
		{
			int listData = reader.Read_int();
			content_info.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(type);
		writer.write_int(send_uid);
		writer.write_str(name);
		writer.write_str(receive_name);
		writer.write_int(vip_lev);
		writer.write_int(prof);
		writer.write_str(content);
		writer.write_int(scene);
		writer.write_int(x);
		writer.write_int(y);
		writer.write_int(z);
		writer.write_int(sort);
		writer.write_int(time);
		writer.write_int(item_type);
		ushort lenitem = (ushort)item.Count;
		writer.write_short(lenitem);
		for(int i_item = 0 ; i_item < lenitem ; i_item ++)
		{
			st.net.NetBase.item_des listData = item[i_item];
			listData.toBinary(writer);
		}
		ushort lencontent_info = (ushort)content_info.Count;
		writer.write_short(lencontent_info);
		for(int i_content_info = 0 ; i_content_info < lencontent_info ; i_content_info ++)
		{
			int listData = content_info[i_content_info];
			writer.write_int(listData);
		}
		return writer.data;
	}

}
