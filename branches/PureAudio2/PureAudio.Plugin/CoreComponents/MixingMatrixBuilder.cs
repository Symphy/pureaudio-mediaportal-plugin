#region Copyright (C) 2005-2010 Team MediaPortal

// Copyright (C) 2005-2010 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Collections.Generic;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;

namespace MediaPortal.Plugins.PureAudio
{
  public partial class BassPlayer
  {
    private partial class InputSourceHolder
    {
      private partial class MixingMatrix
      {
        private class MixingMatrixBuilder
        {

          #region Fields

          private MixingMatrix _MixingMatrix;

          #endregion

          #region Public members

          public MixingMatrixBuilder(MixingMatrix mixingMatrix)
          {
            _MixingMatrix = mixingMatrix;
          }
          
          public void BuildMixingMatrix()
          {
            ChannelAssignmentDef inputAssignmentDef = _MixingMatrix._InputAssignmentInfo.AssignmentDef;
            ChannelAssignmentDef outputAssignmentDef = _MixingMatrix._OutputAssignmentInfo.AssignmentDef;
            bool upmix = _MixingMatrix._Upmix;

            if (inputAssignmentDef == outputAssignmentDef)
            {
              // No mixing required
              _MixingMatrix._BassMixMatrix = null;
              _MixingMatrix._Scaling = 0;
            }
            else
            {
              float[,] matrix = BuildMatrix(_MixingMatrix._InputAssignmentInfo, _MixingMatrix._OutputAssignmentInfo, upmix);
              _MixingMatrix._BassMixMatrix = matrix;
              _MixingMatrix._Scaling = GetAntiClipScaling(matrix);
            }
          }

          #endregion

          #region Private members

          private float[,] BuildMatrix(IChannelAssignmentInfo inputAssignmentInfo, IChannelAssignmentInfo outputAssignmentInfo, bool upmix)
          {
            Mapping mapping;

            if (upmix)
              mapping = BuildUpmixMapping(inputAssignmentInfo, outputAssignmentInfo);
            else
              mapping = BuildMapping(inputAssignmentInfo, outputAssignmentInfo);

            // This adjustment can be deleted because now it is done with a DSP_Gain after mixing.
            
            //// For inputchannels that are mapped more then once; adjust the mixinglevel.
            //// The total final acoustic level should equal 0dB

            //ChannelName[] channelNames = (ChannelName[])Enum.GetValues(typeof(ChannelName));
            //foreach (ChannelName inputChannelName in channelNames)
            //{
            //  // Get all mappings for this inputchannel
            //  List<MappingItem> items = mapping.FindAll(i => i.InputChannelName == inputChannelName);
            //  if (items.Count > 0)
            //  {
            //    // Calculate mixing level for this inputchannel
            //    // Acoustic level increases with mixing level^2!
            //    float adjustment = (float)(1d / Math.Sqrt(items.Count));
            //    double dBAdjustment = Math.Round((20d * Math.Log10(adjustment)), 1);

            //    Log.Debug("MatrixBuilder: Adjusting mixinglevel for inputchannel {0} with {1}dB", inputChannelName, dBAdjustment);

            //    // Set mixing level
            //    items.ForEach(i => i.Level = i.Level * adjustment);
            //  }
            //}

            // Create a mixing matrix from the mapping
            float[,] matrix = new float[outputAssignmentInfo.Channels.Count, inputAssignmentInfo.Channels.Count];
            for (int inputChannelIndex = 0; inputChannelIndex < inputAssignmentInfo.Channels.Count; inputChannelIndex++)
            {
              ChannelName inputChannelName = inputAssignmentInfo.Channels[inputChannelIndex];
              for (int outputChannelIndex = 0; outputChannelIndex < outputAssignmentInfo.Channels.Count; outputChannelIndex++)
              {
                ChannelName outputChannelName = outputAssignmentInfo.Channels[outputChannelIndex];
                MappingItem item = mapping.Find(i => i.InputChannelName == inputChannelName && i.OutputChannelName == outputChannelName);
                if (item != null)
                {
                  matrix[outputChannelIndex, inputChannelIndex] = item.Level;
                }
              }
            }
            return matrix;
          }

          private Mapping BuildMapping(IChannelAssignmentInfo inputAssignmentInfo, IChannelAssignmentInfo outputAssignmentInfo)
          {
            Mapping mapping = new Mapping();
            OutputAssignment outputAssignment = new OutputAssignment(outputAssignmentInfo);

            foreach (ChannelName channelName in inputAssignmentInfo.Channels)
            {
              switch (channelName)
              {
                case ChannelName.Lf:
                  if (!mapping.AttemptMapping(outputAssignment, channelName, channelName))
                    mapping.AttemptMapping(outputAssignment, channelName, ChannelName.C);

                  break;

                case ChannelName.Rf:
                  if (!mapping.AttemptMapping(outputAssignment, channelName, channelName))
                    mapping.AttemptMapping(outputAssignment, channelName, ChannelName.C);

                  break;

                case ChannelName.C:
                  if (!mapping.AttemptMapping(outputAssignment, channelName, channelName))
                  {
                    mapping.AttemptMapping(outputAssignment, channelName, ChannelName.Lf);
                    mapping.AttemptMapping(outputAssignment, channelName, ChannelName.Rf);
                  }

                  break;

                case ChannelName.LFE:
                  if (!mapping.AttemptMapping(outputAssignment, channelName, channelName))
                  {
                    mapping.AttemptMapping(outputAssignment, channelName, ChannelName.C);
                    mapping.AttemptMapping(outputAssignment, channelName, ChannelName.Lf);
                    mapping.AttemptMapping(outputAssignment, channelName, ChannelName.Rf);
                  }

                  break;

                case ChannelName.Ls:
                  if (!mapping.AttemptMapping(outputAssignment, channelName, channelName))
                    if (!mapping.AttemptMapping(outputAssignment, channelName, ChannelName.Lf, -3))
                      mapping.AttemptMapping(outputAssignment, channelName, ChannelName.C, -3);

                  break;

                case ChannelName.Rs:
                  if (!mapping.AttemptMapping(outputAssignment, channelName, channelName))
                    if (!mapping.AttemptMapping(outputAssignment, channelName, ChannelName.Rf, -3))
                      mapping.AttemptMapping(outputAssignment, channelName, ChannelName.C, -3);

                  break;

                case ChannelName.Lrs:
                  if (!mapping.AttemptMapping(outputAssignment, channelName, channelName))
                    if (!mapping.AttemptMapping(outputAssignment, channelName, ChannelName.Ls))
                      if (!mapping.AttemptMapping(outputAssignment, channelName, ChannelName.Lf, -3))
                        mapping.AttemptMapping(outputAssignment, channelName, ChannelName.C, -3);

                  break;

                case ChannelName.Rrs:
                  if (!mapping.AttemptMapping(outputAssignment, channelName, channelName))
                    if (!mapping.AttemptMapping(outputAssignment, channelName, ChannelName.Rs))
                      if (!mapping.AttemptMapping(outputAssignment, channelName, ChannelName.Rf, -3))
                        mapping.AttemptMapping(outputAssignment, channelName, ChannelName.C, -3);

                  break;
              }
            }
            return mapping;
          }

          private Mapping BuildUpmixMapping(IChannelAssignmentInfo inputAssignmentInfo, IChannelAssignmentInfo outputAssignmentInfo)
          {
            Mapping mapping = new Mapping();
            OutputAssignment outputAssignment = new OutputAssignment(outputAssignmentInfo);

            // Todo: 

            return mapping;
          }

          private float GetAntiClipScaling(float[,] mixingMatring)
          {
            float dBScaling = 0f;

            if (mixingMatring != null)
            {
              // Determine maximum total mixinglevel
              float maxCoefficient = 0;
              for (int output = 0; output < mixingMatring.GetLength(0); output++)
              {
                float coefficient = 0;
                for (int input = 0; input < mixingMatring.GetLength(1); input++)
                {
                  coefficient += mixingMatring[output, input];
                }
                if (coefficient > maxCoefficient)
                  maxCoefficient = coefficient;
              }

              Log.Debug("MatrixBuilder: Calculated maximum coefficient: {0}", maxCoefficient);

              // Calculate scaling required to maximize total mixinglevel to 1.0
              // Todo: maybe this can be a bit more a compromise between the risk of clipping and volume
              dBScaling = (float)Math.Round((20d * Math.Log10(1d / maxCoefficient)), 1);
              Log.Debug("MatrixBuilder: applied anti-clipping scaling: {0}dB", dBScaling);
            }
            return dBScaling;
          }

          #endregion

          #region Nested types

          private class OutputAssignment
          {
            IChannelAssignmentInfo _ChannelAssignmentInfo;

            public IChannelAssignmentInfo ChannelAssignmentInfo
            {
              get { return _ChannelAssignmentInfo; }
            }

            public OutputAssignment(IChannelAssignmentInfo channelAssignmentInfo)
            {
              _ChannelAssignmentInfo = channelAssignmentInfo;
            }

            public bool ContainsChannel(ChannelName channelName)
            {
              return _ChannelAssignmentInfo.Channels.FindIndex(c => c == channelName) > -1;
            }
          }

          private class Mapping : List<MappingItem>
          {
            public void Add(ChannelName inputChannelName, ChannelName outputChannelName)
            {
              Add(new MappingItem(inputChannelName, outputChannelName));
            }

            public bool AttemptMapping(OutputAssignment outputAssignment, ChannelName inputChannelName, ChannelName outputChannelName)
            {
              return AttemptMapping(outputAssignment, inputChannelName, outputChannelName, 0);
            }

            public bool AttemptMapping(OutputAssignment outputAssignment, ChannelName inputChannelName, ChannelName outputChannelName, int dBLevel)
            {
              bool canMap = outputAssignment.ContainsChannel(outputChannelName);

              if (canMap)
                Add(new MappingItem(inputChannelName, outputChannelName, dBLevel));

              return canMap;
            }
          }

          private class MappingItem
          {
            ChannelName _InputChannelName;
            ChannelName _OutputChannelName;
            float _Level;

            public ChannelName InputChannelName
            {
              get { return _InputChannelName; }
            }

            public ChannelName OutputChannelName
            {
              get { return _OutputChannelName; }
            }

            public float Level
            {
              get { return _Level; }
              set { _Level = value; }
            }

            public MappingItem(ChannelName inputChannelName, ChannelName outputChannelName) :
              this(inputChannelName, outputChannelName, 0)
            {
            }

            public MappingItem(ChannelName inputChannelName, ChannelName outputChannelName, int dBLevel)
            {
              _InputChannelName = inputChannelName;
              _OutputChannelName = outputChannelName;
              _Level = (float)Math.Pow(10d, (double)dBLevel / 20d);

              Log.Debug("MatrixBuilder: Adding Mapping {0} -> {1}, {2}dB", inputChannelName, outputChannelName, dBLevel);
            }
          }
          
          #endregion

        }
      }
    }
  }
}