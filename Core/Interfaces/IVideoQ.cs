using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;

namespace Core.Interfaces
{
    public interface IVideoQ
    {
        Task<string> VideoHightQ(string Url, YoutubeClient youtube);
        Task<byte[]> VideoMediumQ(string Url, YoutubeClient youtube);
        Task<byte[]> VideoLowQ(string Url, YoutubeClient youtube);
        Task<byte[]> AudioQ(string Url, YoutubeClient youtube);
    }
}
