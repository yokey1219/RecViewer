using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordFileUtil
{
    public class RecordInfoFactory
    {
        public static List<RecordInfoItem> Infos;

        static RecordInfoFactory()
        {
            Infos = new List<RecordInfoItem>();
            //Infos.Add(new RecordInfoItem(GetRecordName(0), 0)); //Infos.Add(new RecordInfoItem("承载比(CBR)", 0));
            Infos.Add(new RecordInfoItem(GetRecordName(3), 3)); //Infos.Add(new RecordInfoItem("回弹模量-强度仪法", 1));
            //Infos.Add(new RecordInfoItem(GetRecordName(2), 2)); //Infos.Add(new RecordInfoItem("无侧限抗压强度", 2));
            //Infos.Add(new RecordInfoItem(GetRecordName(3), 3)); //Infos.Add(new RecordInfoItem("回弹模量-顶面法", 3));
        }


        public static AbstractRecordInfo CreateInfo(String name)
        {
            RecordInfoItem item=Infos.Find(o => o.Name.Equals(name));
            if (item != null)
            {
                //AbstractRecordInfo info = CreateInfo(item.InfoType);
                
                return CreateInfo(item.InfoType);
            }
            else
                return null;
        }

        public static AbstractRecordInfo CreateInfo(int type)
        {
            AbstractRecordInfo info = null;
            switch (type)
            {
                case 3:
                    info = new ModulusStrengthInfo();
                    break;
                //case 3:
                //    info = new ModulusStrengthInfo();
                //    break;
                //case 2:
                //    info = new StrengthRecordInfo();
                //    break;
                //case 3:
                //    info = new ModulusTopRecordInfo();
                //    break;
            }
            if (info != null)
                info.SetName(GetRecordName(type));
            return info;
        }

        public static string GetRecordName(int type)
        {
            switch (type)
            {
                case 3:
                    return "沥青混合料弯曲试验";
                    //break;
                //case 1:
                 //   return "回弹模量-强度仪法";
                    //break;
                //case 2:
                 //   return "无侧限抗压强度";
                    //break;
                //case 3:
                 //   return "回弹模量-顶面法";
                    //break;
                default:
                    return "unkown";
                    //break;
            }
        }

        public static string GetRecordName(byte type)
        {
            return GetRecordName(Convert.ToInt32(type));
        }
    }

    public class RecordInfoItem
    {
        protected String name;
        protected int infotype;
        public String Name { get { return name; } }
        public int InfoType { get { return infotype; } }
        public RecordInfoItem(String name, int infotype)
        {
            this.name = name;
            this.infotype = infotype;
        }
    }
}
