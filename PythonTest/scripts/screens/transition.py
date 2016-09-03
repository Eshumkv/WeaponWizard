#!/usr/bin/env python

"""
The screen to be used when a screen transition happens
"""

from Microsoft.Xna.Framework import Color, Rectangle

from base import BaseScreen

class Transition:
    speed = 1.5    # seconds
    color = Color.Black
    is_finished = False

    def __init__(self, to_opaque=True):
        self.to_opaque = to_opaque
        self.alpha = 0

        if not self.to_opaque:
            self.alpha = 255

    def Update(self, gametime, debug=False):
        if self.to_opaque:
            self.alpha += gametime.ElapsedGameTime.TotalMilliseconds / (self.speed * 3.3333)

            if self.alpha >= 255:
                self.is_finished = True
        else:
            self.alpha -= gametime.ElapsedGameTime.TotalMilliseconds / (self.speed * 3.3333)

            if self.alpha <= 0:
                self.is_finished = True

        if debug:
            print(self.alpha)

        self.color = Color(self.color, self.alpha)

class TransitionScreen (BaseScreen):
    transition1 = Transition()
    transition2 = Transition(False)

    def __init__(self, transition_from, transition_to, color1=None, color2=None):
        self.old_screen = transition_from
        self.next_screen = transition_to

        self.bgcolor = transition_from.bgcolor

        if color1 is None:
            color1 = Color.Black

        if color2 is None:
            color2 = Color.Black

        self.transition1.color = color1
        self.transition2.color = color2

    def OnEnter(self, engine):
        BaseScreen.OnEnter(self, engine)
        screen = self.engine.GetScreenSize()
        self.rect = Rectangle(0, 0, screen.X, screen.Y)

    def Update(self, gametime):
        if not self.transition1.is_finished:
            self.transition1.Update(gametime)
        elif not self.transition2.is_finished:
            self.transition2.Update(gametime, True)
            self.bgcolor = self.next_screen.bgcolor
        else:
            return self.next_screen

        return None

    def Draw(self, spritebatch):
        transition = None

        if not self.transition1.is_finished:
            transition = self.transition1
            self.old_screen.Draw(spritebatch)
        else:
            transition = self.transition2
            self.next_screen.Draw(spritebatch)

        spritebatch.Begin()

        spritebatch.Draw(self.engine.DummyTexture,
            destinationRectangle=self.rect,
            color=self.transition1.color)

        spritebatch.End()
