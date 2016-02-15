# NControl.Controls
Sample Control implementations using the [NControl](https://github.com/chrfalch/NControl) library. Implementations for iOS/Android/Windows Phone 8/8.1 is included.

## Controls
Some of this controls are just conceptual demos of things that can be accomplished with NControl and Xamarin.Forms. Other controls are complete.

### SvgImage
Control that can read svg files using the SvgReader class found in NGraphics. The control handles resizing and redrawing within Xamarin.Forms. Uses embedded resources for svg files. To load an svg file, set the SvgAssemblyType property to a type defined in the assembly where the svg file is embedded, and provide the full resouce path for the embedded svg file to the SvgResource property.

*Thanks to Veridit IT As for support*

### BlurImageView (conceptual)
Simple but full implementation of a Xamarin.Forms Image that blurs the image. 

### CalendarView (conceptual)
Simple calendar view.

### CardPage
Popup in Xamarin.Forms with full support for view models, page and content. 

### FloatingLabel
Fully implemented floating label from [Brad Frost's Floating Label concept](http://bradfrost.com/blog/post/float-label-pattern/).

### RoundCornerView
Implements a content view with round corners and correct clipping on all platforms.

### FontAwesomeLabel
Label with constants for FontAwesome icons

### FontMaterialDesignLabel
Label with constnats for Material design icons.

### GalleryView (conceptual)
Container view with snapping and sliding.

### PagingView
Page control based on the iOS paging control.

### RippleButton (conceptual)
Material design based button with ripple effect.

### ActionButton (conceptual)
Material design based action button

### ToggleActionButton (conceptual)
Material design based action button with toggle support

### ExpandableActionButton (conceptual)
Material design based action button with expanding sub buttons.

### TabStripControl
Fully implemented TabControl.
