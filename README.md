Animate-Sprite
===========================================
This is a simple XNA 4.0 Library which will allow you to easily create animated sprites using xml and the standard content pipeline.

This library will allow you to load sections of an image, often refered to as a sprite sheet, and render each sub image in a 
specified order.

Version History
===========================================
Version 1.0 - Created the basis of utilizing sprites and the XNA content reader and writer to allow for sprites to be declared using
			  XML and images. These sprites are also unique in that they have the ability to return which pixels or points are filled
			  using opaque color data (as determined by  the image/renderer).
			  
Version 1.1 - Added custom events when the animation finished looping through; this event is thrown when the animation is completed,
			  whether or not the animation resets.
			  
Version 1.2 - Added overrides for GetOpaqueData(). First override GetOpaqueData(int alpha) returns the data array based on the pixel
			  having an alpha greater than or equal to the alpha that user specifies rather than the pixel having to have an aplha 
			  of 255. The second override determines opacity based on if the color of the pixel is equal to the color input by the 
			  user; if the colors match, the pixel is deemed transparent.
			  
Version 1.3 - Added Draw() overrides so that special sprite effects can be applied. Expanded the GetOpaqueData() so that more choices
			  can be made to have the sprites work how a user wants them to.
			 
Version 2.0 - Added Rotation, Scale, Origin, Transform, Bounds, CurrentRectange, and OpaqueData. Removed GetOpaqueData(). Allow for 
			  pixel perfect collision detection to be done even when the sprite is rotated or scaled.
			  
To Do List
============================================
[X] Pixel Perfect Collision Detection
[X] Transformation Pixel Perfect Collision Detection
[X] Eliminate Slow Down from Collision Calculation
[ ] Allow for regions to be defined for collision
[ ] Write How To

How To Use this library.
============================================
How To Write XML for Sprites
--------------------------------------------
Coming Soon

How to Load Sprites
--------------------------------------------
Coming Soon

How to Choose Colors for Collision
--------------------------------------------
Coming Soon

How to Detect Collision
--------------------------------------------
Coming Soon

How to Apply Transformations to Sprite
--------------------------------------------
Coming Soon

Notes
============================================
The Sonic Advance 3 images are all owned by SEGA and are used only for purposes of demonstration, not for reproduction use in a 
distributed product for gain.


Copyright 2010, 2011, 2013 Alexander Lyons