﻿using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicElementSymbolPicker.PointTools
{
  internal class CloudPointTool : LayoutTool
  {
    public CloudPointTool()
    {
      SketchType = SketchGeometryType.Point;
    }
    protected override Task OnToolActivateAsync(bool active)
    {
      return base.OnToolActivateAsync(active);
    }
    protected override Task<bool> OnSketchCompleteAsync(Geometry geometry)
    {
      if (ActiveElementContainer == null)
        Task.FromResult(true);
     return  QueuedTask.Run(() =>
      {
        var marker = SymbolFactory.Instance.ConstructMarker(CIMColor.CreateRGBColor(0, 0, 255), 20, SimpleMarkerStyle.Cloud);
        //cast CIMMarker to CIMVectorMarker 
        var cimVectorMarker = marker as CIMVectorMarker;

        if (cimVectorMarker == null) return true;

        //Create a PointSymbol using the CIMVector marker
        var newPointSymbol = SymbolFactory.Instance.ConstructPointSymbol(cimVectorMarker);

        //Create CIMPointGraphic - using the point Symbol.
        var cimGraphic = new CIMPointGraphic
        {
          Symbol = newPointSymbol.MakeSymbolReference(),
          Location = geometry as MapPoint
        };
        //Create GraphicsElement using the CIMPointGraphic.
        var ge = LayoutElementFactory.Instance.CreateGraphicElement(this.ActiveElementContainer, cimGraphic);
        return true;
      });
    }
  }
}
