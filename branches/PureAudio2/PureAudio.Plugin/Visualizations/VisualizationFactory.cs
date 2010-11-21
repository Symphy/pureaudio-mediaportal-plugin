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
using System.Text;

namespace MediaPortal.Plugins.PureAudio
{
  public class VisualizationFactory
  {
    public static VisualizationFactory Create(BassPlayerSettings settings)
    {
      VisualizationFactory factory = new VisualizationFactory(settings);
      return factory;
    }

    private BassPlayerSettings settings;

    public VisualizationFactory(BassPlayerSettings settings)
    {
      this.settings = settings;
    }

    public BaseVisualizationWindow GetVisualizationWindow()
    {
      BaseVisualizationWindow visualizationWindow;
      VisualizationType visualizationType = settings.VisualizationType;
      switch (visualizationType)
      {
        case VisualizationType.None:
          visualizationWindow = null;
          break;

        case VisualizationType.WMP:
          visualizationWindow = new WMPVisualizationWindow(settings);
          break;

        default:
          throw new Exception(String.Format("Unknown VisualizationType \"{0}\"", visualizationType));
      }
      return visualizationWindow;
    }
  }
}
