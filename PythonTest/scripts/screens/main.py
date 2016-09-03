#!/usr/bin/env python

"""
The main screen (startscreen) of the application
"""

from Microsoft.Xna.Framework import Color, Point
from Microsoft.Xna.Framework.Graphics import Texture2D

from WeaponWizard import Interfaces
from WeaponWizard.Elements.Gui import ImageButton, Label

from base import BaseScreen
import new_game
import transition

class MainScreen (BaseScreen):
    def __init__(self):
        self.bgcolor = Color.Red

    def OnEnter(self, engine):
        BaseScreen.OnEnter(self, engine)

        self.button = ImageButton(self.engine.Content.Load[Texture2D]("default"), "test", 200, 200, 200, 48)
        self.button.Middle = Point(200, 200)
        self.button.ButtonClicked += self.button_clicked

        self.text = Label("Test text!", 50, 50, Color.Blue)

        print("Main screen GET")

    def OnExit(self):
        BaseScreen.OnExit(self)
        print("Leaving the main screen")

    def Update(self, gametime):
        self.button.Update(self.engine, gametime)

        return self.ret_screen

    def Draw(self, spritebatch):
        spritebatch.Begin()

        self.button.Draw(spritebatch)
        self.text.Draw(spritebatch)

        spritebatch.End()

    def button_clicked(self, button):
        self.ret_screen = new_game.NewGameScreen()
