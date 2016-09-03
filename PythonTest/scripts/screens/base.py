#!/usr/bin/env python

"""
The base class of a screen.
"""

from Microsoft.Xna.Framework import Color

from WeaponWizard import Interfaces

class BaseScreen (Interfaces.IScreen):
    bgcolor = Color.CornflowerBlue
    ret_screen = None

    def __init__(self):
        pass

    def get_BackgroundColor(self):
        return self.bgcolor

    def set_BackgroundColor(self, value):
        self.bgcolor = value

    def OnEnter(self, engine):
        self.engine = engine

    def OnExit(self):
        self.engine.ClearEntities()

    def Update(self, gametime):
        return self.ret_screen

    def Draw(self, spritebatch):
        pass