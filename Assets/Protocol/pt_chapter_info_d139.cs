using System.Collections;
using System.Collections.Generic;

public class pt_chapter_info_d139 : st.net.NetBase.Pt {
	public pt_chapter_info_d139()
	{
		Id = 0xD139;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_chapter_info_d139();
	}
	public int chapterId;
	public int chapter_task_num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		chapterId = reader.Read_int();
		chapter_task_num = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(chapterId);
		writer.write_int(chapter_task_num);
		return writer.data;
	}

}
