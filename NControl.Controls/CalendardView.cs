using System;
using Xamarin.Forms;
using System.Globalization;
using NControl.Abstractions;
using System.Linq;

namespace NControl.Controls
{
	/// <summary>
	/// Class CalendarView.
	/// </summary>
	public class CalendarView : NControlView
	{
		#region Private Members

		/// <summary>
		/// The calendar grid.
		/// </summary>
		private readonly NControlView _calendar;

		/// <summary>
		/// The month year label.
		/// </summary>
		private readonly Label _monthYearLabel;

		/// <summary>
		/// The day labels.
		/// </summary>
		private readonly Label[] _dayNameLabels = new Label[7];

		/// <summary>
		/// The day number labels.
		/// </summary>
		private readonly Label[] _dayNumberLabels = new Label[42];

		/// <summary>
		/// The ellipse that ripples.
		/// </summary>
		private readonly NControlView _ellipse;

        private DateTime _currentMonth;
        private DateTime _firstDate;
        private DateTime _lastDate;

		#endregion

		#region Events

		/// <summary>
		/// Occurs when on date selected.
		/// </summary>
		public event EventHandler OnDateSelected;

		#endregion

		#region Constants

		/// <summary>
		/// The height of the top.
		/// </summary>
		private const double TopHeight = 34;

		/// <summary>
		/// The height of the day names.
		/// </summary>
		private const double DayNamesHeight = 24;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Unizite.FormsApp.Controls.CalendarView"/> class.
        /// </summary>
        public CalendarView()
        {
            _currentMonth = GetFirstDayInMonth(DateTime.Now);

            // Layout
            var layout = new StackLayout { Spacing = 0, VerticalOptions = LayoutOptions.FillAndExpand };

            // Header
            _monthYearLabel = new Label {
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                XAlign = TextAlignment.Center,
                YAlign = TextAlignment.Center,
                HeightRequest = TopHeight,
            };

            // Prev month
            var prevMonthBtn = new RippleButton {
                BackgroundColor = Color.Transparent,
                FontFamily = FontAwesomeLabel.FontAwesomeName,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = TopHeight,
                WidthRequest = 34,
                TextColor = Color.FromHex("#AAAAAA"),
                Text = FontAwesomeLabel.FAChevronLeft,
            };
            prevMonthBtn.Command = new Command(
                (obj) =>
                {
                    _currentMonth = _currentMonth.AddMonths(-1);
                    UpdateCalendar();
                },
                (obj) => _currentMonth > MinDate - MinDate.TimeOfDay);

            // Next month
            var nextMonthBtn = new RippleButton {
                BackgroundColor = Color.Transparent,
                FontFamily = FontAwesomeLabel.FontAwesomeName,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = TopHeight,
                WidthRequest = 34,
                TextColor = Color.FromHex("#AAAAAA"),
                Text = FontAwesomeLabel.FAChevronRight,
            };
            nextMonthBtn.Command = new Command(
                (obj) =>
                {
                    _currentMonth = _currentMonth.AddMonths(1);
                    UpdateCalendar();
                },
                (obj) => _currentMonth.AddMonths(1) <= MaxDate);

            var headerLayout = new StackLayout {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    prevMonthBtn, _monthYearLabel, nextMonthBtn
                }
            };

            layout.Children.Add(headerLayout);

            // Day names
            var dayNames = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
            var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

            var dayGrid = new Grid {
                HeightRequest = DayNamesHeight,
            };

            var currentWeekDay = firstDayOfWeek;
            for (var d = 0; d < 7; d++) {
                var label = new Label {
                    BackgroundColor = Color.Transparent,
                    XAlign = TextAlignment.Center,
                    YAlign = TextAlignment.Center,
                    Text = dayNames[(int)currentWeekDay]
                };

                _dayNameLabels[d] = label;
                currentWeekDay++;
                if ((int)currentWeekDay == 7)
                    currentWeekDay = 0;

                dayGrid.Children.Add(label, d, 0);
            }

            layout.Children.Add(dayGrid);
            layout.Children.Add(new BoxView { HeightRequest = 8});

			// Calendar
			_calendar = new NControlView{
				DrawingFunction = DrawCalendar
			};

            var calendarLayout = new AbsoluteLayout {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand};

            calendarLayout.Children.Add(_calendar);
            AbsoluteLayout.SetLayoutBounds(_calendar, new Rectangle(0f, 0f, 1f, 1f));
            AbsoluteLayout.SetLayoutFlags(_calendar, AbsoluteLayoutFlags.All);
			
            var dayNumberGrid = CreateDayNumberGrid();
            calendarLayout.Children.Add(dayNumberGrid);
            AbsoluteLayout.SetLayoutBounds(dayNumberGrid, new Rectangle(0f, 0f, 1f, 1f));
            AbsoluteLayout.SetLayoutFlags(dayNumberGrid, AbsoluteLayoutFlags.All);

			// Ellipse
			_ellipse = new NControlView {
				BackgroundColor = Color.Transparent,
				DrawingFunction = (canvas, rect) =>{
                    var h = Math.Min(rect.Width, rect.Height);
                    var dx = (rect.Width - h)/2;
                    var dy = (rect.Height - h)/2;
                    var r = new NGraphics.Rect(dx, dy, h, h);
                    canvas.DrawEllipse(r, null, 
						new NGraphics.SolidBrush(Color.FromHex("#DDDDDD").ToNColor()));
				},
				Scale = 0.0,
			};	

            calendarLayout.Children.Add(_ellipse);
            AbsoluteLayout.SetLayoutBounds(_ellipse, new Rectangle(0f, 0f, 1/7f, 1/6f));
            AbsoluteLayout.SetLayoutFlags(_ellipse, AbsoluteLayoutFlags.All);

            layout.Children.Add(calendarLayout);
			Content = layout;

		}

        private Grid CreateDayNumberGrid()
        {
            var lc = 0;
            var dayNumberGrid = new Grid {  
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,            
                ColumnSpacing = 0, RowSpacing = 0, Padding = 0,
                ColumnDefinitions = {
                    new ColumnDefinition{Width = new GridLength(100.0/7.0, GridUnitType.Star) },
                    new ColumnDefinition{Width = new GridLength(100.0/7.0, GridUnitType.Star) },
                    new ColumnDefinition{Width = new GridLength(100.0/7.0, GridUnitType.Star) },
                    new ColumnDefinition{Width = new GridLength(100.0/7.0, GridUnitType.Star) },
                    new ColumnDefinition{Width = new GridLength(100.0/7.0, GridUnitType.Star) },
                    new ColumnDefinition{Width = new GridLength(100.0/7.0, GridUnitType.Star) },
                    new ColumnDefinition{Width = new GridLength(100.0/7.0, GridUnitType.Star) },
                },
                RowDefinitions = {
                    new RowDefinition{ Height = new GridLength(100.0/5.0, GridUnitType.Star) },
                    new RowDefinition{ Height = new GridLength(100.0/5.0, GridUnitType.Star) },
                    new RowDefinition{ Height = new GridLength(100.0/5.0, GridUnitType.Star) },
                    new RowDefinition{ Height = new GridLength(100.0/5.0, GridUnitType.Star) },
                    new RowDefinition{ Height = new GridLength(100.0/5.0, GridUnitType.Star) },
                    new RowDefinition{ Height = new GridLength(100.0/5.0, GridUnitType.Star) },
                }
            };

            for (var r = 0; r < 6; r++) {
                for (var c = 0; c < 7; c++) {

                    var dayLabel = new Label{
                        XAlign = TextAlignment.Center,
                        YAlign = TextAlignment.Center,
						HorizontalOptions = LayoutOptions.CenterAndExpand,
						VerticalOptions = LayoutOptions.CenterAndExpand,
                        TextColor = Color.Black,
						BackgroundColor = Color.Transparent,
                        Text = "A" + lc.ToString(),
                    };

                    dayNumberGrid.Children.Add (dayLabel, c, r);
                    _dayNumberLabels [lc++] = dayLabel;
                }
            }

            return dayNumberGrid;
        }

		/// <summary>
		/// Touchs down.
		/// </summary>
		/// <param name="point">Point.</param>
		/// <returns><c>true</c>, if began was touchesed, <c>false</c> otherwise.</returns>
		/// <param name="points">Points.</param>
		public override bool TouchesBegan (System.Collections.Generic.IEnumerable<NGraphics.Point> points)
		{
			base.TouchesBegan (points);

			var p = points.First ();
			if (p.Y < TopHeight + DayNamesHeight)
				return false;

            var col = (int)(p.X * 7 / _calendar.Width);
            var row = (int)((p.Y - TopHeight - DayNamesHeight) * 6 / _calendar.Height);


			var choosenDate = _firstDate.AddDays(col + (row * 7));			
            if (choosenDate > _lastDate)
                return true;

            if (choosenDate < _currentMonth && _currentMonth > MinDate - MinDate.TimeOfDay)
            {
                _currentMonth = _currentMonth.AddMonths(-1);
            }
            else if (choosenDate >= _currentMonth.AddMonths(1) && _currentMonth.AddMonths(1) <= MaxDate)
            {
                _currentMonth = _currentMonth.AddMonths(1);
            }
            else if (choosenDate >= MinDate - MinDate.TimeOfDay && choosenDate <= MaxDate)
            {
                _ellipse.TranslationX = (col * _calendar.Width / 7);
                _ellipse.TranslationY = (row * _calendar.Height /6 ); 

                _ellipse.Scale = 0.0;
                _ellipse.Opacity = 1.0;

                _ellipse.ScaleTo (2);
                _ellipse.FadeTo (0.0);

                SelectedDate = choosenDate + SelectedDate.TimeOfDay;
                if (OnDateSelected != null)
                {
                    OnDateSelected(this, EventArgs.Empty);
                }

                return true;
            }

            UpdateCalendar();

			return true;
		}

		/// <summary>
		/// Draw the specified canvas.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
		/// <param name="rect">Rect.</param>
		public void DrawCalendar (NGraphics.ICanvas canvas, NGraphics.Rect rect)
		{
            _monthYearLabel.Text = _currentMonth.ToString("MMMMM yyyy");

            base.Draw (canvas, rect);

			// Should we use last line?
			// var drawLastLine = true;

			var colWidth = rect.Width/7;
			var rowHeight = rect.Height/6;
            var hasEmptyRow = false;

			// Find first week of day
			var firstWeekDay = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            var date = _currentMonth;
            while (date.DayOfWeek != firstWeekDay)
            {
                date = date.AddDays(-1);
            }
            _firstDate = date;

			// Set up brushes
			var selectedBrush = new NGraphics.SolidBrush(SelectedBackground.ToNColor());
            var regularBrush = new NGraphics.SolidBrush(RegularBackground.ToNColor());
            var weekendBrush = new NGraphics.SolidBrush(WeekendBackground.ToNColor());
            var notInMonthBrush = new NGraphics.SolidBrush(NotInMonthBackground.ToNColor());
            var disabledBrush = new NGraphics.SolidBrush(DisabledBackground.ToNColor());

			var col = 0;
			var row = 0;

			for (int d = 0; d < _dayNumberLabels.Length; d++) {

				// Update text
                var label = _dayNumberLabels[d];
				label.XAlign = TextAlignment.Center;
				label.YAlign = TextAlignment.Center;

                if (hasEmptyRow || (date.DayOfWeek == firstWeekDay && date >= _currentMonth.AddMonths(1)))
                {
                    hasEmptyRow = true;
                    label.IsVisible = false;
                }
                else
                {
                    label.IsVisible = true;
                    label.Text = date.Day.ToString();
                    _lastDate = date;

                    if (HighlightDisabled && (date < MinDate - MinDate.TimeOfDay || date > MaxDate))
                    {
                        _dayNumberLabels[d].TextColor = DisabledColor;
                        canvas.DrawRectangle (new NGraphics.Rect (col * colWidth, row * rowHeight,
                            colWidth, rowHeight), null, disabledBrush);
                    }
                    else if (IsSameDay (SelectedDate, date))
                    {
                        // Selected date
                        _dayNumberLabels[d].TextColor = SelectedColor;
                        canvas.DrawRectangle (new NGraphics.Rect (col * colWidth, row * rowHeight,
                            colWidth, rowHeight), null, selectedBrush);
                    }
                    else if (date.Month != _currentMonth.Month)
                    {
                        // Prev/next month
                        _dayNumberLabels[d].TextColor = NotInMonthColor;
                        canvas.DrawRectangle(new NGraphics.Rect(col * colWidth, row * rowHeight, colWidth, rowHeight), null, notInMonthBrush);
                    }
                    else if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
                    {
                        // Weekends
                        _dayNumberLabels[d].TextColor = WeekendColor;
                        canvas.DrawRectangle(new NGraphics.Rect(col * colWidth, row * rowHeight, colWidth, rowHeight), null, weekendBrush);
                    }
                    else
                    {
                        // Regular days
                        _dayNumberLabels [d].TextColor = RegularColor; 
                        canvas.DrawRectangle (new NGraphics.Rect (col * colWidth, row * rowHeight, colWidth, rowHeight), null, regularBrush);
                    }


                    if (IsSameDay (DateTime.Now, date)) {

                        // Today
                        
                        // Background and color if not selected
                        if(!IsSameDay(SelectedDate, date))
                        {

                            _dayNumberLabels[d].TextColor = TodayColor;

                            var wh = Math.Min(colWidth, rowHeight);
                            var dx = (colWidth - wh) / 2;
                            var dy = (rowHeight - wh) / 2;
                            var rc = new NGraphics.Rect ((col * colWidth) + dx, (row * rowHeight) + dy, wh, wh);
                            rc.Inflate (-1, -1);       

                            canvas.DrawEllipse (rc, null, new NGraphics.SolidBrush(TodayBackground.ToNColor()));
                        }
                        else
                        {
                            _dayNumberLabels[d].TextColor = TodayBackground;
                        }
                    }
                }

				// Col/row-counter
				col++;
				if (col == 7) {
					col = 0;
					row++;
				}

                date = date.AddDays(1);
			}

            var colRowPen = new NGraphics.Pen(GridColor.ToNColor (), 1);

			// Draw row lines
            for (var r = 0; r < (hasEmptyRow ? 6 : 7) ; r++) {
				canvas.DrawPath (new NGraphics.PathOp[] {
					new NGraphics.MoveTo(0, r *rowHeight), 
					new NGraphics.LineTo(rect.Width, r*rowHeight) 
				}, colRowPen);
			}

			// Draw col lines
			for (var c = 0; c < 7; c++) {
				canvas.DrawPath (new NGraphics.PathOp[] {
					new NGraphics.MoveTo(c*colWidth, 0), 
                    new NGraphics.LineTo(c*colWidth, hasEmptyRow ? rect.Height - rowHeight : rect.Height)

				}, colRowPen);
			}	

		}

		/// <summary>
		/// This is where all the days are calculated
		/// </summary>
		private void UpdateCalendar ()
		{
			_calendar.Invalidate ();		
		}

		/// <summary>
		/// The FontFamily property.
		/// </summary>
		public static BindableProperty FontFamilyProperty = 
			BindableProperty.Create<CalendarView, string> (p => p.FontFamily, null,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (CalendarView)bindable;
					ctrl.FontFamily = newValue;
				});

		/// <summary>
		/// Gets or sets the FontFamily of the CalendarView instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public string FontFamily {
			get{ return (string)GetValue (FontFamilyProperty); }
			set {
				SetValue (FontFamilyProperty, value);
				foreach (var lbl in _dayNameLabels)
					lbl.FontFamily = value;

				foreach (var lbl in _dayNumberLabels)
					lbl.FontFamily = value;

				_monthYearLabel.FontFamily = value;
			}
		}
            
		/// <summary>
		/// The SelectedDate property.
		/// </summary>
		public static BindableProperty SelectedDateProperty = BindableProperty.Create<CalendarView, DateTime> (
            p => p.SelectedDate, DateTime.MinValue, BindingMode.TwoWay,
            propertyChanged: (b, o, n) => { ((CalendarView)b).UpdateCalendar(); });

		/// <summary>
		/// Gets or sets the SelectedDate of the Class instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public DateTime SelectedDate {
			get { return (DateTime)GetValue (SelectedDateProperty); }
			set { SetValue(SelectedDateProperty, value); }
		}

        public static BindableProperty MinDateProperty = BindableProperty.Create<CalendarView, DateTime>(
            p => p.MinDate, DateTime.MinValue,
            propertyChanged: (b, o, n) => { ((CalendarView)b).UpdateCalendar(); });

        public DateTime MinDate
        {
            get  { return (DateTime)GetValue(MinDateProperty); }
            set  { SetValue(MinDateProperty, value); } 
        }

        public static BindableProperty MaxDateProperty = BindableProperty.Create<CalendarView, DateTime>(
            p => p.MaxDate, DateTime.MaxValue,
            propertyChanged: (b, o, n) => { ((CalendarView)b).UpdateCalendar(); });

        public DateTime MaxDate
        {
            get  { return (DateTime)GetValue(MaxDateProperty); }
            set  { SetValue(MaxDateProperty, value); } 
        }

        public static BindableProperty HighlightDisabledProperty = BindableProperty.Create<CalendarView, bool>(
            p => p.HighlightDisabled, true,
            propertyChanged: (b, o, n) => { ((CalendarView)b).UpdateCalendar(); });

        public bool HighlightDisabled
        {
            get { return (bool)GetValue(HighlightDisabledProperty); }
            set { SetValue(HighlightDisabledProperty, value); }
        }

        public static BindableProperty DisabledColorProperty = BindableProperty.Create<CalendarView, Color>(
            p => p.DisabledColor, Color.White,
            propertyChanged: (b, o, n) => { ((CalendarView)b).UpdateCalendar(); });
        
        #region Colors

        public Color DisabledColor
        {
            get { return (Color)GetValue(DisabledColorProperty); }
            set { SetValue(DisabledColorProperty, value); }
        }

        public static BindableProperty DisabledBackgroundProperty = BindableProperty.Create<CalendarView, Color>(
            p => p.DisabledBackground, Color.Gray,
            propertyChanged: (b, o, n) => { ((CalendarView)b).UpdateCalendar(); });

        public Color DisabledBackground
        {
            get { return (Color)GetValue(DisabledBackgroundProperty); }
            set { SetValue(DisabledBackgroundProperty, value); }
        }

        public static BindableProperty SelectedColorProperty = BindableProperty.Create<CalendarView, Color>(
            p => p.SelectedColor, Color.White,
            propertyChanged: (b, o, n) => { ((CalendarView)b).UpdateCalendar(); });

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        public static BindableProperty SelectedBackgroundProperty = BindableProperty.Create<CalendarView, Color>(
            p => p.SelectedBackground, Color.Accent,
            propertyChanged: (b, o, n) => { ((CalendarView)b).UpdateCalendar(); });

        public Color SelectedBackground
        {
            get { return (Color)GetValue(SelectedBackgroundProperty); }
            set { SetValue(SelectedBackgroundProperty, value); }
        }

        public static BindableProperty TodayColorProperty = BindableProperty.Create<CalendarView, Color>(
            p => p.TodayColor, Color.White,
            propertyChanged: (b, o, n) => { ((CalendarView)b).UpdateCalendar(); });
        
        public Color TodayColor
        {
            get { return (Color)GetValue(TodayColorProperty); }
            set { SetValue(TodayColorProperty, value); }
        }

        public static BindableProperty TodayBackgroundProperty = BindableProperty.Create<CalendarView, Color>(
            p => p.TodayBackground, Color.FromHex("#fc3d39"),
            propertyChanged: (b, o, n) => { ((CalendarView)b).UpdateCalendar(); });

        public Color TodayBackground
        {
            get { return (Color)GetValue(TodayBackgroundProperty); }
            set { SetValue(TodayBackgroundProperty, value); }
        }

        public static BindableProperty WeekendColorProperty = BindableProperty.Create<CalendarView, Color>(
            p => p.WeekendColor, Color.FromHex("#fc3d39"),
            propertyChanged: (b, o, n) => { ((CalendarView)b).UpdateCalendar(); });

        public Color WeekendColor
        {
            get { return (Color)GetValue(WeekendColorProperty); }
            set { SetValue(WeekendColorProperty, value); }
        }

        public static BindableProperty WeekendBackgroundProperty = BindableProperty.Create<CalendarView, Color>(
            p => p.WeekendBackground, Color.FromHex("#EEEEEE"),
            propertyChanged: (b, o, n) => { ((CalendarView)b).UpdateCalendar(); });

        public Color WeekendBackground
        {
            get { return (Color)GetValue(WeekendBackgroundProperty); }
            set { SetValue(WeekendBackgroundProperty, value); }
        }

        public static BindableProperty RegularColorProperty = BindableProperty.Create<CalendarView, Color>(
            p => p.RegularColor, Color.Black,
            propertyChanged: (b, o, n) => { ((CalendarView)b).UpdateCalendar(); });

        public Color RegularColor
        {
            get { return (Color)GetValue(RegularColorProperty); }
            set { SetValue(RegularColorProperty, value); }
        }

        public static BindableProperty RegularBackgroundProperty = BindableProperty.Create<CalendarView, Color>(
            p => p.RegularBackground, Color.FromHex("#EEEEEE"),
            propertyChanged: (b, o, n) => { ((CalendarView)b).UpdateCalendar(); });

        public Color RegularBackground
        {
            get { return (Color)GetValue(RegularBackgroundProperty); }
            set { SetValue(RegularBackgroundProperty, value); }
        }

        public static BindableProperty NotInMonthColorProperty = BindableProperty.Create<CalendarView, Color>(
            p => p.NotInMonthColor, Color.FromHex("#CECECE"),
            propertyChanged: (b, o, n) => { ((CalendarView)b).UpdateCalendar(); });

        public Color NotInMonthColor
        {
            get { return (Color)GetValue(NotInMonthColorProperty); }
            set { SetValue(NotInMonthColorProperty, value); }
        }

        public static BindableProperty NotInMonthBackgroundProperty = BindableProperty.Create<CalendarView, Color>(
            p => p.NotInMonthBackground, Color.White,
            propertyChanged: (b, o, n) => { ((CalendarView)b).UpdateCalendar(); });

        public Color NotInMonthBackground
        {
            get { return (Color)GetValue(NotInMonthBackgroundProperty); }
            set { SetValue(NotInMonthBackgroundProperty, value); }
        }

        public static BindableProperty GridColorProperty = BindableProperty.Create<CalendarView, Color>(
            p => p.GridColor, Color.White,
            propertyChanged: (b, o, n) => { ((CalendarView)b).UpdateCalendar(); });

        public Color GridColor
        {
            get { return (Color)GetValue(GridColorProperty); }
            set { SetValue(GridColorProperty, value); }
        }

		/// <summary>
		/// The BorderColor property.
		/// </summary>
		public static BindableProperty BorderColorProperty = 
			BindableProperty.Create<CalendarView, Color> (p => p.BorderColor, Color.Gray,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (CalendarView)bindable;
					ctrl.BorderColor = newValue;
				});

		/// <summary>
		/// Gets or sets the BorderColor of the CalendarView instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public Color BorderColor {
			get{ return (Color)GetValue (BorderColorProperty); }
			set {
				SetValue (BorderColorProperty, value);
				Invalidate ();
			}
		}

        #endregion

        /// <summary>
		/// Gets the first day in month.
		/// </summary>
		/// <returns>The first day in month.</returns>
		/// <param name="date">Date.</param>
		private DateTime GetFirstDayInMonth(DateTime date)
		{
			return date.Date.AddDays(1-date.Day);
		}
          

		/// <summary>
		/// Gets the last day in month.
		/// </summary>
		/// <returns>The last day in month.</returns>
		/// <param name="date">Date.</param>
		public DateTime GetLastDayInMonth(DateTime date)
		{
            return date.Date.AddDays(-date.Day).AddMonths(1);
		}

		/// <summary>
		/// Determines whether this instance is same day the specified date1 date2.
		/// </summary>
		/// <returns><c>true</c> if this instance is same day the specified date1 date2; otherwise, <c>false</c>.</returns>
		/// <param name="date1">Date1.</param>
		/// <param name="date2">Date2.</param>
		public bool IsSameDay(DateTime date1, DateTime date2)
		{
			return date1.Year == date2.Year &&
				date1.Month == date2.Month &&
				date1.Day == date2.Day;
		}

		/// <summary>
		/// Rounds up.
		/// </summary>
		/// <returns>The up.</returns>
		/// <param name="numToRound">Number to round.</param>
		/// <param name="multiple">Multiple.</param>
		private double RoundTo(double value, double factor) 
		{ 
			var retVal = Math.Round((value / (double)factor),
				MidpointRounding.AwayFromZero) * factor;			

			return retVal;
		}
	}
		
}

