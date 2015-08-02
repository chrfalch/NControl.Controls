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
			// Layout
			var layout = new RelativeLayout ();

			// Header
			_monthYearLabel = new Label {
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				XAlign = TextAlignment.Center,
				YAlign = TextAlignment.Center,
				HeightRequest = TopHeight,
			};

			_monthYearLabel.SetBinding (Label.TextProperty, MonthYearStringProperty.PropertyName);

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
			prevMonthBtn.Command = new Command((obj) => 
				this.SelectedDate = this.SelectedDate.AddMonths (-1));

			// Next month
			var nextMonthBtn = new RippleButton{
				BackgroundColor = Color.Transparent,
				FontFamily = FontAwesomeLabel.FontAwesomeName,
				HorizontalOptions = LayoutOptions.End,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = TopHeight,
				WidthRequest = 34,
				TextColor = Color.FromHex("#AAAAAAA"),
				Text = FontAwesomeLabel.FAChevronRight,
			};
			nextMonthBtn.Command = new Command((obj) =>
				this.SelectedDate = this.SelectedDate.AddMonths (1));

			var headerLayout = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = {
					prevMonthBtn, _monthYearLabel, nextMonthBtn
				}
			};

			layout.Children.Add (headerLayout, ()=> new Rectangle(0, 0, 
				layout.Width, TopHeight));

			// Day names
			var dayNames = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
			var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

			var dayGrid = new Grid {
				HeightRequest = DayNamesHeight,
			};

			var currentWeekDay = firstDayOfWeek;
			for (var d = 0; d < 7; d++) {
				var label = new Label{
					BackgroundColor= Color.Transparent,
					XAlign = TextAlignment.Center,
					YAlign = TextAlignment.Center,
					Text = dayNames[(int)currentWeekDay]
				};

				_dayNameLabels [d] = label;
				currentWeekDay++;
				if((int)currentWeekDay == 7)
					currentWeekDay = 0;

				dayGrid.Children.Add (label, d, 0);
			}

			layout.Children.Add(dayGrid, ()=> new Rectangle (0, TopHeight,
				layout.Width, DayNamesHeight));

			// Calendar
			_calendar = new NControlView{
				DrawingFunction = DrawCalendar,
			};

			layout.Children.Add (_calendar, ()=> new Rectangle (
				0, TopHeight+DayNamesHeight,
				layout.Width, layout.Height - (TopHeight + DayNamesHeight)));

			// Day Number Labels
			var lc = 0;
			var dayNumberGrid = new Grid { 				
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

			layout.Children.Add (dayNumberGrid, ()=> new Rectangle (
				0, TopHeight+DayNamesHeight,
				layout.Width, layout.Height - (TopHeight + DayNamesHeight)));

			for (var r = 0; r < 6; r++) {
				for (var c = 0; c < 7; c++) {
					
					var dayLabel = new Label{
						XAlign = TextAlignment.Center,
						YAlign = TextAlignment.Center,
						TextColor = Color.Black,
						BackgroundColor = Color.Transparent,
						Text = "A" + lc.ToString(),
					};
						
					dayNumberGrid.Children.Add (dayLabel, c, r);
					_dayNumberLabels [lc++] = dayLabel;
				}
			}

			// Ellipse
			_ellipse = new NControlView {
				BackgroundColor = Color.Transparent,
				DrawingFunction = (canvas, rect) =>{
					canvas.DrawEllipse(rect, null, 
						new NGraphics.SolidBrush(Color.FromHex("#DDDDDD").ToNColor()));
				},
				Scale = 0.0,
			};	

			layout.Children.Add (_ellipse, () => new Rectangle (
				0, TopHeight + DayNamesHeight, 
				Math.Min(_calendar.Width / 7, _calendar.Height/7),
				Math.Min(_calendar.Width / 7, _calendar.Height/7)
				)
			);

			Content = layout;
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

			_ellipse.Scale = 0.0;
			_ellipse.Opacity = 1.0;

			p.Y += ((_calendar.Height / 6) / 2);

			_ellipse.TranslationX = Math.Floor(p.X / (_calendar.Width/7)) * (_calendar.Width/7);
			_ellipse.TranslationY = Math.Floor(p.Y / (_calendar.Height/6)) * 
				(_calendar.Height/6) - (TopHeight + DayNamesHeight + ((_calendar.Height/6)/2));

			_ellipse.ScaleTo (2);
			_ellipse.FadeTo (0.0);

			// Update selected date
			var col = (int)Math.Round(_ellipse.TranslationX / (_calendar.Width / 7));
			var row = (int)Math.Round(_ellipse.TranslationY / (_calendar.Height / 6));
			var choosenDate = StartDate.AddDays(col + (row * 7));			
			var callEvent = (choosenDate.Month == SelectedDate.Month);

            SelectedDate = new DateTime(choosenDate.Year, choosenDate.Month, choosenDate.Day,
                SelectedDate.Hour, SelectedDate.Minute, SelectedDate.Second, DateTimeKind.Utc);

			// Call event
			if (callEvent && OnDateSelected != null)
				OnDateSelected (this, EventArgs.Empty);
		
			return true;
		}

		/// <summary>
		/// Draw the specified canvas.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
		/// <param name="rect">Rect.</param>
		public void DrawCalendar (NGraphics.ICanvas canvas, NGraphics.Rect rect)
		{
			base.Draw (canvas, rect);

			// Should we use last line?
			// var drawLastLine = true;

			var colWidth = rect.Width/7;
			var rowHeight = rect.Height/6;

			// Find first week of day
			var firstWeekDay = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

			// Find last of this day in previous month
			var lastDayInPrevMonth = GetLastDayInMonth(SelectedDate.AddMonths(-1));
			while (lastDayInPrevMonth.DayOfWeek != firstWeekDay)
				lastDayInPrevMonth = lastDayInPrevMonth.AddDays (-1);

			// Set up brushes
			var selectedBrush = new NGraphics.SolidBrush(SelectedDateColor.ToNColor());
			var prevNextBrush = new NGraphics.SolidBrush(Color.FromHex("#EEEEEE").ToNColor());

			var currentDate = lastDayInPrevMonth;
			StartDate = lastDayInPrevMonth;

			var col = 0;
			var row = 0;

			for (int d = 0; d < _dayNumberLabels.Length; d++) {

				// Update text
				_dayNumberLabels [d].Text = currentDate.Day.ToString();

				if (currentDate.Month != SelectedDate.Month) {

					// Prev/next month
					_dayNumberLabels [d].TextColor = Color.FromHex ("#CECECE");

				} 
				else 
				{

					// Background
					canvas.DrawRectangle (new NGraphics.Rect (col * colWidth, row * rowHeight,
						colWidth, rowHeight), null, prevNextBrush);
					
					if (currentDate.DayOfWeek == DayOfWeek.Sunday ||
					    currentDate.DayOfWeek == DayOfWeek.Saturday) {

						// Weekends
						_dayNumberLabels [d].TextColor = Color.FromHex ("#CACACA");

					} else {
					
						// Regular days
						_dayNumberLabels [d].TextColor = Color.Black;
					}
				}


				if (IsSameDay (SelectedDate, currentDate)) {
					
					// Selected colors
					_dayNumberLabels[d].TextColor = Color.White;

					// Background
					canvas.DrawRectangle (new NGraphics.Rect (col * colWidth, row * rowHeight,
						colWidth, rowHeight), null, selectedBrush);
				}

                if (IsSameDay (DateTime.Now, currentDate)) {

                    // Today
                    _dayNumberLabels[d].TextColor = Color.White;

                    // Background
                    var wh = Math.Min(colWidth, rowHeight);
                    var rc = new NGraphics.Rect ((col * colWidth), (row * rowHeight), wh, wh);
                    rc.Inflate (-1, -1);
                    rc.X+=3;                    

                    canvas.DrawEllipse (rc, null, new NGraphics.SolidBrush(Color.FromHex("#fc3d39").ToNColor()));
                }
					
				// Col/row-counter
				col++;
				if (col == 7) {
					col = 0;
					row++;
				}

				currentDate = currentDate.AddDays (1);
			}

			var colRowPen = new NGraphics.Pen (Color.FromHex ("#FFFFFF").ToNColor (), 1);

			// Draw row lines
			for (var r = 0; r < 7; r++) {
				canvas.DrawPath (new NGraphics.PathOp[] {
					new NGraphics.MoveTo(0, r *rowHeight), 
					new NGraphics.LineTo(rect.Width, r*rowHeight) 
				}, colRowPen);
			}

			// Draw col lines
			for (var c = 0; c < 7; c++) {
				canvas.DrawPath (new NGraphics.PathOp[] {
					new NGraphics.MoveTo(c*colWidth, 0), 
					new NGraphics.LineTo(c*colWidth, rect.Height)

				}, colRowPen);
			}	

		}

		/// <summary>
		/// Gets or sets the start date.
		/// </summary>
		/// <value>The start date.</value>
		private DateTime StartDate {
			get;
			set;
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
		/// The SelectedDateColor property.
		/// </summary>
		public static BindableProperty SelectedDateColorProperty = 
			BindableProperty.Create<CalendarView, Color> (p => p.SelectedDateColor, Color.Accent,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (CalendarView)bindable;
					ctrl.SelectedDateColor = newValue;
				});

		/// <summary>
		/// Gets or sets the SelectedDateColor of the CalendarView instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public Color SelectedDateColor {
			get{ return (Color)GetValue (SelectedDateColorProperty); }
			set {
				SetValue (SelectedDateColorProperty, value);
				UpdateCalendar ();
			}
		}

		/// <summary>
		/// The SelectedDate property.
		/// </summary>
		public static BindableProperty SelectedDateProperty = 
			BindableProperty.Create<CalendarView, DateTime> (p => p.SelectedDate, DateTime.MinValue,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (CalendarView)bindable;
					ctrl.SelectedDate = newValue;
				});

		/// <summary>
		/// Gets or sets the SelectedDate of the Class instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public DateTime SelectedDate {
			get { return (DateTime)GetValue (SelectedDateProperty); }
			set {
				SetValue (SelectedDateProperty, value);
				MonthYearString = value.ToString ("MMMMM yyyy");
				UpdateCalendar ();
			}
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

		/// <summary>
		/// The MonthYearString property.
		/// </summary>
		private static BindableProperty MonthYearStringProperty = 
			BindableProperty.Create<CalendarView, string> (p => p.MonthYearString, null,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (CalendarView)bindable;
					ctrl.MonthYearString = newValue;
				});

		/// <summary>
		/// Gets or sets the MonthYearString of the CalendarView instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		private string MonthYearString {
			get{ return (string)GetValue (MonthYearStringProperty); }
			set {
				SetValue (MonthYearStringProperty, value);
				_monthYearLabel.Text = value;
			}
		}

		/// <summary>
		/// Gets the first day in month.
		/// </summary>
		/// <returns>The first day in month.</returns>
		/// <param name="date">Date.</param>
		private DateTime GetFirstDayInMonth(DateTime date)
		{
			return date.AddDays(1-date.Day);
		}

		/// <summary>
		/// Gets the last day in month.
		/// </summary>
		/// <returns>The last day in month.</returns>
		/// <param name="date">Date.</param>
		public DateTime GetLastDayInMonth(DateTime date)
		{
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 
                12, 0, 0, DateTimeKind.Utc);
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

