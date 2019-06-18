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
            Infos.Add(new RecordInfoItem(GetRecordName(0), 0));
            //Infos.Add(new RecordInfoItem(GetRecordName(1), 1));
            //Infos.Add(new RecordInfoItem(GetRecordName(2), 2));//Infos.Add(new RecordInfoItem("承载比(CBR)", 0));
            Infos.Add(new RecordInfoItem(GetRecordName(3), 3)); //Infos.Add(new RecordInfoItem("回弹模量-强度仪法", 1));
            Infos.Add(new RecordInfoItem(GetRecordName(4), 4)); //Infos.Add(new RecordInfoItem("回弹模量-强度仪法", 1));
            Infos.Add(new RecordInfoItem(GetRecordName(5), 5));
            Infos.Add(new RecordInfoItem(GetRecordName(6), 6));
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
                case 0:
                    info = new WendingduTestInfo();
                    break;
                case 1:
                    info = new ModulusYuanInfo();
                    break;
                case 2:
                    info = new LengzhutiInfo();
                    break;
                case 3:
                    info = new WanquTestInfo();
                    break;
                case 4:
                    info = new PilieTestInfo();
                    break;
                case 5:
                    info = new DongrongInfo();
                    break;
                case 6:
                    info = new ModeInjectionInfo();
                    break;
            }
            if (info != null)
                info.SetName(GetRecordName(type));
            return info;
        }

        public static string GetRecordName(int type)
        {
            switch (type)
            {
                case 0:
                    return "稳定度抗压试验";
                case 1:
                    return "回弹模量-圆柱体试验";
                case 2:
                    return "棱柱体试验";
                case 3:
                    return "沥青混合料弯曲试验";
                case 4:
                    return "劈裂试验";
                case 5:
                    return "冻融试验";
                case 6:
                    return "贯入模式";
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
