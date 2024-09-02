using Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IVideoService
    {
        public Task<string> FindVideo(string Url, string Quality);
        public Task<RFileDto> AdditionalSettings(string Url);
    }
}
