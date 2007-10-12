//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2006 Keio University
//
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//
// E-Cell is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// E-Cell is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public
// License along with E-Cell -- see the file COPYING.
// If not, write to the Free Software Foundation, Inc.,
// 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//
//END_HEADER
//
// written by Motokazu Ishikawa <m.ishikawa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo;
using EcellLib.PathwayWindow;
using UMD.HCIL.Piccolo.Activities;

namespace EcellLib.PathwayWindow.Handler
{
    /// <summary>
    /// This class handles zoom events.
    /// </summary>
    public class PPathwayZoomEventHandler : PDragSequenceEventHandler
    {
        #region Static readonly
        /// <summary>
        /// Minimum scale.
        /// </summary>
        public static readonly float MIN_SCALE = .1f;

        /// <summary>
        /// Maximum scale
        /// </summary>
        public static readonly float MAX_SCALE = 5;
        #endregion

        #region Fields
        /// <summary>
        /// The PathwayView instance
        /// </summary>
        protected PathwayControl m_view;
        
        /// <summary>
        /// The point where the mouse is down.
        /// </summary>
		private PointF m_viewZoomPoint;
		#endregion

		#region Constructors
		/// <summary>
		/// Constructs a new PZoomEventHandler.
		/// </summary>
		public PPathwayZoomEventHandler(PathwayControl view) {
			this.AcceptsEvent = new AcceptsEventDelegate(PPathwayZoomEventHandlerAcceptsEvent);
            m_view = view;
		}
		#endregion

		#region Accessors
        /// <summary>
        /// Accessor for PathwayView, which this handler controls
        /// </summary>
        public PathwayControl View
        {
            get { return m_view; }
            set { m_view = value; }
        }
		/// <summary>
		/// Gets or sets the minimum view magnification factor that this event handler is
		/// bound by.
		/// </summary>
		/// <value>The minimum view magnification factor.</value>
		/// <remarks>
		/// When this property is set the camera is left at its current scale evem if the
		/// value is larger than the current scale.
		/// <para>
		/// The value must be greater than 0.
		/// </para>
		/// </remarks>
		/*public virtual float MinScale {
			get { return m_minScale; }
			set {m_minScale = value; }
		}*/

		/// <summary>
		/// Gets or sets the maximum view magnification factor that this event handler is
		/// bound by.
		/// </summary>
		/// <value>The maximum view magnification factor.</value>
		/// <remarks>
		/// When this property is set the camera is left at its current scale even if
		/// the value is smaller than the current scale.
		/// <para>
		/// The value must be greater than 0.
		/// </para>
		/// </remarks>
		/*public virtual float MaxScale {
			get { return m_maxScale; }
			set {m_maxScale = value; }
		}*/
        #endregion
	    
        /// <summary>
        /// Return true if this handler handle an event.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected virtual bool PPathwayZoomEventHandlerAcceptsEvent(PInputEventArgs e)
        {
            if (!e.Handled && e.IsMouseEvent && e.Button == MouseButtons.Right)
            {
                return true;
            }
            else
            {
                if (null != base.DragActivity)
                    base.DragActivity.Terminate();
                return false;
            }
        }

        #region Events

        /// <summary>
		/// Overridden.  See <see cref="PDragSequenceEventHandler.OnDragActivityFirstStep">
		/// PDragSequenceEventHandler.OnDragActivityFirstStep</see>.
		/// </summary>
		protected override void OnDragActivityFirstStep(object sender, PInputEventArgs e) {
			m_viewZoomPoint = e.Position;
			base.OnDragActivityFirstStep(sender, e);
        }

		/// <summary>
		/// Overridden.  See <see cref="PDragSequenceEventHandler.OnDragActivityStep">
		/// PDragSequenceEventHandler.OnDragActivityStep</see>.
		/// </summary>
		protected override void OnDragActivityStep(object sender, PInputEventArgs e)
        {
            
			base.OnDragActivityStep(sender, e);

			PCamera camera = e.Camera;
			float dx = e.CanvasPosition.X - MousePressedCanvasPoint.X;
			float scaleDelta = (1.0f + (0.001f * dx));		

			float currentScale = camera.ViewScale;
			float newScale = currentScale * scaleDelta;

			if (newScale < MIN_SCALE) {
                scaleDelta = MIN_SCALE / currentScale;
			}

            if ((MAX_SCALE > 0) && (newScale > MAX_SCALE))
            {
                scaleDelta = MAX_SCALE / currentScale;
			}

			camera.ScaleViewBy(scaleDelta, m_viewZoomPoint.X, m_viewZoomPoint.Y);
		}

        protected override void OnDrag(object sender, PInputEventArgs e)
        {
            base.OnDrag(sender, e);
            m_view.CanvasDictionary[e.Canvas.Name].PathwayCanvas.ContextMenuStrip = null;
        }

        /// <summary>
        /// Called when drag finished.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnEndDrag(object sender, PInputEventArgs e)
        {
            base.OnEndDrag(sender,e);
            if (m_view != null)
            {                
                m_view.UpdateOverview();
            }
        }
		#endregion
    }
}