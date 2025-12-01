using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization.Render;

public static class ImageSaver
{
    public static void Save(Bitmap bitmap, string fileName)
    {   
        var projectDir = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName;
        var imagesDir = Path.Combine(projectDir, "Image");
        var path = Path.Combine(imagesDir, fileName);
        bitmap.Save(path, ImageFormat.Png);
    }
}