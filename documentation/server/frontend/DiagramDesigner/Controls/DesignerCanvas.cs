﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using System.Linq;

namespace DiagramDesigner
{
    public class DesignerCanvas : Canvas
    {

        private ConnectorViewModel partialConnection;
        private List<Connector> connectorsHit = new List<Connector>();
        private Connector sourceConnector;
        private Point? rubberbandSelectionStartPoint = null;

        public DesignerCanvas()
        {
            this.AllowDrop = true;
            Mediator.Instance.Register(this);
        }


        public Connector SourceConnector
        {
            get { return sourceConnector; }
            set
            {
                if (sourceConnector != value)
                {
                    if (value.Orientation == ConnectorOrientation.Input)
                    {
                        MessageBox.Show("Der Start einer Verbindung aus einem Eingang heraus ist nicht erlaubt.");
                    }
                    else
                    {
                        sourceConnector = value;
                        connectorsHit.Add(sourceConnector);
                        FullyCreatedConnectorInfo sourceDataItem = sourceConnector.DataContext as FullyCreatedConnectorInfo;

                        Rect rectangleBounds = sourceConnector.TransformToVisual(this).TransformBounds(new Rect(sourceConnector.RenderSize));
                        Point point = new Point(rectangleBounds.Left + (rectangleBounds.Width / 2),
                                                rectangleBounds.Bottom + (rectangleBounds.Height / 2));
                        partialConnection = new ConnectorViewModel(sourceDataItem, new PartCreatedConnectionInfo(point));
                        sourceDataItem.DataItem.Parent.AddItemCommand.Execute(partialConnection);
                    }
                }
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //if we are source of event, we are rubberband selecting
                if (e.Source == this)
                {
                    // in case that this click is the start for a 
                    // drag operation we cache the start point
                    rubberbandSelectionStartPoint = e.GetPosition(this);

                    IDiagramViewModel vm = (this.DataContext as IDiagramViewModel);
                    if (!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                    {
                        vm.ClearSelectedItemsCommand.Execute(null);
                    }
                    e.Handled = true;
                }
            }


        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            Mediator.Instance.NotifyColleagues<bool>("DoneDrawingMessage", true);

            if (sourceConnector != null)
            {
                FullyCreatedConnectorInfo sourceDataItem = sourceConnector.DataContext as FullyCreatedConnectorInfo;
                if (connectorsHit.Count() == 2)
                {
                    Connector sinkConnector = connectorsHit.Last();
                    FullyCreatedConnectorInfo sinkDataItem = sinkConnector.DataContext as FullyCreatedConnectorInfo;
                    if (sinkDataItem.Orientation == ConnectorOrientation.Output || sinkDataItem.Orientation == ConnectorOrientation.TrueOutput ||sinkDataItem.Orientation == ConnectorOrientation.FalseOutput)
                    {
                        //Need to remove last item as we did not finish drawing the path
                        int indexOfLastTempConnection = sourceDataItem.DataItem.Parent.Items.Count - 1;
                        sourceDataItem.DataItem.Parent.RemoveItemCommand.Execute(sourceDataItem.DataItem.Parent.Items[indexOfLastTempConnection]);
                        MessageBox.Show("Die Verknüpfung an einen Ausgang ist nicht erlaubt.");
                    }
                    else if (sourceDataItem.DataItem.GetType().Name == "StartStepViewModel" && sinkDataItem.DataItem.GetType().Name == "ForkViewModel") 
                    {
                        //Need to remove last item as we did not finish drawing the path
                        int indexOfLastTempConnection = sourceDataItem.DataItem.Parent.Items.Count - 1;
                        sourceDataItem.DataItem.Parent.RemoveItemCommand.Execute(sourceDataItem.DataItem.Parent.Items[indexOfLastTempConnection]);
                        MessageBox.Show("Eine Bedingung darf nicht unmittelbar auf einen Startzustand folgen.");
                    }
                    else
                    {
                        int indexOfLastTempConnection = sinkDataItem.DataItem.Parent.Items.Count - 1;
                        sinkDataItem.DataItem.Parent.RemoveItemCommand.Execute(sinkDataItem.DataItem.Parent.Items[indexOfLastTempConnection]);
                        sinkDataItem.DataItem.Parent.AddItemCommand.Execute(new ConnectorViewModel(sourceDataItem, sinkDataItem));

                        if (sourceDataItem.DataItem.GetType().Name == "ForkViewModel")
                        {
                            if (sourceDataItem.Orientation == ConnectorOrientation.TrueOutput)
                            {
                                sourceDataItem.DataItem.enableTopConnector = false;
                            }
                            else if(sourceDataItem.Orientation == ConnectorOrientation.FalseOutput)
                            {
                                sourceDataItem.DataItem.enableBottomConnector = false;
                            }
                        }
                        else
                        {
                            sourceDataItem.DataItem.enableRightConnector = false;
                        }
                        sinkDataItem.DataItem.enableInputConnector = false;
                    }
                }
                else
                {
                    //Need to remove last item as we did not finish drawing the path
                    int indexOfLastTempConnection = sourceDataItem.DataItem.Parent.Items.Count - 1;
                    sourceDataItem.DataItem.Parent.RemoveItemCommand.Execute(sourceDataItem.DataItem.Parent.Items[indexOfLastTempConnection]);
                }
            }
            connectorsHit = new List<Connector>();
            sourceConnector = null;
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if(SourceConnector != null)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Point currentPoint = e.GetPosition(this);
                    partialConnection.SinkConnectorInfo = new PartCreatedConnectionInfo(currentPoint);
                    HitTesting(currentPoint);
                }
            }
            else
            {
                // if mouse button is not pressed we have no drag operation, ...
                if (e.LeftButton != MouseButtonState.Pressed)
                    rubberbandSelectionStartPoint = null;

                // ... but if mouse button is pressed and start
                // point value is set we do have one
                if (this.rubberbandSelectionStartPoint.HasValue)
                {
                    // create rubberband adorner
                    AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                    if (adornerLayer != null)
                    {
                        RubberbandAdorner adorner = new RubberbandAdorner(this, rubberbandSelectionStartPoint);
                        if (adorner != null)
                        {
                            adornerLayer.Add(adorner);
                        }
                    }
                }
            }
            e.Handled = true;
        }


        protected override Size MeasureOverride(Size constraint)
        {
            Size size = new Size();

            foreach (UIElement element in this.InternalChildren)
            {
                double left = Canvas.GetLeft(element);
                double top = Canvas.GetTop(element);
                left = double.IsNaN(left) ? 0 : left;
                top = double.IsNaN(top) ? 0 : top;

                //measure desired size for each child
                element.Measure(constraint);

                Size desiredSize = element.DesiredSize;
                if (!double.IsNaN(desiredSize.Width) && !double.IsNaN(desiredSize.Height))
                {
                    size.Width = Math.Max(size.Width, left + desiredSize.Width);
                    size.Height = Math.Max(size.Height, top + desiredSize.Height);
                }
            }
            // add margin 
            size.Width += 10;
            size.Height += 10;
            return size;
        }

        private void HitTesting(Point hitPoint)
        {
            DependencyObject hitObject = this.InputHitTest(hitPoint) as DependencyObject;
            while (hitObject != null &&
                    hitObject.GetType() != typeof(DesignerCanvas))
            {
                if (hitObject is Connector)
                {
                    if (!connectorsHit.Contains(hitObject as Connector))
                        connectorsHit.Add(hitObject as Connector);
                }
                hitObject = VisualTreeHelper.GetParent(hitObject);
            }

        }


        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            DragObject dragObject = e.Data.GetData(typeof(DragObject)) as DragObject;
            if (dragObject != null)
            {
                (DataContext as IDiagramViewModel).ClearSelectedItemsCommand.Execute(null);
                Point position = e.GetPosition(this);
                DesignerItemViewModelBase itemBase = (DesignerItemViewModelBase)Activator.CreateInstance(dragObject.ContentType);
                itemBase.Left = Math.Max(0, position.X - itemBase.ItemWidth / 2);
                itemBase.Top = Math.Max(0, position.Y - itemBase.ItemHeight / 2);
                itemBase.IsSelected = true;
                (DataContext as IDiagramViewModel).AddItemCommand.Execute(itemBase);
            }
            e.Handled = true;
        }
    }
}
