using Core.DTO;
using Core.Interfaces;
using Core.Modal;
using Microsoft.AspNetCore.Mvc;
using YoutubeExplode;

namespace BackendVideoD.Controllers
{
    [Route("[Controller]")]
    public class VideoDController : Controller
    {
        private readonly IVideoService _videoService;

        public VideoDController(IVideoService videoService)
        {
            _videoService = videoService;
        }


        [HttpPost("VideoDownload")]
        public async Task<IActionResult> VideoDownload([FromBody] YTVideo yTVideo)
        {
            try
            {
                var videoPath = await _videoService.FindVideo(yTVideo.UrlVideo, yTVideo.VideoQuality);

                var file = await _videoService.AdditionalSettings(yTVideo.UrlVideo);

                if (videoPath != String.Empty && file != null)
                {
                    var response = new RFileDto
                    {
                        VideoFile = videoPath,
                        ImageUrl = file.ImageUrl,
                        NameVideo = file.NameVideo,
                        Duration = file.Duration,

                    };

                    return Ok(response);

                }
                else
                {
                    
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
               
                return BadRequest(ex.Message);
            }
        } 
    }
}
