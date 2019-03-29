using Windows.Devices.Sensors;
using Windows.Graphics.Display;
using Windows.Storage.FileProperties;

namespace Param_RootNamespace.Helpers
{
    public static class CameraOrientationExtensions
    {
        public static SimpleOrientation ToSimpleOrientation(this DisplayInformation displayInformation, SimpleOrientation deviceOrientation, bool isFlipped)
        {
            var result = deviceOrientation;

            if (displayInformation.NativeOrientation == DisplayOrientations.Portrait)
            {
                switch (result)
                {
                    case SimpleOrientation.Rotated90DegreesCounterclockwise:
                        result = SimpleOrientation.NotRotated;
                        break;
                    case SimpleOrientation.Rotated180DegreesCounterclockwise:
                        result = SimpleOrientation.Rotated90DegreesCounterclockwise;
                        break;
                    case SimpleOrientation.Rotated270DegreesCounterclockwise:
                        result = SimpleOrientation.Rotated180DegreesCounterclockwise;
                        break;
                    case SimpleOrientation.NotRotated:
                        result = SimpleOrientation.Rotated270DegreesCounterclockwise;
                        break;
                }
            }

            if (isFlipped)
            {
                switch (result)
                {
                    case SimpleOrientation.Rotated90DegreesCounterclockwise:
                        return SimpleOrientation.Rotated270DegreesCounterclockwise;
                    case SimpleOrientation.Rotated270DegreesCounterclockwise:
                        return SimpleOrientation.Rotated90DegreesCounterclockwise;
                }
            }

            return result;
        }

        public static int ToDegrees(this DisplayOrientations orientation)
        {
            switch (orientation)
            {
                case DisplayOrientations.Portrait:
                    return 90;
                case DisplayOrientations.LandscapeFlipped:
                    return 180;
                case DisplayOrientations.PortraitFlipped:
                    return 270;
                default:
                    return 0;
            }
        }

        public static PhotoOrientation ToPhotoOrientation(this SimpleOrientation orientation, bool isFlipped)
        {
            if (isFlipped)
            {
                switch (orientation)
                {
                    case SimpleOrientation.Rotated90DegreesCounterclockwise:
                        return PhotoOrientation.Transverse;
                    case SimpleOrientation.Rotated180DegreesCounterclockwise:
                        return PhotoOrientation.FlipVertical;
                    case SimpleOrientation.Rotated270DegreesCounterclockwise:
                        return PhotoOrientation.Transpose;
                    default:
                        return PhotoOrientation.FlipHorizontal;
                }
            }

            switch (orientation)
            {
                case SimpleOrientation.Rotated90DegreesCounterclockwise:
                    return PhotoOrientation.Rotate90;
                case SimpleOrientation.Rotated180DegreesCounterclockwise:
                    return PhotoOrientation.Rotate180;
                case SimpleOrientation.Rotated270DegreesCounterclockwise:
                    return PhotoOrientation.Rotate270;
                default:
                    return PhotoOrientation.Normal;
            }
        }
    }
}
