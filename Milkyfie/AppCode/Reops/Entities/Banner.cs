using System.Collections.Generic;

namespace Milkyfie.AppCode.Reops.Entities
{
    public class Banners
    {
        public int BannerID { get; set; }
        public string BackLink { get; set; }
        public string Banner { get; set; }
        public float BannerNo { get; set; }
        public bool IsActive { get; set; }
        public bool IsPopup { get; set; }
    }

    public class bannerpopup
    {
        public List<Banners> banners { get; set; }
        public Banners popup { get; set; }

    }

}
