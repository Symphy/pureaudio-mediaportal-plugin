using System;

namespace MediaPortal.Player.PureAudio
{
  public static class MixingMatrixHelper
  {
    public static float[,] CreateMixingMatrix(int inputChannels, ConfigProfile _profile)
    {
      Log.Debug("Creating mixing matrix...");
      switch (inputChannels)
      {
        case 1:
          return CreateMonoUpMixMatrix(_profile);
        case 2:
          return CreateStereoUpMixMatrix(_profile);
        case 4:
          return CreateQuadraphonicUpMixMatrix(_profile);
        case 5:
          return CreateFiveDotZeroUpMixMatrix(_profile);
        case 6:
          return CreateFiveDotOneUpMixMatrix(_profile);
        default:
          return null;
      }
    }

    private static float[,] CreateQuadraphonicUpMixMatrix(ConfigProfile _profile)
    {
      float[,] mixMatrix = null;

      switch (_profile.QuadraphonicUpMix)
      {
        case QuadraphonicUpMix.None:
          break;

        case QuadraphonicUpMix.FiveDotOne:
          // Channel 1: left front out = left front in
          // Channel 2: right front out = right front in
          // Channel 3: centre out = left/right front in
          // Channel 4: LFE out = left/right front in
          // Channel 5: left surround out = left surround in
          // Channel 6: right surround out = right surround in
          mixMatrix = new float[6, 4];
          mixMatrix[0, 0] = 1;
          mixMatrix[1, 1] = 1;
          mixMatrix[2, 0] = 0.5f;
          mixMatrix[2, 1] = 0.5f;
          mixMatrix[3, 0] = 0.5f;
          mixMatrix[3, 1] = 0.5f;
          mixMatrix[4, 2] = 1;
          mixMatrix[5, 3] = 1;

          break;

        case QuadraphonicUpMix.SevenDotOne:
          // Channel 1: left front out = left front in
          // Channel 2: right front out = right front in
          // Channel 3: center out = left/right front in
          // Channel 4: LFE out = left/right front in
          // Channel 5: left surround out = left surround in
          // Channel 6: right surround out = right surround in
          // Channel 7: left back out = left surround in
          // Channel 8: right back out = right surround in
          mixMatrix = new float[8, 4];
          mixMatrix[0, 0] = 1;
          mixMatrix[1, 1] = 1;
          mixMatrix[2, 0] = 0.5f;
          mixMatrix[2, 1] = 0.5f;
          mixMatrix[3, 0] = 0.5f;
          mixMatrix[3, 1] = 0.5f;
          mixMatrix[4, 2] = 1;
          mixMatrix[5, 3] = 1;
          mixMatrix[6, 2] = 1;
          mixMatrix[7, 3] = 1;

          break;
      }
      return mixMatrix;
    }

    private static float[,] CreateFiveDotOneUpMixMatrix(ConfigProfile _profile)
    {
      float[,] mixMatrix = null;
      switch (_profile.FiveDotOneUpMix)
      {
        case FiveDotOneUpMix.None:
          break;

        case FiveDotOneUpMix.SevenDotOne:
          mixMatrix = new float[8, 6];
          mixMatrix[0, 0] = 1;
          mixMatrix[1, 1] = 1;
          mixMatrix[2, 2] = 1;
          mixMatrix[3, 3] = 1;
          mixMatrix[4, 4] = 1;
          mixMatrix[5, 5] = 1;
          mixMatrix[6, 4] = 1;
          mixMatrix[7, 5] = 1;

          break;
      }
      return mixMatrix;
    }

    private static float[,] CreateFiveDotZeroUpMixMatrix(ConfigProfile _profile)
    {
      float[,] mixMatrix = null;
      switch (_profile.FiveDotZeroUpMix)
      {
        case FiveDotZeroUpMix.None:
          break;

        case FiveDotZeroUpMix.FiveDotOne:
          mixMatrix = new float[6, 5];
          mixMatrix[0, 0] = 1;
          mixMatrix[1, 1] = 1;
          mixMatrix[2, 2] = 1;
          mixMatrix[4, 3] = 1;
          mixMatrix[5, 4] = 1;

          break;

        case FiveDotZeroUpMix.SevenDotOne:
          mixMatrix = new float[8, 5];
          mixMatrix[0, 0] = 1;
          mixMatrix[1, 1] = 1;
          mixMatrix[2, 2] = 1;
          mixMatrix[4, 3] = 1;
          mixMatrix[5, 4] = 1;
          mixMatrix[6, 3] = 1;
          mixMatrix[7, 4] = 1;

          break;
      }
      return mixMatrix;
    }

    private static float[,] CreateMonoUpMixMatrix(ConfigProfile _profile)
    {
      float[,] mixMatrix = null;

      switch (_profile.MonoUpMix)
      {
        case MonoUpMix.None:
          break;

        case MonoUpMix.Stereo:
          // Channel 1: left front out = in
          // Channel 2: right front out = in
          mixMatrix = new float[2, 1];
          mixMatrix[0, 0] = 1;
          mixMatrix[1, 0] = 1;

          break;

        case MonoUpMix.QuadraphonicPhonic:
          // Channel 1: left front out = in
          // Channel 2: right front out = in
          // Channel 3: left rear out = in
          // Channel 4: right rear out = in
          mixMatrix = new float[4, 1];
          mixMatrix[0, 0] = 1;
          mixMatrix[1, 0] = 1;
          mixMatrix[2, 0] = 1;
          mixMatrix[3, 0] = 1;

          break;

        case MonoUpMix.FiveDotOne:
          // Channel 1: left front out = in
          // Channel 2: right front out = in
          // Channel 3: centre out = in
          // Channel 4: LFE out = in
          // Channel 5: left rear/side out = in
          // Channel 6: right rear/side out = in
          mixMatrix = new float[6, 1];
          mixMatrix[0, 0] = 1;
          mixMatrix[1, 0] = 1;
          mixMatrix[2, 0] = 1;
          mixMatrix[3, 0] = 1;
          mixMatrix[4, 0] = 1;
          mixMatrix[5, 0] = 1;

          break;

        case MonoUpMix.SevenDotOne:
          // Channel 1: left front out = in
          // Channel 2: right front out = in
          // Channel 3: centre out = in
          // Channel 4: LFE out = in
          // Channel 5: left rear/side out = in
          // Channel 6: right rear/side out = in
          // Channel 7: left-rear center out = in
          // Channel 8: right-rear center out = in
          mixMatrix = new float[8, 1];
          mixMatrix[0, 0] = 1;
          mixMatrix[1, 0] = 1;
          mixMatrix[2, 0] = 1;
          mixMatrix[3, 0] = 1;
          mixMatrix[4, 0] = 1;
          mixMatrix[5, 0] = 1;
          mixMatrix[6, 0] = 1;
          mixMatrix[7, 0] = 1;

          break;
      }
      return mixMatrix;
    }

    private static float[,] CreateStereoUpMixMatrix(ConfigProfile _profile)
    {
      float[,] mixMatrix = null;

      switch (_profile.StereoUpMix)
      {
        case StereoUpMix.None:
          break;

        case StereoUpMix.QuadraphonicPhonic:
          // Channel 1: left front out = left in
          // Channel 2: right front out = right in
          // Channel 3: left rear out = left in
          // Channel 4: right rear out = right in
          mixMatrix = new float[4, 2];
          mixMatrix[0, 0] = 1;
          mixMatrix[1, 1] = 1;
          mixMatrix[2, 0] = 1;
          mixMatrix[3, 1] = 1;

          break;

        case StereoUpMix.FiveDotOne:
          // Channel 1: left front out = left in
          // Channel 2: right front out = right in
          // Channel 3: centre out = left/right in
          // Channel 4: LFE out = left/right in
          // Channel 5: left rear/side out = left in
          // Channel 6: right rear/side out = right in
          mixMatrix = new float[6, 2];
          mixMatrix[0, 0] = 1;
          mixMatrix[1, 1] = 1;
          mixMatrix[2, 0] = 0.5f;
          mixMatrix[2, 1] = 0.5f;
          mixMatrix[3, 0] = 0.5f;
          mixMatrix[3, 1] = 0.5f;
          mixMatrix[4, 0] = 1;
          mixMatrix[5, 1] = 1;

          break;

        case StereoUpMix.SevenDotOne:
          // Channel 1: left front out = left in
          // Channel 2: right front out = right in
          // Channel 3: centre out = left/right in
          // Channel 4: LFE out = left/right in
          // Channel 5: left rear/side out = left in
          // Channel 6: right rear/side out = right in
          // Channel 7: left-rear center out = left in
          // Channel 8: right-rear center out = right in
          mixMatrix = new float[8, 2];
          mixMatrix[0, 0] = 1;
          mixMatrix[1, 1] = 1;
          mixMatrix[2, 0] = 0.5f;
          mixMatrix[2, 1] = 0.5f;
          mixMatrix[3, 0] = 0.5f;
          mixMatrix[3, 1] = 0.5f;
          mixMatrix[4, 0] = 1;
          mixMatrix[5, 1] = 1;
          mixMatrix[6, 0] = 1;
          mixMatrix[7, 1] = 1;

          break;
      }
      return mixMatrix;
    }

  }
}
