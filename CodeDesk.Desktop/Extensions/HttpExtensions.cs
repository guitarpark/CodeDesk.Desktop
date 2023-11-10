using System.Net;
using System.Runtime.InteropServices;

namespace CodeDesk.Desktop.Extensions
{
    public static class HttpExtensions
    {
        public static async Task DownloadFile(string url, string savePath, Action<double> progress)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();

                        using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                        {
                            long totalBytes = response.Content.Headers.ContentLength ?? -1;
                            long receivedBytes = 0;
                            using (FileStream fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                byte[] buffer = new byte[8192];
                                int bytesRead;

                                while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                                {
                                    await fileStream.WriteAsync(buffer, 0, bytesRead);

                                    receivedBytes += bytesRead;
                                    progress.Invoke((double)receivedBytes / totalBytes * 100);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var bbb = ex;
                }
            }
        }
        static readonly string AppScheme = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "http" : "app";
        internal static SchemeConfig ParseScheme(this Uri uri)
        {
            return new SchemeConfig()
            {
                AppScheme=AppScheme,
                AppOriginUri = new Uri($"{AppScheme}://{uri.Host}/"),
                AppOrigin = $"{AppScheme}://{uri.Host}/",
                HomePagePath = $"wwwroot/{uri.AbsolutePath}",
                HomePage = uri.AbsolutePath.Substring(1, uri.AbsolutePath.Length - 1),
            };
        }
    }
}
