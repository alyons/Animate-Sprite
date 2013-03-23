Animate-Sprite
Copyright 2011, 2013 Alexander Lyons
===========================================
This is a simple XNA 4.0 Library which will allow you to easily create animated sprites using xml and the standard content pipeline.

This library will allow you to load sections of an image, often refered to as a sprite sheet, and render each sub image in a 
specified order. The sprite can be a looped or single run animation.

Version History
===========================================
Version 1.0 - Created the basis of utilizing sprites and the XNA content reader and writer to allow for sprites to be declared using
			  XML and images. These sprites are also unique in that they have the ability to return which pixels or points are filled
			  using opaque color data (as determined by  the image/renderer.
Version 1.1 - Added custom events when the animation finished looping through; this event is thrown when the animation is completed,
			  whether or not the animation resets.
Version 1.2 - Added overrides for GetOpaqueData(). First override GetOpaqueData(int alpha) returns the data array based on the pixel
			  having an alpha greater than or equal to the alpha that user specifies rather than the pixel having to have an aplha 
			  of 255. The second override determines opacity based on if the color of the pixel is equal to the color input by the 
			  user; if the colors match, the pixel is deemed transparent.

How To Use
============================================