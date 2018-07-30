            Loaded += (sender, eventArgs) =>
            {
                //^^
                //{[{

                //in tabbedpivot projects the ballpoint pen is not selected by default, so we set it explicitly
                toolbar.ActiveTool = toolbar.GetToolButton(InkToolbarTool.BallpointPen);
                toolbar.ActiveTool.IsChecked = true;
                //}]}
            };