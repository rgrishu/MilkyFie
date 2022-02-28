namespace GenricFrame.AppCode.Reops.Entities
{
    public class News
    {
        public int NewsID { get; set; }
        public string NewsTitle { get; set; }
        public string NewsDescription { get; set; }
        public  bool IsActive { get; set; }
        public  bool IsAutoExpired { get; set; }
    }
}
