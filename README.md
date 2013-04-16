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
			  pixel perfect collision detection to be done even when the sprite is rotated or scaled. Removed some of the draw overrides, 
			  will add in what I think needs to be added back in. (Possibly on request).
			  
Version 2.1 - Added ChangeFrame Event, which will tell the caller when a frame has changed and what frame it has changed to. Added the
			  SpriteEffects to handle what effects will be applied to the sprite.
			  
Feature Set
============================================
Done
--------------------------------------------
 * Pixel Perfect Collision Detection (Done)
 * Transformation Pixel Perfect Collision Detection (Done)
 * Eliminate Slow Down from Collision Calculation (Done)
 * Change Frame Event (Done)
 * End of Animation Event (Done)
 * SpriteEffect changes collision area (Done)

To Do
--------------------------------------------
 * Update Example
 * Write Read Me

Might Do
-------------------------------------------
 * Allow for specific frames to throw ChangeFrame Events
 * Allow for regions to be defined for collision
 * Allow for ractangles to be predefined in Sprite XML
 * Allow for collision regions to be predefined in Sprite XML
 * Allow for detection colors to be predefined in Sprite XML
 * Create Version for XNA 4.5
 
Low Possibility
-------------------------------------------
 * Update Sprite to allow for 2.5D gaming (i.e. layering)
 * Create a Utility to create sprites from textures

How To Use this library.
============================================
How To Write XML for Sprites
--------------------------------------------
Below I am just going to provide some examples of what the end result of the xml files will look like. There is a strong chance
that I end up not writing a small application to help create the sprites, but who knows I might.

Sprite.xml

		<?xml version="1.0" encoding="utf-8" ?>
		<XnaContent xmlns:Generic="SpriteLibrary.Sprite">
		  <Name>One Sprite</Name>
		  <FPS>12</FPS>
		  <TextureAsset>SomeSpriteSheet</TextureAsset>
		  <RectangleAsset>OneSpriteRects</RectangleAsset>
		</XnaContent>

SpriteList.xml

		<?xml version="1.0" encoding="utf-8" ?>
		<XnaContent xmlns:Generic="System.Collections.Generic">
		  <Asset Type="Generic:List[SpriteLibrary.Sprite]">
			<Item>
			  <Name>One Sprite</Name>
			  <FPS>12</FPS>
			  <TextureAsset>SomeSpriteSheet</TextureAsset>
			  <RectangleAsset>OneSpriteRects</RectangleAsset>
			</Item>
			<Item>
			  <Name>Two Sprite</Name>
			  <FPS>0</FPS>
			  <TextureAsset>SomeSpriteSheet</TextureAsset>
			  <RectangleAsset>TwoSpriteRects</RectangleAsset>
			</Item>
		  </Asset>
		</XnaContent>

OneSpriteRects.xml

		<?xml version="1.0" encoding="utf-8" ?>
		<XnaContent xmlns:Generic="System.Collections.Generic">
		  <Asset Type="Generic:List[string]">
			<Item>6 10 32 35</Item>
			<Item>42 10 32 35</Item>
			<Item>78 10 32 35</Item>
			<Item>114 10 32 35</Item>
			<Item>150 10 32 35</Item>
			<Item>186 10 32 35</Item>
			<Item>222 10 32 35</Item>
			<Item>258 10 32 35</Item>
		  </Asset>
		</XnaContent>

For the Items here, they are strings delimited with spaces to give the x and y coordinates for a selection of the image, 
as well as the width and the height of each rectangle. This means that each item could potentially be a different size, 
depending on your needs. Essentially, you are slicing a rectangle from the sprite sheet.

How to Load Sprites
--------------------------------------------
It is pretty simple to import a sprite:

        Sprite someSprite = Content.Load<Sprite>(@"SpriteXML");
		
Yeah, it is that simple.

For the List example above, you would use:

        List<Sprite> sprites = Content.Load<List<Sprite>>(@"SpriteListXML");
		
I know, mind blowing.

You will need three things to truly load a sprite; and xml file defining the sprite, an xml file defining the rectangles of the file, and
the image you are using to pull the sprites from.

Note: Please be sure you have both libraries present as references for your project as well as the using statements to utilize the libraries.

How to Choose Colors for Collision
--------------------------------------------
Coming Soon

How to Detect Collision
--------------------------------------------
Honestly, this is not the hard part, it's choosing when to check for collisions, and who to check against(i.e. which other sprites).

		spriteA.Collide(spriteB);

The Collide function returns wether or not two sprites have overlapping collidable pixels.

How to Apply Transformations to Sprite
--------------------------------------------
Just use the Position, Rotation, and Scale protperties. This will ensure that everything is handled correctly(or so I hope).

Something Akin to an API
============================================
Constructors
--------------------------------------------
<dl>
<dt>Sprite()</dt>
<dd>Creates a new instance of class Sprite. You should really stick to using Content.Load<Sprite>().</dd>
</dl>

Events
--------------------------------------------
<dl>
<dt>EndOfAnimation</dt>
<dd>This alerts when the sprite has gone past it's last frame (i.e. hit frame zero).</dd>
<dt>ChangeFrame</dt>
<dd>This alerts when the sprite has changed frame.</dd>
</dl>

Properties
--------------------------------------------
<dl>
<dt>Name</dt>
<dd>Just the name of the sprite.</dd>
<dt>FPS (int)</dt>
<dd>Frames per second. States how many frames the sprite will try to display per second.</dd>
<dt>TextureAsset (String)</dt>
<dd>A string which points to the texture used as the sprite sheet.</dd>
<dt>RectangleAsset (String)</dt>
<dd>A string which points to the xml which defines the rectangles</dd>
<dt>Texture (Texture2D)</dt>
<dd>The texture used to house the sprites.</dd>
<dt>Rectangles (List<Rectangle>)</dt>
<dd>The list of rectangles which defines the sprite.</dd>
<dt>Position (Vector2)</dt>
<dd>The position on the screen at which the sprite resides. This is set by the centeer of the sprite(Origin).</dd>
<dt>Rotation (float)</dt>
<dd>The rotation of the sprite. 0.0f is a upright to the sprite sheet. Rotation is measured in radians.</dd>
<dt>Scale (float)</dt>
<dd>The scale at which the sprite is set. 1.0f is pixel for pixel, greater than 1.0f is larger, less than 1.0f is smaller.</dd>
<dt>Origin (Vector2)</dt>
<dd>This will tell you the center of the sprite, relative to the top left hand corner of the rectangle defining the image.</dd>
<dt>Bounds (Rectangle)</dt>
<dd>Defines the current space that the sprite is occupying (including empty space). This takes into account Position, Rotation, and Scale(the Bound rectangle itself does not rotate, but it will contain all of the rotated sprite).</dd>
<dt>OpaqueData (bool[,])</dt>
<dd>Returns a 2D array of booleans which corresponds with which pixels have color in them. This data is not rotated nor scaled.</dd>
<dt>SpriteEffects</dt>
<dd>The sprite effects being applied to the sprite.</dd>
</dl>

Methods
--------------------------------------------
<dl>
<dt>void Load(ContentManager content)</dt>
<dd>This method loads the texture and rectangles to make the sprite. This is automatically done when using Content.Load<Sprite>()</dd>
<dt>void Update(GameTime gameTime)</dt>
<dd>This method takes in the current game time, and handles updating the sprite. Well, more proceeding through frames than anything else.</dd>
<dt>void Draw(SpriteBatch spriteBatch)</dt>
<dd>This draws the sprite.</dd>
<dt>void Draw(SpriteBatch spriteBatch, Color color)</dt>
<dd>Allows for a color to be applied to the draw.</dd>
<dt>BuildOpaqueData</dt>
<dd>To be defined later.</dd>
<dt>bool Collide(Sprite other)</dt>
<dd>Tells whether or not two sprites are colliding.</dd>
<dt>List<Vector2> PointsOfCollision(Sprite other)</dt>
<dd>Tells the points at which two sprites collide.</dd>
</dl>

Note: This section is still incomplete.

Frequently Asked Questions
============================================
Okay, so there is nothing here right now, but the way I see it, this probably won't have too much in it.

Notes
============================================
Warnings
--------------------------------------------
<dl>
<dt>Computationally Expensive</dt>
<dd>Using this library can be computationally expensive. Be careful when using something like this on a phone, and be sure to test your code,
not just for functionality, but also for speed and check for slow down. You don't know what your user's device will be or necessarily
what the specs are going to be so make sure you test.</dd>
</dl>

Thanks
--------------------------------------------
Just a shout out to some people who have helped me build this project

 * Dr. Preston - For being an awesome teacher, and inspiring me to complete this library
 * Will - For looking over my code and making sure I don't make too many Ryan mistakes
 * Ryan - For letting me make fun of you for your bad coding
 * Karlos - For helping me to understand legal jargon and such so that people can  use this library readily.
 * Kyle - For letting me use him as a guinea pig... I mean tester. 

The Sonic Advance 3 images are all owned by SEGA and are used only for purposes of demonstration, not for reproduction use in a 
distributed product for gain.

Contact me at pyroticblaziken@gmail.com

Please, before you email me questions concerning this, would you be so kind as to read through my document here. I really tried to make it at 
least some what thourough, so if I get a question of "how do I load a sprite?", I probably won't answer. Also, if you find any bugs or anything
let me know so that I can go through and change them. I will probably have a way to do forks and such in the future, but for now, just email me.

Copyright 2010, 2011, 2013 Alexander Lyons