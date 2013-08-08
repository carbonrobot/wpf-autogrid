    /// <summary>
    /// Defines a flexible grid area that consists of columns and rows.
    /// Depending on the orientation, either the rows or the columns are auto-generated,
    /// and the children's position is set according to their index.
    /// </summary>
    public class AutoGrid : Grid
    {
        /// <summary>
        /// Gets or sets the child horizontal alignment.
        /// </summary>
        /// <value>The child horizontal alignment.</value>
        public HorizontalAlignment? ChildHorizontalAlignment
        {
            get { return (HorizontalAlignment?)GetValue(ChildHorizontalAlignmentProperty); }
            set { SetValue(ChildHorizontalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the child margin.
        /// </summary>
        /// <value>The child margin.</value>
        public Thickness? ChildMargin
        {
            get { return (Thickness?)GetValue(ChildMarginProperty); }
            set { SetValue(ChildMarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets the child vertical alignment.
        /// </summary>
        /// <value>The child vertical alignment.</value>
        public VerticalAlignment? ChildVerticalAlignment
        {
            get { return (VerticalAlignment?)GetValue(ChildVerticalAlignmentProperty); }
            set { SetValue(ChildVerticalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the column count
        /// </summary>
        public int ColumnCount
        {
            get { return (int)GetValue(ColumnCountProperty); }
            set { SetValue(ColumnCountProperty, value); }
        }

        /// <summary>
        /// Gets or sets the columns
        /// </summary>
        public string Columns
        {
            get { return (string)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the fixed column width
        /// </summary>
        public GridLength ColumnWidth
        {
            get { return (GridLength)GetValue(ColumnWidthProperty); }
            set { SetValue(ColumnWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the fixed row height
        /// </summary>
        public GridLength RowHeight
        {
            get { return (GridLength)GetValue(RowHeightProperty); }
            set { SetValue(RowHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the children are automatically indexed.
        /// <remarks>
        /// The default is <c>true</c>.
        /// Note that if children are already indexed, setting this property to <c>false</c> will not remove their indices.
        /// </remarks>
        /// </summary>
        public bool IsAutoIndexing
        {
            get { return (bool)GetValue(IsAutoIndexingProperty); }
            set { SetValue(IsAutoIndexingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the orientation.
        /// <remarks>The default is Vertical.</remarks>
        /// </summary>
        /// <value>The orientation.</value>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the number of rows
        /// </summary>
        public int RowCount
        {
            get { return (int)GetValue(RowCountProperty); }
            set { SetValue(RowCountProperty, value); }
        }

        /// <summary>
        /// Gets or sets the rows
        /// </summary>
        public string Rows
        {
            get { return (string)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        /// <summary>
        /// Handles the column count changed event
        /// </summary>
        public static void ColumnCountChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue < 0)
                return;

            var grid = obj as AutoGrid;

            // look for an existing column definition for the height
            var width = GridLength.Auto;
            if (grid.ColumnDefinitions.Count > 0)
                width = grid.ColumnDefinitions[0].Width;

            // clear and rebuild
            grid.ColumnDefinitions.Clear();
            for (int i = 0; i < (int)e.NewValue; i++)
                grid.ColumnDefinitions.Add(
                    new ColumnDefinition() { Width = width });
        }

        /// <summary>
        /// Handle the columns changed event
        /// </summary>
        public static void ColumnsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if ((string)e.NewValue == string.Empty)
                return;

            var grid = obj as AutoGrid;
            grid.ColumnDefinitions.Clear();

            var defs = Parse((string)e.NewValue);
            foreach (var def in defs)
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = def });
        }

        /// <summary>
        /// Handle the fixed column width changed event
        /// </summary>
        public static void FixedColumnWidthChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var grid = obj as AutoGrid;

            // add a default column if missing
            if (grid.ColumnDefinitions.Count == 0)
                grid.ColumnDefinitions.Add(new ColumnDefinition());

            // set all existing columns to this width
            for (int i = 0; i < grid.ColumnDefinitions.Count; i++)
                grid.ColumnDefinitions[i].Width = (GridLength)e.NewValue;
        }

        /// <summary>
        /// Handle the fixed row height changed event
        /// </summary>
        public static void FixedRowHeightChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var grid = obj as AutoGrid;

            // add a default column if missing
            if (grid.RowDefinitions.Count == 0)
                grid.RowDefinitions.Add(new RowDefinition());

            // set all existing columns to this width
            for (int i = 0; i < grid.RowDefinitions.Count; i++)
                grid.RowDefinitions[i].Height = (GridLength)e.NewValue;
        }

        /// <summary>
        /// Parse an array of grid lengths from comma delim text
        /// </summary>
        public static GridLength[] Parse(string text)
        {
            var tokens = text.Split(',');
            var definitions = new GridLength[tokens.Length];
            for (var i = 0; i < tokens.Length; i++)
            {
                var str = tokens[i];
                double value;

                // ratio
                if (str.Contains('*'))
                {
                    if (!double.TryParse(str.Replace("*", ""), out value))
                        value = 1.0;

                    definitions[i] = new GridLength(value, GridUnitType.Star);
                    continue;
                }

                // pixels
                if (double.TryParse(str, out value))
                {
                    definitions[i] = new GridLength(value);
                    continue;
                }

                // auto
                definitions[i] = GridLength.Auto;
            }
            return definitions;
        }

        /// <summary>
        /// Handles the row count changed event
        /// </summary>
        public static void RowCountChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue < 0)
                return;

            var grid = obj as AutoGrid;

            // look for an existing row to get the height
            var height = GridLength.Auto;
            if (grid.RowDefinitions.Count > 0)
                height = grid.RowDefinitions[0].Height;

            // clear and rebuild
            grid.RowDefinitions.Clear();
            for (int i = 0; i < (int)e.NewValue; i++)
                grid.RowDefinitions.Add(
                    new RowDefinition() { Height = height });
        }

        /// <summary>
        /// Handle the rows changed event
        /// </summary>
        public static void RowsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if ((string)e.NewValue == string.Empty)
                return;

            var grid = obj as AutoGrid;
            grid.RowDefinitions.Clear();

            var defs = Parse((string)e.NewValue);
            foreach (var def in defs)
                grid.RowDefinitions.Add(new RowDefinition() { Height = def });
        }

        /// <summary>
        /// Handled the redraw properties changed event
        /// </summary>
        private static void OnPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((AutoGrid)o)._shouldReindex = true;
        }

        /// <summary>
        /// Identifies the <see cref="ChildHorizontalAlignment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ChildHorizontalAlignmentProperty =
            DependencyProperty.Register("ChildHorizontalAlignment", typeof(HorizontalAlignment?), typeof(AutoGrid),
                new FrameworkPropertyMetadata((HorizontalAlignment?)null, FrameworkPropertyMetadataOptions.AffectsMeasure, OnPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="ChildMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ChildMarginProperty =
            DependencyProperty.Register("ChildMargin", typeof(Thickness?), typeof(AutoGrid),
                new FrameworkPropertyMetadata((Thickness?)null, FrameworkPropertyMetadataOptions.AffectsMeasure, OnPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="ChildVerticalAlignment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ChildVerticalAlignmentProperty =
            DependencyProperty.Register("ChildVerticalAlignment", typeof(VerticalAlignment?), typeof(AutoGrid),
                new FrameworkPropertyMetadata((VerticalAlignment?)null, FrameworkPropertyMetadataOptions.AffectsMeasure, OnPropertyChanged));

        /// <summary>
        /// Adds the specified number of Columns to ColumnDefinitions.
        /// Default Width is Auto
        /// </summary>
        public static readonly DependencyProperty ColumnCountProperty =
            DependencyProperty.RegisterAttached("ColumnCount", typeof(int), typeof(AutoGrid),
                new PropertyMetadata(-1, ColumnCountChanged));

        /// <summary>
        /// Sets the number of and width of all columns.
        /// </summary>
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.RegisterAttached("Columns", typeof(string), typeof(AutoGrid),
                new PropertyMetadata("", ColumnsChanged));

        /// <summary>
        /// Sets the default width of all columns. Used to evenly space columns.
        /// </summary>
        public static readonly DependencyProperty ColumnWidthProperty =
            DependencyProperty.RegisterAttached("ColumnWidth", typeof(GridLength), typeof(AutoGrid),
                new PropertyMetadata(GridLength.Auto, FixedColumnWidthChanged));

        /// <summary>
        /// Specify the fixed row height for all rows
        /// </summary>
        public static readonly DependencyProperty RowHeightProperty =
            DependencyProperty.RegisterAttached("RowHeight", typeof(GridLength), typeof(AutoGrid),
                new PropertyMetadata(GridLength.Auto, FixedRowHeightChanged));
         
        /// <summary>
        /// Identifies the <see cref="IsAutoIndexing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsAutoIndexingProperty =
            DependencyProperty.Register("IsAutoIndexing", typeof(bool), typeof(AutoGrid),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure, OnPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(AutoGrid),
                new FrameworkPropertyMetadata(Orientation.Vertical, FrameworkPropertyMetadataOptions.AffectsMeasure, OnPropertyChanged));

        /// <summary>
        /// Adds the specified number of Rows to the grid.
        /// Default Height of each row is Auto
        /// </summary>
        public static readonly DependencyProperty RowCountProperty =
            DependencyProperty.RegisterAttached("RowCount", typeof(int), typeof(AutoGrid),
                new PropertyMetadata(-1, RowCountChanged));

        /// <summary>
        /// Gets or sets the number and width or all rows
        /// </summary>
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.RegisterAttached("Rows", typeof(string), typeof(AutoGrid),
                new PropertyMetadata("", RowsChanged));

        private int _rowOrColumnCount;

        /// <summary>
        /// A value of <c>true</c> forces children to be re-indexed at the next oportunity.
        /// </summary>
        private bool _shouldReindex = true;

        #region Overrides

        /// <summary>
        /// Measures the children of a <see cref="T:System.Windows.Controls.Grid"/> in anticipation of arranging them during the <see cref="M:ArrangeOverride"/> pass.
        /// </summary>
        /// <param name="constraint">Indicates an upper limit size that should not be exceeded.</param>
        /// <returns>
        ///     <see cref="Size"/> that represents the required size to arrange child content.
        /// </returns>
        protected override Size MeasureOverride(Size constraint)
        {
            bool isVertical = Orientation == Orientation.Vertical;

            if (_shouldReindex || (IsAutoIndexing &&
                ((isVertical && _rowOrColumnCount != ColumnDefinitions.Count) ||
                (!isVertical && _rowOrColumnCount != RowDefinitions.Count))))
            {
                _shouldReindex = false;

                if (IsAutoIndexing)
                {
                    _rowOrColumnCount = (isVertical) ? ColumnDefinitions.Count : RowDefinitions.Count;
                    if (_rowOrColumnCount == 0) _rowOrColumnCount = 1;

                    int cellCount = 0;
                    foreach (UIElement child in Children)
                    {
                        cellCount += (isVertical) ? Grid.GetColumnSpan(child) : Grid.GetRowSpan(child);
                    }

                    //  Update the number of rows/columns
                    if (isVertical)
                    {
                        int newRowCount = ((cellCount - 1) / _rowOrColumnCount + 1);
                        while (RowDefinitions.Count < newRowCount)
                        {
                            RowDefinitions.Add(new RowDefinition());
                        }
                        if (RowDefinitions.Count > newRowCount)
                        {
                            RowDefinitions.RemoveRange(newRowCount, RowDefinitions.Count - newRowCount);
                        }
                    }
                    else // horizontal
                    {
                        int newColumnCount = ((cellCount - 1) / _rowOrColumnCount + 1);
                        while (ColumnDefinitions.Count < newColumnCount)
                        {
                            ColumnDefinitions.Add(new ColumnDefinition());
                        }
                        if (ColumnDefinitions.Count > newColumnCount)
                        {
                            ColumnDefinitions.RemoveRange(newColumnCount, ColumnDefinitions.Count - newColumnCount);
                        }
                    }
                }

                //  Update children indices
                int position = 0;
                foreach (UIElement child in Children)
                {
                    if (IsAutoIndexing)
                    {
                        if (isVertical)
                        {
                            Grid.SetRow(child, position / _rowOrColumnCount);
                            Grid.SetColumn(child, position % _rowOrColumnCount);
                            position += Grid.GetColumnSpan(child);
                        }
                        else
                        {
                            Grid.SetRow(child, position % _rowOrColumnCount);
                            Grid.SetColumn(child, position / _rowOrColumnCount);
                            position += Grid.GetRowSpan(child);
                        }
                    }

                    // Set margin and alignment
                    if (ChildMargin != null)
                    {
                        child.SetIfDefault(FrameworkElement.MarginProperty, ChildMargin.Value);
                    }
                    if (ChildHorizontalAlignment != null)
                    {
                        child.SetIfDefault(FrameworkElement.HorizontalAlignmentProperty, ChildHorizontalAlignment.Value);
                    }
                    if (ChildVerticalAlignment != null)
                    {
                        child.SetIfDefault(FrameworkElement.VerticalAlignmentProperty, ChildVerticalAlignment.Value);
                    }
                }
            }

            return base.MeasureOverride(constraint);
        }

        /// <summary>
        /// Called when the visual children of a <see cref="Grid"/> element change.
        /// <remarks>Used to mark that the grid children have changed.</remarks>
        /// </summary>
        /// <param name="visualAdded">Identifies the visual child that's added.</param>
        /// <param name="visualRemoved">Identifies the visual child that's removed.</param>
        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            _shouldReindex = true;

            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }

        #endregion Overrides
    }

