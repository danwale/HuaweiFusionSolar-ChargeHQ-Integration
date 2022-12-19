namespace HuaweiSolar.Models
{
    public class BaseResponse
    {
        public string buildCode
        {
            get; set;
        }

        public int failCode
        {
            get; set;
        }

        public string message
        {
            get; set;
        }

        public bool success
        {
            get; set;
        } = false;
    }
}