/*
 * Nasa APOD Background Changer
 * 
 * Jared Healy, All rights reserved (c) 2022
 * 
 * A small program which changes the background of the computer to 
 * Nasa's Astronomy photo of the day.
 * 
 * 9/02/2022; Commented through all code
 * 
 */


using System.Text.Json;
namespace Nasa
{
    class PicDownloader
    {
        static readonly HttpClient client = new();
        internal static string? apiKey = null;


        /**
         * <summary>Gets the Nasa astronomy photo of the day, saves it to the desktop and sets it as the wallpaper</summary>
         */
        public async static Task SetTodaysPhoto()
        {
            if (apiKey == null) return;
            try
            {
                string imageUrl = await GetTodaysPhotoLink();
                byte[] ret = await DownloadPhoto(imageUrl);
                string path = Directory.GetCurrentDirectory();
                File.WriteAllBytes($"{path}/bg.png", ret);
                var uri = new Uri($"{path}/bg.png");
                Wallpaper.Set(uri, Wallpaper.Style.Fill);
            }
            catch
            {
                //ignored
            }
        }

        internal static async void EventReciever(object? sender, EventArgs e)
        {
            await SetTodaysPhoto();
        }

        /**
         * <summary>Fetches the current astronomy picture of the day link using NASA's Api, returning the link as a string</summary>
         * <returns>Url to image as a string</returns>
         */
        private static async Task<string> GetTodaysPhotoLink()
        {
            string json = await client.GetStringAsync($"https://api.nasa.gov/planetary/apod?api_key={apiKey}");
            //string json = await client.GetStringAsync($"https://192.168.0.18:80/hello?world={apiKey}");
            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json)!;
            if (dict["media_type"] == "video")
            {
                throw new Exception("Today's Photo is a video");
            }
            string imageUrl = dict["url"];
            return imageUrl;
        }


        /**
         * <summary>Takes and image from a url and returns it as a byte array</summary>
         * <param name="url">the url to the image to get as bytes</param>
         * <returns>The bytes from the image located at the url as byte array</returns>
         */
        private static async Task<byte[]> DownloadPhoto(string url)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            byte[] content = await response.Content.ReadAsByteArrayAsync();
            return content;
        }
    }
}
