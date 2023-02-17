namespace HuaweiSolar.Models.Huawei
{
    public class PlantList
    {
        public List<PlantInfo> list {get;set;}

        public int pageCount {get;set;}
        public int pageNo {get;set;}
        public int pageSize {get;set;}
        public int total {get;set;}
    }
}