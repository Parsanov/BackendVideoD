using Core.Interfaces;
using System.Diagnostics;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace Application.VideoQ
{
    public class VideoQService : IVideoQ
    {

        public async Task<string> VideoHightQ(string url, YoutubeClient youtube)
        {
            try
            {
                var video = await youtube.Videos.GetAsync(url);
                var title = video.Title;
                var safeTitle = string.Join("_", title.Split(Path.GetInvalidFileNameChars()));

                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(url);
                var videoStreamInfo = streamManifest.GetVideoOnlyStreams()
                    .Where(s => s.VideoQuality.Label.StartsWith("1080"))
                    .FirstOrDefault() ?? streamManifest.GetVideoOnlyStreams()
                    .OrderByDescending(s => s.VideoQuality)
                    .FirstOrDefault();
                var audioStreamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

                if (videoStreamInfo != null && audioStreamInfo != null)
                {
                    var outputPath = Path.Combine("D:\\films", $"{safeTitle}_merged.{videoStreamInfo.Container}");
                    var videoTempPath = Path.Combine("D:\\films", $"video_{safeTitle}.{videoStreamInfo.Container}");
                    var audioTempPath = Path.Combine("D:\\films", $"audio_{safeTitle}.{audioStreamInfo.Container}");

                    await youtube.Videos.Streams.DownloadAsync(videoStreamInfo, videoTempPath);
                    await youtube.Videos.Streams.DownloadAsync(audioStreamInfo, audioTempPath);

                    var mergeSuccess = await MergeVideo(videoTempPath, audioTempPath, outputPath);

                    if (mergeSuccess)
                    {

                        File.Delete(videoTempPath);
                        File.Delete(audioTempPath);

                        return outputPath;
                    }
                    else
                    {
                        File.Delete(videoTempPath);
                        File.Delete(audioTempPath);

                        return String.Empty;
                    }
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return String.Empty;
            }
        }


        private static async Task<bool> MergeVideo(string videoPath, string audioPath, string outputPath)
        {
            try
            {
                Console.WriteLine("Starting MergeVideo method.");

                using (var ffmpeg = new Process())
                {
                    ffmpeg.StartInfo = new ProcessStartInfo
                    {
                        FileName = @"D:\Program\Ffmperg\bin\ffmpeg.exe",
                        Arguments = $"-i \"{videoPath}\" -i \"{audioPath}\" -c:v copy -c:a aac -strict experimental \"{outputPath}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    ffmpeg.Start();

                    var outputTask = ffmpeg.StandardOutput.ReadToEndAsync();
                    var errorTask = ffmpeg.StandardError.ReadToEndAsync();

                    await ffmpeg.WaitForExitAsync();

                    string output = await outputTask;
                    string error = await errorTask;

                    ffmpeg.StandardOutput.Dispose();
                    ffmpeg.StandardError.Dispose();

                    Console.WriteLine("FFmpeg process completed.");

                    return File.Exists(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in MergeVideo: {ex.Message}");
                return false;
            }
        }


        public Task<byte[]> VideoMediumQ(string Url, YoutubeClient youtube)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> VideoLowQ(string Url, YoutubeClient youtube)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> AudioQ(string Url, YoutubeClient youtube)
        {
            throw new NotImplementedException();
        }
    }
}
