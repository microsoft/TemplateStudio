Imports System.Runtime.CompilerServices
Imports Windows.Devices.Sensors
Imports Windows.Graphics.Display
Imports Windows.Storage.FileProperties

Namespace Helpers
    Public Module CameraOrientationExtensions

      <Extension>
      Public Function ToSimpleOrientation(displayInformation As DisplayInformation, deviceOrientation As SimpleOrientation, isFlipped As Boolean) As SimpleOrientation
          Dim result = deviceOrientation

          If displayInformation.NativeOrientation = DisplayOrientations.Portrait Then
              Select Case result
                Case SimpleOrientation.Rotated90DegreesCounterclockwise
                    result = SimpleOrientation.NotRotated
                Case SimpleOrientation.Rotated180DegreesCounterclockwise
                    result = SimpleOrientation.Rotated90DegreesCounterclockwise
                Case SimpleOrientation.Rotated270DegreesCounterclockwise
                    result = SimpleOrientation.Rotated180DegreesCounterclockwise
                Case SimpleOrientation.NotRotated
                    result = SimpleOrientation.Rotated270DegreesCounterclockwise
              End Select
          End If

          If isFlipped Then
              Select Case result
                Case SimpleOrientation.Rotated90DegreesCounterclockwise
                    Return SimpleOrientation.Rotated270DegreesCounterclockwise
                Case SimpleOrientation.Rotated270DegreesCounterclockwise
                    Return SimpleOrientation.Rotated90DegreesCounterclockwise
              End Select
          End If

          Return result
      End Function

      <Extension>
      Public Function ToDegrees(orientation As DisplayOrientations) As Integer
          Select Case orientation
              Case DisplayOrientations.Portrait
                  Return 90
              Case DisplayOrientations.LandscapeFlipped
                  Return 180
              Case DisplayOrientations.PortraitFlipped
                  Return 270
              Case Else
                  Return 0
          End Select
      End Function

      <Extension>
      Public Function ToPhotoOrientation(orientation As SimpleOrientation, isFlipped As Boolean) As PhotoOrientation
          If isFlipped Then
              Select Case orientation
                Case SimpleOrientation.Rotated90DegreesCounterclockwise
                    Return PhotoOrientation.Transverse
                Case SimpleOrientation.Rotated180DegreesCounterclockwise
                    Return PhotoOrientation.FlipVertical
                Case SimpleOrientation.Rotated270DegreesCounterclockwise
                    Return PhotoOrientation.Transpose
                Case Else
                    Return PhotoOrientation.FlipHorizontal
              End Select
          End If

          Select Case orientation
              Case SimpleOrientation.Rotated90DegreesCounterclockwise
                  Return PhotoOrientation.Rotate90
              Case SimpleOrientation.Rotated180DegreesCounterclockwise
                  Return PhotoOrientation.Rotate180
              Case SimpleOrientation.Rotated270DegreesCounterclockwise
                  Return PhotoOrientation.Rotate270
              Case Else
                  Return PhotoOrientation.Normal
          End Select
      End Function
    End Module
End Namespace
