using System.Drawing;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.PointProvider;
using TagsCloudVisualization.Render;

namespace TagsCloudVisualizationTest;

public class SpiralPointProviderTest
{
    private Point validCenter;
    private Size validRectangleSize;
    private List<Rectangle> testingRectangles;
    private string projectDir;
    
    [SetUp]
    public void Setup()
    {
        validCenter = new Point(1920 / 2, 1080 / 2);
        validRectangleSize = new Size(50, 35);
        projectDir = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName;
    }

    [TearDown]
    public void TearDown()
    {
        var currentContext = TestContext.CurrentContext;
        if (currentContext.Result.Outcome.Status != TestStatus.Failed) 
            return;
        var renderer = new TagCloudRenderer(new Size(1920, 1080));
        var bitmap = renderer.CreateRectangleCloud(testingRectangles);
        var fileName = $"{currentContext.Test.Name}.png";
        var imagesDir = Path.Combine(projectDir, "Image");
        var path = Path.Combine(imagesDir, fileName);
        
        ImageSaver.Save(bitmap, fileName);
        Console.WriteLine($"Tag cloud visualization saved to file {path}");
    }
    
    [TestCase(-1)]
    [TestCase(0)]
    public void GetCircularCloudRectangles_ShouldThrowException_WhenCountIsNotPositive(int count)
    {
        var spiralPointProvider = new SpiralPointProvider(validCenter);
        
        var act = () => spiralPointProvider
            .GetRectangles(count, validRectangleSize)
            .ToList();

        act.Should().Throw<ArgumentException>()
            .WithMessage("Count must be positive");
    }

    [Test]
    public void GetCircularCloudRectangles_ShouldNotProduceIntersectingRectangles_WhenCalledMultipleTimes()
    {
        var spiralPointProvider = new SpiralPointProvider(validCenter);

        var rectangles = spiralPointProvider
            .GetRectangles(100, validRectangleSize)
            .ToList();
        testingRectangles = rectangles;
        
        for (var i = 0; i < rectangles.Count; i++)
        {
            for (var j = i + 1; j < rectangles.Count; j++)
            {
                rectangles[i].IntersectsWith(rectangles[j])
                    .Should().BeFalse($"rect #{i} should not intersect rect #{j}");
            }
        }
    }
    
    [TestCase(-1)]
    [TestCase(2)]
    public void GetRandomSizedRectangles_ShouldThrowException_WhenInvalidCoefficient(double minScaleFactor)
    {
        var spiralPointProvider = new SpiralPointProvider(validCenter);

        var act = () => spiralPointProvider
            .GetRandomSizedRectangles(100, validRectangleSize, minScaleFactor)
            .ToList();
        
        act.Should().Throw<ArgumentException>()
            .WithMessage("minScaleFactor must be between 0 and 1");
    }
    
    [TestCase(0.1)]
    [TestCase(0.2)]
    [TestCase(0.5)]
    [TestCase(0.9)]
    public void GetRandomSizedRectangles_ShouldNotProduceIntersectingRectangles_WhenValidCoefficient(double minScaleFactor)
    {
        var spiralPointProvider = new SpiralPointProvider(validCenter);

        var rectangles = spiralPointProvider
            .GetRandomSizedRectangles(100, validRectangleSize, minScaleFactor)
            .ToList();
        testingRectangles = rectangles;
        
        for (var i = 0; i < rectangles.Count; i++)
        {
            for (var j = i + 1; j < rectangles.Count; j++)
            {
                rectangles[i].IntersectsWith(rectangles[j])
                    .Should().BeFalse($"rect #{i} should not intersect rect #{j}");
            }
        }
    }

    [Test]
    public void FindMinRadius_ShouldReturnRadiusOfEnclosingCircle_WhenValidParameters()
    {
        var spiralPointProvider = new SpiralPointProvider(validCenter);
        var rectangles = spiralPointProvider
            .GetRandomSizedRectangles(100, validRectangleSize, 0.5)
            .ToList();
        testingRectangles = rectangles;
        
        var circleRadius = Geometry.FindMinRadius(rectangles, validCenter);
        
        Geometry.AreRectanglesInsideCircle(rectangles, validCenter, circleRadius).Should().BeTrue();
        Geometry.AreRectanglesInsideCircle(rectangles, validCenter, circleRadius-1).Should().BeFalse();
    }
    
    [Test]
    public void GetRandomSizedRectangles_ShouldEnsurePackingDensity_WhenValidParameters()
    {
        var spiralPointProvider = new SpiralPointProvider(validCenter);
        var rectangles = spiralPointProvider
            .GetRandomSizedRectangles(100, validRectangleSize, 0.5)
            .ToList();
        testingRectangles = rectangles;
        
        var circleRadius = Geometry.FindMinRadius(rectangles, validCenter);
        var packingDensity = Geometry.GetRectanglesPackingDensity(rectangles, circleRadius);
        
        packingDensity.Should().BeGreaterThan(0.4);
    }
}