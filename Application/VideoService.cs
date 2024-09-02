using YoutubeExplode;
using Core.Interfaces;
using Core.DTO;

namespace Application
{
    public class VideoService : IVideoService
    {
        private readonly IVideoQ _videoQ;
        private readonly YoutubeClient _youtubeClient;

        public VideoService(IVideoQ videoQ, YoutubeClient youtubeClient)
        {
            _videoQ = videoQ;
            _youtubeClient = youtubeClient;
        }


        public async Task<string> FindVideo(string Url, string Quality)
        {
          return await _videoQ.VideoHightQ(Url, _youtubeClient);
        }

        public async Task<RFileDto?> AdditionalSettings(string Url)
        {
            var video = await _youtubeClient.Videos.GetAsync(Url);

            var thumbnails =  video.Thumbnails.LastOrDefault()?.Url;
            var nameVideo = video.Title;
            var duration = video.Duration;

            return new RFileDto
            {
                NameVideo = nameVideo,
                ImageUrl = thumbnails,
                Duration = duration.ToString(),
            };
        }
    }
}
