namespace HuaweiSolar.Models
{
    public class DevRealKpiData<T> where T: BaseDevRealKpiDataItemMap
    {
        public long devId { get; set; }
        public T dataItemMap { get; set; }
    }
}