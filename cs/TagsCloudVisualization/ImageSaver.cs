using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace TagsCloudVisualization;

public static class ImageSaver
{
    public static void ImageSave(Bitmap bitmap, string fileName)
    {   
        var projectDir = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName;
        var imagesDir = Path.Combine(projectDir, "Image");
        var path = Path.Combine(imagesDir, fileName);
        bitmap.Save(path, ImageFormat.Png);
    }
}